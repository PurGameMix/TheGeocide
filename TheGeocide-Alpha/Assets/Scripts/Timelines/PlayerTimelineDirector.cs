using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Timelines
{
    public class PlayerTimelineDirector : MonoBehaviour
    {

        [SerializeField]
        private Rigidbody2D _rb;

        [SerializeField]
        private PlayerStateMachine _playerStateMachine;

        internal void StopMouvement()
        {
            _rb.velocity = Vector2.zero;
        }

        internal void TPPlayerToPosition(Vector2 teleportPosition)
        {
            _rb.gameObject.transform.position = teleportPosition;
        }

        internal void TurnPlayer(bool wantToLookRight)
        {
            _playerStateMachine.CheckDirectionToFace(wantToLookRight);
        }
    }
}
