using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Environment
{

        /// <summary>
        /// Makes a object move between two points.
        /// </summary>
        public class MovingPlatform : MonoBehaviour
        {
            [SerializeField]
            Transform begin = null;
            [SerializeField]
            Transform end = null;
            [SerializeField]
            float speed = 0.5f;
            [SerializeField]
            private Vector2 _spawnInterval; 
            [SerializeField]
            Transform _gfx;

            [SerializeField]
            AudioSource _audioSource;

            private float _randomTime;
            private float _currentCd = 0;

            public Vector2 Velocity { get; private set; }

            private bool _isGoingLeft;

        public bool IsGraphicsLookingLeft;


        private void Start()
        {
            ResetObject();
            _isGoingLeft = end.position.x < begin.position.x;
        }

        private void ResetObject()
        {
            _audioSource.Stop();
            transform.position = begin.position;
            ResetCoolDown();
        }
        private void ResetCoolDown()
        {      
            _randomTime = UnityEngine.Random.Range(_spawnInterval.x, _spawnInterval.y);
            _audioSource.PlayDelayed(_randomTime);
            _currentCd = 0;
        }

        private void Update()
            {
            if(_currentCd < _randomTime)
            {
                _currentCd += Time.deltaTime;
                return;
            }

            //if (!_audioClip.isPlaying)
            //{
            //    _audioClip.Play();
            //}

            if (IsGraphicsLookingLeft && !_isGoingLeft
                || !IsGraphicsLookingLeft && _isGoingLeft)
            {
                _gfx.Rotate(0, 180, 0);
                IsGraphicsLookingLeft = !IsGraphicsLookingLeft;
            }

            Velocity = (end.position - begin.position).normalized * speed;
            transform.Translate(Velocity * Time.deltaTime);

            if ((end.position - begin.position).sqrMagnitude < (transform.position - begin.position).sqrMagnitude)
            {
                ResetObject();
            }
        }

            private void OnDrawGizmos()
            {
                if (begin != null && end != null)
                {
                    Gizmos.color = Color.red;

                    DrawArrow(begin.position, end.position);
                    DrawArrow(end.position, begin.position);
                }
            }

            private void DrawArrow(Vector2 start, Vector2 end)
            {
                Vector2 dir = end - start;
                float length = dir.magnitude;
                dir /= length;
                Vector2 baseA = start + dir * (length - 0.1f);
                Gizmos.DrawLine(start, baseA);

                Vector2 normal = new Vector2(-dir.y, dir.x) * 0.1f;
                Gizmos.DrawLine(baseA - normal, baseA + normal);
                Gizmos.DrawLine(baseA - normal, end);
                Gizmos.DrawLine(baseA + normal, end);
            }
        }

}