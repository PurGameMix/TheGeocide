using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableOS : CanBeDetect, ICanBeDamaged
{
    [SerializeField]
    private Collider2D _colliderComponent;
    [SerializeField]
    private SpriteRenderer _spriteComponent;
    [SerializeField]
    private AudioSource _audioComponent;
    // Start is called before the first frame update

    public void TakeDamage(int damage, HealthEffectorType type)
    {
        _audioComponent.Play();

        if(_spriteComponent != null)
        {
            _spriteComponent.sprite = null;
        }

        _colliderComponent.enabled = false;
        Destroy(gameObject, 1f);
    }
}
