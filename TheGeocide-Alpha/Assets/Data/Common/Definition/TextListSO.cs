using System.Collections.Generic;
using UnityEngine;

namespace Assets.Data.Common
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Common/TextList")]
    public class TextListSO : ScriptableObject
    {
        public List<string> TradKeyList;
    }
}