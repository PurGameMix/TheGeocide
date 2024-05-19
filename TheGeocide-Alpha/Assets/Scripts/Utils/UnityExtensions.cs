using UnityEngine;


namespace Assets.Scripts.Utils
{
    public static class UnityExtensions
    {

        /// <summary>
        /// Extension method to check if a layer is in a layermask
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static bool Contains(this LayerMask mask, int layer)
        {
            return ((mask & (1 << layer)) != 0);
        }
    }
}