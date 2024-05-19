using UnityEngine;

public class IceExplosion : MonoBehaviour
{

    [SerializeField]
    private Collider2D _collider2D;
    [SerializeField]
    private LayerMask _hitLayers;

    private int _damage = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleExplosion()
    {
        var hitEnemies = Physics2D.OverlapBoxAll(_collider2D.bounds.center, _collider2D.bounds.size, 0, _hitLayers);

        foreach (Collider2D collider in hitEnemies)
        {
            HandleSpellDamage(collider);
        }
    }

    public void AnimationCompleted()
    {
        Destroy(gameObject);
    }


    private void HandleSpellDamage(Collider2D hitInfo)
    {

        if (hitInfo.isTrigger)
        {
            return;
        }

        if (hitInfo.tag == "Player")
        {
            return;
        }

        var canBeDamageObj = hitInfo.GetComponent<ICanBeDamaged>();
        if (canBeDamageObj != null)
        {

            canBeDamageObj.TakeDamage(_damage, GetEffector());
        }
    }

    private HealthEffectorType GetEffector()
    {
        return HealthEffectorType.playerFroze;
    }
}
