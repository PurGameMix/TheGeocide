using UnityEngine;
using System;
namespace Assets.Scripts.Environment.Breakables
{
    [Serializable]
    public class BreakableDegradation
    {
        public int Id;
        public int Health;
        public Sprite Sprite;
        public string AudioClip;
    }
}
