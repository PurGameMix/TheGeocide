using Assets.Scripts.Utils;
using UnityEngine;

public class SwingingBladeEdge : MonoBehaviour
{

    public int Damage = 50;

    [SerializeField]
    private LayerMask _canTriggerLayer;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (isInLayerMask(collision.gameObject.layer))
		{
			var damagableObject = collision.gameObject.GetComponent<ICanBeDamaged>();
			if (damagableObject != null)
			{
				damagableObject.TakeDamage(Damage, HealthEffectorType.trap);
			}
		}
	}

	private bool isInLayerMask(int layerValue)
	{
		return _canTriggerLayer.Contains(layerValue);
	}
}
