using UnityEngine;

public class FireExplosion : InOutFX
{
    [SerializeField]
    private AudioSource _audio;
    [SerializeField]
    private Collider2D _collider2D;
    [SerializeField]
    private LayerMask _hitLayers;

    public int Damage = 50;
    public void HandleExplosion()
    {
        var hitEnemies = Physics2D.OverlapBoxAll(_collider2D.bounds.center, _collider2D.bounds.size, 0, _hitLayers);

        foreach (Collider2D collider in hitEnemies)
        {
            HandleDamage(collider);
        }
    }

    private void HandleDamage(Collider2D hitInfo)
    {

        var canBeDamageObj = hitInfo.GetComponent<ICanBeDamaged>();
        if (canBeDamageObj != null)
        {

            canBeDamageObj.TakeDamage(Damage, GetEffector());
        }
    }

    private HealthEffectorType GetEffector()
    {
        return HealthEffectorType.enemy;
    }

    internal override void EffectBegin(string effectName)
    {
        HandleExplosion();
    }

    internal override void EffectCompleted(string effectName)
    {
        Destroy(gameObject, 1f);
    }
}
