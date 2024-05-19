using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Data.Common.Definition
{
    using Assets.Scripts.Utils;
    using System.Collections.Generic;
    using UnityEngine;

    public class BoxTriggerSide : MonoBehaviour
    {
        public CollisionSide2D WantedEnterSide;
        public bool IsOneShot;
        [SerializeField]
        private BoxCollider2D _collider;

        private float TopLeftAngle;
        private float TopRightAngle;
        private float BottomLeftAngle;
        private float BottomRightAngle;

        private bool _enterGoodSide;
        internal bool IsTriggered;
        private bool _hasTriggered;
        private static Dictionary<CollisionSide2D, CollisionSide2D> _oppositeMap = new Dictionary<CollisionSide2D, CollisionSide2D>() {
        {CollisionSide2D.Top, CollisionSide2D.Bottom},
        {CollisionSide2D.Bottom, CollisionSide2D.Top},
        {CollisionSide2D.Right, CollisionSide2D.Left},
        {CollisionSide2D.Left, CollisionSide2D.Right},
        };

        private void Start()
        {

            var topLeftPoint = new Vector2(_collider.bounds.center.x - (_collider.bounds.size.x / 2), _collider.bounds.center.y + (_collider.bounds.size.y / 2));
            TopLeftAngle = VectorUtils.GetAngle(_collider.bounds.center, topLeftPoint);
            var topRightPoint = new Vector2(_collider.bounds.center.x + (_collider.bounds.size.x / 2), _collider.bounds.center.y + (_collider.bounds.size.y / 2));
            TopRightAngle = VectorUtils.GetAngle(_collider.bounds.center, topRightPoint);
            var bottomLeftPoint = new Vector2(_collider.bounds.center.x - (_collider.bounds.size.x / 2), _collider.bounds.center.y - (_collider.bounds.size.y / 2));
            BottomLeftAngle = VectorUtils.GetAngle(_collider.bounds.center, bottomLeftPoint);
            var bottomRightPoint = new Vector2(_collider.bounds.center.x + (_collider.bounds.size.x / 2), _collider.bounds.center.y - (_collider.bounds.size.y / 2));
            BottomRightAngle = VectorUtils.GetAngle(_collider.bounds.center, bottomRightPoint);

            //Debug.Log($"TopLeftAngle : {TopLeftAngle}");
            //Debug.Log($"TopRightAngle : {TopRightAngle}");
            //Debug.Log($"BottomLeftAngle : {BottomLeftAngle}");
            //Debug.Log($"BottomRightAngle : {BottomRightAngle}");
        }

        void OnTriggerEnter2D(Collider2D collision)
        {

            if (IsOneShot && _hasTriggered)
            {
                return;
            }

            if (collision.tag == "Player")
            {
                IsTriggered = false;
                var enterSide = GetHitSide(collision);

                //Debug.Log("Enter " + enterSide);
                _enterGoodSide = enterSide == WantedEnterSide;
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if(IsOneShot && _hasTriggered)
            {
                return;
            }

            if (collision.tag == "Player")
            {
                var exitSide = GetHitSide(collision);

                //Debug.Log("Exit " + exitSide);
                if (_enterGoodSide && exitSide == GetOppositeSide(WantedEnterSide))
                {
                    IsTriggered = true;
                    _hasTriggered = true;
                }
            }
        }

        private CollisionSide2D GetOppositeSide(CollisionSide2D enterSide)
        {
            return _oppositeMap[enterSide];
        }

        private CollisionSide2D GetHitSide(Collider2D collision)
        {
            var closestPoint = collision.ClosestPoint(_collider.bounds.center);
            var angle = VectorUtils.GetAngle(_collider.bounds.center, closestPoint);
            //Debug.Log("angle:" + angle);

            if (TopLeftAngle <= angle && angle < TopRightAngle)
            {
                return CollisionSide2D.Top;
            }
            if (TopRightAngle <= angle && angle < BottomRightAngle)
            {
                return CollisionSide2D.Right;
            }
            if (BottomRightAngle <= angle && angle < BottomLeftAngle)
            {
                return CollisionSide2D.Bottom;
            }


            return CollisionSide2D.Left;
        }

        private Vector2 GetDrawVector(CollisionSide2D wantedEnterSide)
        {

            var vector = _collider.bounds.center;
            var vectorSizeH = _collider.bounds.size.y /2;
            var vectorSizeW = _collider.bounds.size.x / 2;
            switch (wantedEnterSide)
            {
                case CollisionSide2D.Left: vector.x = vector.x - vectorSizeW;  break;
                case CollisionSide2D.Right: vector.x = vector.x + vectorSizeW; break;
                case CollisionSide2D.Top: vector.y = vector.y + vectorSizeH; break;
                case CollisionSide2D.Bottom: vector.y = vector.y - vectorSizeH; break;
            }

            return vector;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            //var topLeftPoint = new Vector2(_collider.bounds.center.x - (_collider.bounds.size.x / 2), _collider.bounds.center.y + (_collider.bounds.size.y / 2));
            //var topRightPoint = new Vector2(_collider.bounds.center.x + (_collider.bounds.size.x / 2), _collider.bounds.center.y + (_collider.bounds.size.y / 2));
            //var bottomLeftPoint = new Vector2(_collider.bounds.center.x - (_collider.bounds.size.x / 2), _collider.bounds.center.y - (_collider.bounds.size.y / 2));
            //var bottomRightPoint = new Vector2(_collider.bounds.center.x + (_collider.bounds.size.x / 2), _collider.bounds.center.y - (_collider.bounds.size.y / 2));

            Gizmos.DrawWireCube(_collider.bounds.center, _collider.bounds.size);
            Gizmos.DrawLine(_collider.bounds.center, GetDrawVector(WantedEnterSide));
        }
    }

}
