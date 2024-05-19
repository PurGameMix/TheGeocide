using Assets.Scripts.Utils;
using System;
using UnityEngine;

namespace Assets.Scripts.Environment.Traps.Spikes
{
    public class PressureSpikeHitBox : MonoBehaviour
    {

        [SerializeField]
        private LayerMask _canTriggerLayer;

        public GameObject HitObject;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isInLayerMask(collision.gameObject.layer) && !collision.isTrigger)
            {
                HitObject = collision.gameObject;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (isInLayerMask(collision.gameObject.layer) && HitObject != null)
            {
                HitObject = null;
            }
        }

        private bool isInLayerMask(int layerValue)
        {
            return _canTriggerLayer.Contains(layerValue);
        }
    }
}