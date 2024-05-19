using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class VectorUtils
    {
       public static float GetAngle(Vector2 origin, Vector2 target)
        {
            return -Mathf.Atan2(target.y - origin.y, target.x - origin.x) * 180 / Mathf.PI;
        }
    }
}
