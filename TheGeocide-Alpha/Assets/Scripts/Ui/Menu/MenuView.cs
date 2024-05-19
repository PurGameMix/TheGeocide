using System;
using UnityEngine;

namespace Assets.Scripts.Ui.Menu
{
    [Serializable]
    public class MenuView
    {
        public GameObject View;

        public GameObject Parent;

        internal string GetName()
        {
            return View.name;
        }

        internal void SetActiveView(bool isActive)
        {
            View.SetActive(isActive);
        }

        internal string GetParentName()
        {
            return Parent.name;
        }
    }
}
