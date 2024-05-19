using Assets.Data.Enemy.Definition;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThreatDetector : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D _collider;

    [SerializeField]
    private ThreatDetectorChannel _tdc;

    [SerializeField]
    private ThreatDetectionType DetectorType;
    [SerializeField]
    private LayerMask _visibleLayer;

    [SerializeField]
    private EntityInfo entityInfo;

    private string _threatManagerId;

    private Dictionary<string, ThreatObjectState> _inRangeObjects = new Dictionary<string, ThreatObjectState>();

    private DetectionTarget _FocusedThreat;
    private float RayDistance;

    private float _innerRefresdCd = 1f;
    private float _currentTime = 0;
    private void Start()
    {
        RayDistance = _collider.radius+1;
        _threatManagerId = entityInfo.EntityName;
        CheckInnerDetectable();
    }

    /// <summary>
    /// Check if objects in range disapear
    /// </summary>
    private void CheckInnerDetectable()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_collider.bounds.center, _collider.radius, _visibleLayer);

        var inRangeObjects = new List<string>();
        foreach (Collider2D collider in hitEnemies)
        {
            var detectable = collider.GetComponent<CanBeDetect>();
            if (detectable != null)
            {
                inRangeObjects.Add(detectable.name);
                //Register objects in range
                RegisterDetectedObject(detectable);
            }
        }


        //object out of range
        foreach (var kvp in _inRangeObjects)
        {
            if (!inRangeObjects.Any(item => item == kvp.Key))
            {
                kvp.Value.IsRemoved = true;
            }
        }
    }

    private float ThreatInSight(Transform target)
    {
        _debugOrigin = transform.position;
        _debugTarget = target.position;

        var rayDirection = (Vector2)(target.position - transform.position);

        if(-1<= rayDirection.x && rayDirection.x <= 1 
          && -1 <= rayDirection.y && rayDirection.y <= 1
          )
        {
            return 1;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, RayDistance, _visibleLayer.value);

        if(hit.collider == null)
        {
            return 0;
        }

        var detectable = hit.collider.gameObject.GetComponent<CanBeDetect>();
        _debugHitPoint = hit.point;

        //Debug.Log("Object in sight : " + hit.transform.gameObject.name);
        return detectable != null? hit.distance : 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_visibleLayer.Contains(other.gameObject.layer))
        {
            return;
        }

        var detectable = other.GetComponent<CanBeDetect>();
        if (detectable != null)
        {
            RegisterDetectedObject(detectable);
        }
    }

    private void RegisterDetectedObject(CanBeDetect detectable)
    {
        if (_inRangeObjects.ContainsKey(detectable.name))
        {
            _inRangeObjects[detectable.name].IsRemoved = false;
            _inRangeObjects[detectable.name] = new ThreatObjectState(detectable.name, detectable.GetDPTransform());
            return;
        }
        _inRangeObjects.Add(detectable.name, new ThreatObjectState(detectable.name, detectable.GetDPTransform()));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_visibleLayer.Contains(other.gameObject.layer))
        {
            return;
        }

        var entity = other.GetComponent<CanBeDetect>();
        if (entity != null && !_inRangeObjects[entity.name].IsRemoved)
        {
            TargetLost(entity.name);
            _inRangeObjects[entity.name].IsRemoved = true;         
        }
    }

    private void FixedUpdate()
    {
        if(_inRangeObjects.Count == 0)
        {
            return;
        }

        var atLeastOnDetected = false;

        _currentTime += Time.deltaTime;
        if (_currentTime > _innerRefresdCd)
        {
            _currentTime = 0;
            //CheckInnerDetectable();
        }

        foreach (var kvp in _inRangeObjects)
        {
            if (kvp.Value.IsRemoved)
            {
                continue;
            }


            kvp.Value.ThreatDistance = ThreatInSight(kvp.Value.Target.Transform);
            if(kvp.Value.IsThreatDetected && !atLeastOnDetected)
            {
                atLeastOnDetected = true;
            }
        }

        if (atLeastOnDetected && _FocusedThreat == null)
        {
            TargetAcquired();
        }

        if(_FocusedThreat != null && !atLeastOnDetected)
        {
            TargetLost(_FocusedThreat.Name);
        }
    }

    private void TargetAcquired()
    {
        _FocusedThreat = GetFocus();

        if(_FocusedThreat != null)
        {
            _tdc.RaiseDetection(new ThreatDetectionEvent(_threatManagerId, DetectorType, _FocusedThreat));
        }
    }

    private void TargetLost(string leavingObjectName)
    {
        if (_FocusedThreat == null)
        {
            return;
        }

        if (_FocusedThreat.Name != leavingObjectName)
        {
            return;
        }

        if (DetectorType == ThreatDetectionType.Attack)
        {
            Debug.Log("TargetLost");
        }


        _inRangeObjects[leavingObjectName].IsFocus = false;
        _FocusedThreat = null;
        _tdc.RaiseDetection(new ThreatDetectionEvent(_threatManagerId, DetectorType, null));
    }

    private DetectionTarget GetFocus()
    {
        ThreatObjectState focus = null;
        foreach (var kvp in _inRangeObjects)
        {
            if (!(kvp.Value.IsThreatDetected)){
                continue;
            }

            if (kvp.Value.IsRemoved)
            {
                continue;
            }

            if (focus == null || kvp.Value.ThreatDistance < focus.ThreatDistance )
            {
                focus = kvp.Value;
            }
        }
        focus.IsFocus = true;
        return focus.Target;
    }


    //debug
    private Vector2 _debugOrigin;
    private Vector2 _debugTarget;
    private Vector2 _debugHitPoint;
    void OnDrawGizmos()
    {
        if (_debugOrigin == null || _debugTarget == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_debugOrigin, 0.05f);

        if (_debugHitPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_debugHitPoint, 0.02f);
        }

        Gizmos.color = Color.black;
        Gizmos.DrawLine(_debugOrigin, _debugTarget);
    }
}