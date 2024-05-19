using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts.Utils
{
    class LightGraduelEffect : MonoBehaviour
    {
        [Tooltip("External light to flicker; you can leave this null if you attach script to a light")]
        [SerializeField]
        private Light2D _light;

        [Tooltip("IntensityCurve")]
        public AnimationCurve LightItensityCurve;

        [Tooltip("How fast the graduale light effect will go")]
        [Range(1, 60)]
        public int speed = 5;

        private float _currentIntensityCursor = 0;
        private float _beginTime;
        private float _currentTime;
        private void Start()
        {
            _beginTime = Random.Range(0, 6);
        }
        void Update()
        {
            _currentTime += Time.deltaTime;
            if (_light == null || _currentTime < _beginTime)
                return;

            _currentIntensityCursor += Time.deltaTime * speed/60;
            if(_currentIntensityCursor > 1)
            {
                _currentIntensityCursor = 0;
            }


            // Calculate new smoothed average
            _light.intensity = LightItensityCurve.Evaluate(_currentIntensityCursor);
        }
    }
}
