using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utils
{
    public class UI_ContentSizeFitterRefresh : MonoBehaviour
    {

        [SerializeField]
        private HorizontalOrVerticalLayoutGroup layoutGroup;

        private float _maxRefreshTime = 1;
        private float _currentRefreshTime;
        private void FixedUpdate()
        {
            if (_currentRefreshTime >= _maxRefreshTime)
            {
                return;
            }
            _currentRefreshTime += Time.deltaTime;
            StartCoroutine(RefreshLayout());
        }

        IEnumerator RefreshLayout()
        {
            yield return new WaitForFixedUpdate();
            layoutGroup.enabled = false;
            layoutGroup.enabled = true;
        }
    }
}
