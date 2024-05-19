using Assets.Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts.Environment.Traps.Aoe
{
    public class AreaOfDamage : MonoBehaviour
    {

        [SerializeField]
        private Collider2D _collider;

        [SerializeField]
        private LayerMask _canBeDamageLayer;

        public int DamagePerTick;
        public float TickCooldown;

        private List<AoeEntity> _entityInArea = new List<AoeEntity>();

        // Update is called once per frame
        void FixedUpdate()
        {
            foreach(var item in _entityInArea)
            {
                item.CurrentTiming += Time.deltaTime; 
                if(item.CurrentTiming >= TickCooldown)
                {
                    item.CurrentTiming = 0;
                    item.Entity.TakeDamage(DamagePerTick, HealthEffectorType.trap);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isInLayerMask(collision.gameObject.layer))
            {
                var dmg = collision.GetComponent<ICanBeDamaged>();
                if (dmg != null)
                {
                    RegisterEntity(collision.gameObject.name, dmg);
                }
            }
        }

        private void RegisterEntity(string name, ICanBeDamaged entity)
        {

            var element = _entityInArea.FirstOrDefault(item => item.Name == name);
            if (element != null)
            {
                Debug.LogWarning("Element already present");
                return;
            }
            _entityInArea.Add(new AoeEntity(name, entity, TickCooldown));
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (isInLayerMask(collision.gameObject.layer))
            {
                var dmg = collision.GetComponent<ICanBeDamaged>();
                if (dmg != null)
                {
                    UnRegisterEntity(collision.gameObject.name, dmg);
                }
            }
        }

        private void UnRegisterEntity(string name, ICanBeDamaged dmg)
        {
            var item = _entityInArea.FirstOrDefault(item => item.Name == name);
            if(item != null)
            {
                _entityInArea.Remove(item);
            }        
        }

        private bool isInLayerMask(int layerValue)
        {
            return _canBeDamageLayer.Contains(layerValue);
        }
    }
}