namespace Assets.Scripts.Utils
{
    using Assets.Data.Common.Definition;
    using UnityEngine;

    public static class BoxCollideSideUtils
    {
        public static CollisionSide2D GetHitSide(BoxCollider2D origin, Collider2D collision)
        {

            var topLeftPoint = new Vector2(origin.bounds.center.x - (origin.bounds.size.x / 2), origin.bounds.center.y + (origin.bounds.size.y / 2));
            var topLeftAngle = VectorUtils.GetAngle(origin.bounds.center, topLeftPoint);
            var topRightPoint = new Vector2(origin.bounds.center.x + (origin.bounds.size.x / 2), origin.bounds.center.y + (origin.bounds.size.y / 2));
            var topRightAngle = VectorUtils.GetAngle(origin.bounds.center, topRightPoint);
            var bottomLeftPoint = new Vector2(origin.bounds.center.x - (origin.bounds.size.x / 2), origin.bounds.center.y - (origin.bounds.size.y / 2));
            var bottomLeftAngle = VectorUtils.GetAngle(origin.bounds.center, bottomLeftPoint);
            var bottomRightPoint = new Vector2(origin.bounds.center.x + (origin.bounds.size.x / 2), origin.bounds.center.y - (origin.bounds.size.y / 2));
            var bottomRightAngle = VectorUtils.GetAngle(origin.bounds.center, bottomRightPoint);

            var closestPoint = collision.ClosestPoint(origin.bounds.center);
            var angle = VectorUtils.GetAngle(origin.bounds.center, closestPoint);
            //Debug.Log("angle:" + angle);

            if (topLeftAngle <= angle && angle < topRightAngle)
            {
                return CollisionSide2D.Top;
            }
            if (topRightAngle <= angle && angle < bottomRightAngle)
            {
                return CollisionSide2D.Right;
            }
            if (bottomRightAngle <= angle && angle < bottomLeftAngle)
            {
                return CollisionSide2D.Bottom;
            }

            return CollisionSide2D.Left;
        }
    }

}
