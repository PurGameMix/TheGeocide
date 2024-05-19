using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A spell channeling until he stop on a area
/// Usable for player or enemies
/// </summary>
public class ChannelSpell : MonoBehaviour
{
    public bool IsEnemySpell;
    [SerializeField]
    private Collider2D _collider2D;
    [SerializeField]
    private Effector2D _effector2D;

    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private AudioController _audioController;
    [SerializeField]
    private LayerMask _hitLayers;

    private int _damage = 5;
    private float _cd = 0.5f;
    private float _lastDamageTime = 0;
    private bool _isStop;

    private float _effectProbability;
    private HealthEffectorType _effectType;

    private Dictionary<string, ICanBeDamaged> _DamageableInAreaDico = new Dictionary<string, ICanBeDamaged>();
    private Dictionary<string, ICanBeDamaged> _AddQueue = new Dictionary<string, ICanBeDamaged>();
    private List<string> _RemoveQueue = new List<string>();

    private void FixedUpdate()
    {
        _lastDamageTime += Time.deltaTime;

        //Debug sound event problems when OnBeginAudioCompleted trigger but stop is trigger aswell
        //if (name.Contains("BasicAttack") && _isStop)
        //{
        //    if(_audioController.IsPlaying("Loop"))
        //    {
        //        Debug.Log(DateTime.UtcNow + ": Emergency stop Loop on " + name);
        //        _audioController.Stop("Loop");
        //    }            
        //}

        if (_collider2D.enabled == false)
        {
            return;
        }

        if (_lastDamageTime >= _cd)
        {
            foreach (var kvp in _DamageableInAreaDico)
            {
                kvp.Value.TakeDamage(_damage, GetEffector());
            }

            _lastDamageTime = 0;
        }

        HandleInAreaObjectQueue();
    }

    /// <summary>
    /// Change _DamageableInAreaDico dico only after traitment
    /// </summary>
    private void HandleInAreaObjectQueue()
    {
        if (_AddQueue.Count() > 0)
        {
            foreach (var element in _AddQueue)
            {
                _DamageableInAreaDico.Add(element.Key, element.Value);
            }
            _AddQueue = new Dictionary<string, ICanBeDamaged>();
        }

        if (_RemoveQueue.Count() > 0)
        {
            foreach (var element in _RemoveQueue)
            {
                _DamageableInAreaDico.Remove(element);
            }
            _RemoveQueue = new List<string>();
        }
    }
    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (!_hitLayers.Contains(hitInfo.gameObject.layer))
        {
            return;
        }
        RegisterInAreaEntity(hitInfo);
    }

    void OnTriggerExit2D(Collider2D hitInfo)
    {
        if (!_hitLayers.Contains(hitInfo.gameObject.layer))
        {
            return;
        }

        UnRegisterInAreaEntity(hitInfo);
    }

    public void Init(PlayerItemSO item)
    {
        _damage = item.Damage;
        _effectProbability = item.EffectProbability;
        _effectType = item.EffectType;
    }


    public void Init(int dmg, float tickPerSecond, HealthEffectorType et, float ep = 100)
    {
        _damage = dmg;
        _cd = 1 / tickPerSecond;
        _effectProbability = ep;
        _effectType = et;
    }

    internal void StartChannel()
    {
        _isStop = false;
        ActivePhysics(true);
        _animator.Play("Spell");
        _audioController.Play("Begin", OnBeginAudioCompleted);
        HandleHitStart();
    }

    private void ActivePhysics(bool enable)
    {
        _collider2D.enabled = enable;
        if (_effector2D != null)
        {
            _effector2D.enabled = enable;
        }
    }

    internal void StopChannel(bool destroySpell = false)
    {
        _isStop = true;
        Debug.Log($"{DateTime.UtcNow}: Stop Loop on {name}. Current state : Begin => {_audioController.IsPlaying("Begin")} // Loop => {_audioController.IsPlaying("Loop")}");
        _audioController.Stop("Loop", true);
        _audioController.Stop("Begin", true);
        ActivePhysics(false);
        UnregisterAll();
        _animator.Play("Idle");

        if (destroySpell)
        {
            _audioController.Play("End", DestroySpell);
        }
        else
        {
            _audioController.Play("End");
        }
    }

    private void UnregisterAll()
    {
        _DamageableInAreaDico = new Dictionary<string, ICanBeDamaged>();
        _AddQueue = new Dictionary<string, ICanBeDamaged>();
        _RemoveQueue = new List<string>();
    }

    void HandleHitStart()
    {
        var hitEnemies = Physics2D.OverlapBoxAll(_collider2D.bounds.center, _collider2D.bounds.size, 0, _hitLayers);

        foreach (Collider2D collider in hitEnemies)
        {
            RegisterInAreaEntity(collider);
        }
    }

    public void DestroySpell()
    {
        Destroy(gameObject);
    }


    private void OnBeginAudioCompleted()
    {
        if (_isStop)
        {
            return;
        }
        Debug.Log(DateTime.UtcNow + ": Play Loop on " + name);
        _audioController.Play("Loop");
    }

    private void RegisterInAreaEntity(Collider2D hitInfo)
    {
        var canBeDamageObj = hitInfo.GetComponent<ICanBeDamaged>();
        if (canBeDamageObj != null && !_DamageableInAreaDico.ContainsKey(hitInfo.gameObject.name))
        {
            if (!_AddQueue.ContainsKey(hitInfo.gameObject.name))
            {
                _AddQueue.Add(hitInfo.gameObject.name, canBeDamageObj);
            }
        }
    }

    private void UnRegisterInAreaEntity(Collider2D hitInfo)
    {
        if (_DamageableInAreaDico.ContainsKey(hitInfo.gameObject.name))
        {
            _RemoveQueue.Add(hitInfo.gameObject.name);
        }
    }

    private HealthEffectorType GetEffector()
    {
        var rng = UnityEngine.Random.Range(1, 101);
        if (rng <= _effectProbability)
        {
            return _effectType;
        }

        return IsEnemySpell ? HealthEffectorType.enemy : HealthEffectorType.player; //no effector
    }
}
