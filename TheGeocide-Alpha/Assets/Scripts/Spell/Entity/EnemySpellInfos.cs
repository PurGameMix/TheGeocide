using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Spell.Entity
{
    public class EnemySpellInfos
    {
        public int Damage;
        public float RepulseForce;
        public Transform RepulseOrigin;

        public HealthEffectorType Effector;
        public float EffecProbability = 100;
    }
}
