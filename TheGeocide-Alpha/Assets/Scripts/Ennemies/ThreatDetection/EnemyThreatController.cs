using Assets.Data.Enemy.Definition;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyThreatController : MonoBehaviour
{

    [SerializeField]
    private ThreatDetectorChannel _tdc;
    private string _threatId;
    [SerializeField]
    private EntityInfo entityInfo;

    [SerializeField]
    private SuspeciousTrail _suspeciousPointPrefab;

    private string _playerColliderName = "PlayerDetection";
    //Only handle ranged are add here
    //Instantitate here to avoid multi-thread access lock
    private Dictionary<ThreatDetectionType, DetectionTarget> _detectionDico = new Dictionary<ThreatDetectionType, DetectionTarget>() {
        {ThreatDetectionType.Suspecious, null},
        {ThreatDetectionType.Aggressive, null},
        {ThreatDetectionType.Attack, null},
        {ThreatDetectionType.Close, null},
    };

    private List<Transform> _SPBuffer = new List<Transform>();

    private Transform _playerTransform;

    private void Awake()
    {
        _tdc.OnThreatDetected += OnThreatDetected;
        _playerTransform = GameObject.Find(_playerColliderName).transform;
    }

    private void OnDestroy()
    {
        _tdc.OnThreatDetected -= OnThreatDetected;
    }

    private bool IsMessageForOtherAgent(ThreatDetectionEvent detectionEvt)
    {
        return detectionEvt.RequestId != _threatId;
    }

    // Start is called before the first frame update
    void Start()
    {
        _threatId = entityInfo.EntityName;

     if (string.IsNullOrEmpty(_threatId)) {
            Debug.LogWarning($"{GetType().FullName}: _threatId as not been set for {name}");
        }  
    }

    private void OnThreatDetected(ThreatDetectionEvent detectionEvt)
    {
        if (IsMessageForOtherAgent(detectionEvt))
        {
            return;
        }

        if (detectionEvt.IsDetectionIn())
        {
            RegisterDetection(detectionEvt);
        }
        else
        {
            UnRegisterDetection(detectionEvt);
        }
    }

    private void RegisterDetection(ThreatDetectionEvent detectionEvt)
    {
        _detectionDico[detectionEvt.Type] = detectionEvt.Target;

        //Debug.Log("Detected : "+ detectionEvt.Target.Name);
    }

    private void UnRegisterDetection(ThreatDetectionEvent detectionEvt)
    {
        _detectionDico[detectionEvt.Type] = null;
    }

    internal Transform GetPlayerTransform()
    {
        return _playerTransform;
    }

    internal Transform GetFocusTarget(ThreatDetectionType tdt)
    {
        if (_detectionDico[tdt] == null)
        {
            return null;
        }

        return _detectionDico[tdt].Transform;
    }

    /// <summary>
    /// Only in aggro range
    /// </summary>
    /// <returns></returns>
    internal bool PlayerInStrictAggroRange()
    {
        var aggro = ThreatDetectionType.Aggressive;
        if (_detectionDico.ContainsKey(aggro))
        {
            if(_detectionDico[aggro] == null)
            {
                return false;
            }

            if(_detectionDico[aggro].Name == _playerColliderName){
                if (PlayerInAttackRange())
                {
                    return false;
                }

                return true;
            }
        }

        return false;
    }

    internal bool PlayerInStrictSuspeciousRange()
    {
        var aggro = ThreatDetectionType.Suspecious;
        if (_detectionDico.ContainsKey(aggro))
        {
            if (_detectionDico[aggro] == null)
            {
                return false;
            }

            if (_detectionDico[aggro].Name == _playerColliderName)
            {
                if (PlayerInAggroRange())
                {
                    return false;
                }

                return true;
            }
        }

        return false;
    }

    internal bool PlayerInAttackRange()
    {
        var attack = ThreatDetectionType.Attack;
        if (_detectionDico.ContainsKey(attack))
        {
            if (_detectionDico[attack] == null)
            {
                return false;
            }

            return _detectionDico[attack].Name == _playerColliderName;
        }

        return false;
    }

    internal bool PlayerAheadAttackRange()
    {
        var attack = ThreatDetectionType.Attack;
        if (_detectionDico.ContainsKey(attack))
        {
            if (_detectionDico[attack] == null)
            {
                return false;
            }

            if(_detectionDico[attack].Name == _playerColliderName)
            {
                var distance = transform.position - _detectionDico[attack].Transform.position;
                if(Mathf.Abs(distance.y) < 0.5f)
                {
                    return false;
                }
                var angle = AngleUtils.GetDegreeAngle(transform.position, _detectionDico[attack].Transform.position);
                
                return 45 < angle && angle < 135;
            }
            return false;
        }

        return false;
    }

    /// <summary>
    /// In aggro range or lower
    /// </summary>
    /// <returns></returns>
    internal bool PlayerInAggroRange()
    {
        var aggro = ThreatDetectionType.Aggressive;
        if (_detectionDico.ContainsKey(aggro))
        {
            if (_detectionDico[aggro] == null)
            {
                return false;
            }

            return _detectionDico[aggro].Name == _playerColliderName;
        }
        return false;
    }

    internal bool EntityInAttackRange()
    {
        var attack = ThreatDetectionType.Attack;
        if (_detectionDico.ContainsKey(attack))
        {
            return _detectionDico[attack] != null;
        }

        return false;
    }

    internal bool EntityInCloseRange()
    {
        var close = ThreatDetectionType.Close;
        if (_detectionDico.ContainsKey(close))
        {
            return _detectionDico[close] != null;
        }

        return false;
    }

    internal Transform GetSuspeciousGoalInstance(Transform playerPos)
    {

        if(_SPBuffer.Count > 5)
        {
            Debug.LogError("A little too much. ");
            return null;
        }

        if(playerPos == null)
        {
            Debug.LogError("Problem. ");
            return null;
        }
        var instance= Instantiate(_suspeciousPointPrefab, playerPos.position, playerPos.rotation).transform;
        _SPBuffer.Add(instance);

        return instance;
    }

    internal void DestroySuspeciousGoalInstance(Transform instance)
    {
       if(_SPBuffer.Any(item => item == instance))
        {

            _SPBuffer.Remove(instance);
            Destroy(instance.gameObject);
            return;
        }

        throw new KeyNotFoundException("Not found");
    }
}
