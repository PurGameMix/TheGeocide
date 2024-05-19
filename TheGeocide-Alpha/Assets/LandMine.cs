using Assets.Scripts.Effects.Generic;
using Assets.Scripts.Utils;
using UnityEngine;

public class LandMine : AnimationCaller, ICanBeDamaged
{
    [SerializeField]
    private Collider2D _col;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private AudioController _audioController;


    [SerializeField]
    private LayerMask _hitLayers;

    [SerializeField]
    private FireExplosion _explosionPrefab;

    void Start()
    {
        _animator.Play("Idle");
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (!_hitLayers.Contains(hitInfo.gameObject.layer))
        {
            return;
        }

        Explode();
    }

    private void Explode()
    {
        _animator.Play("Trigger");
        _audioController.Play("Trigger", onAudioCompleted);
    }

    private void onAudioCompleted()
    {
        Instantiate(_explosionPrefab,transform.position,transform.rotation);
        Destroy(gameObject, 0.1f);
    }

    public override void AnimationBegin()
    {
    }

    public override void AnimationCompleted()
    {
        _audioController.Play("Bip");
    }

    public void TakeDamage(int damage, HealthEffectorType type)
    {
        Explode();
    }
}
