using Assets.Data.Items.Definition;
using System;
using UnityEngine.UI;

namespace Assets.Scripts.Ui.Item
{
    [Serializable]
    public class UI_ItemEntry
    {
        public ItemType ItemType;
        public UI_ToolTipTrigger ToolTipComponent;
        public Image ImageComponent;
    };
}
