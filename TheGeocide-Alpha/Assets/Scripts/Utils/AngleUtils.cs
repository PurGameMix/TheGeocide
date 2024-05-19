namespace Assets.Scripts.Utils
{
    using UnityEngine;

    public static class AngleUtils
    {

        /// <summary>
        /// Get 360 Angle where 0 can be define by a vector3
        /// </summary>
        /// <param name="origin">Center of the circle</param>
        /// <param name="target">Target on the circle of the angle calculation</param>
        /// <param name="lowerAnglePosition">Origin of 0° </param>
        /// <returns></returns>
        /// 
        public static float GetDegreeAngle(Vector3 origin, Vector3 target, Vector3 lowerAnglePosition)
        {
            float angle = Vector2.SignedAngle(origin - target, lowerAnglePosition);

            if (angle >= 0)
            {
                return 180 - angle;
            }

            return 180 + Mathf.Abs(angle);
        }

        /// <summary>
        /// Get 360 Angle where 0 is Horizontal right
        /// </summary>
        /// <param name="origin">Center of the circle</param>
        /// <param name="target"></param>
        /// <returns></returns>
        /// 
        public static float GetDegreeAngle(Vector3 origin, Vector3 target)
        {
            float angle = Vector2.SignedAngle(origin - target, Vector3.right);

            if (angle >= 0)
            {
                return 180 - angle;
            }

            return 180 + Mathf.Abs(angle);
        }

        /// <summary>
        /// Get rad Angle from 360 angle where 0 is Horizontal right
        /// </summary>
        /// <param name="playerPoint"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static float GetRadAngle(Vector3 centerPoint, Vector3 position)
        {
            var degAngle = GetDegreeAngle(centerPoint, position);
            return degAngle * Mathf.PI / 180;
        }

    }

}
