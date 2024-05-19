using Assets.Data.GameEvent.Definition;
using Assets.Scripts.Environment.Breakables;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BreakableBloc : MonoBehaviour, ICanBeDamaged
{

    public BreakableMaterialType MaterialType;


    [SerializeField]
    public List<BreakableDegradation> DegradationMap;

    [SerializeField]
    private Animator _animator;

    public string ParentId;
    public float Health = 89;

    [SerializeField]
    private AudioChannel _audioChannel;
    [SerializeField]
    private List<SpriteRenderer> _gfxList;
    [SerializeField]
    private ParticleSystem _particuleSystem;
    [SerializeField]
    private GameEventChannel _geChannel;

    private BreakableDegradation _current;




    // Start is called before the first frame update
    void Start()
    {

        if(!DegradationMap.Any(item => item.Health == 0))
        {
            DegradationMap.Add(new BreakableDegradation()
            {
                Health = 0,
                Id = -1,
                Sprite = null
            });
        }
        RefreshDegradationSprite();
    }

    private void RefreshDegradationSprite()
    {
        var degradation = DegradationMap.Where(item => item.Health > 0).Aggregate((x, y) => Math.Abs(x.Health - Health) < Math.Abs(y.Health - Health) ? x : y);

        if(_current != null && degradation.Id == _current.Id)
        {
            return;
        }

        foreach(var spriteComponent in _gfxList)
        {
            spriteComponent.sprite = degradation.Sprite;
        }

        _current = degradation;
    }

    private void RefreshWithDefaultSprite()
    {
        var degradation = DegradationMap.First(item => item.Health == 0);
        foreach (var spriteComponent in _gfxList)
        {
            spriteComponent.sprite = degradation.Sprite;
        }

        _current = degradation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage, HealthEffectorType type)
    {

        Health -= damage;

        if(Health <= 0)
        {
            Health = 0;
            if (_particuleSystem != null)
            {
                var breakPs = Instantiate(_particuleSystem, transform);
                Destroy(breakPs, 0.3f);
            }         
            _audioChannel.RaiseAudioRequest(new AudioEvent(MaterialType.GetBreakClipName(), ParentId));
            if(_geChannel != null)
            {
                _geChannel.RaiseEvent(new GameEvent()
                {
                    Origin = gameObject,
                    Type = GameEventType.Destroy
                });

            }
            RefreshWithDefaultSprite();
            Destroy(gameObject);
            return;
        }
        _audioChannel.RaiseAudioRequest(new AudioEvent(MaterialType.GetHitClipName(), ParentId));
        _animator.Play("Breakable_Hit");
        RefreshDegradationSprite();
    }
}
