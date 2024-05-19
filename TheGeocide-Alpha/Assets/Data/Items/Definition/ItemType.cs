using System.Collections.Generic;

namespace Assets.Data.Items.Definition
{

    public enum ItemType
    {
        Melee,
        Ranged,
        Landing,
        Uppercut,
        Ultimate
    }

    public static class ItemTypeExtensions
    {
        private static Dictionary<ItemType, string> _colorMap = new Dictionary<ItemType, string>()
        {
            {ItemType.Melee , "#980002" }, //Blood Red
            {ItemType.Ranged , "#EEE600" }, //Titanium Yellow
            {ItemType.Landing , "#318CE7" }, //Bleu de France
            {ItemType.Uppercut , "#76797D" }, //Casper
            {ItemType.Ultimate , "#4F2398" },//Daisy Bush
        };

        public static string GetColor(this ItemType it)
        {
            return _colorMap[it];
        }


        private static List<ItemType> _handleByStateMachineList = new List<ItemType>() {
            ItemType.Landing,
            ItemType.Uppercut,
            ItemType.Ultimate
        };

        public static bool IsHandleByStateMachine(this ItemType st)
        {
            return _handleByStateMachineList.Contains(st);
        }

        private static List<ItemType> _castAndReleaseItemList = new List<ItemType>() {
            ItemType.Landing,
            ItemType.Uppercut,
            ItemType.Ultimate
        };
        public static bool IsCastAndReleaseItem(this ItemType st)
        {
            return _castAndReleaseItemList.Contains(st);
        }

        private static Dictionary<ItemType,string> _inputNameDico = new Dictionary<ItemType, string>() {
        {ItemType.Melee,"MeleeAttack"},
        {ItemType.Ranged,"RangedAttack"},
        {ItemType.Landing,"LandingAttack"},
        {ItemType.Uppercut,"UppercutAttack"},
        {ItemType.Ultimate, "UltimateAttack"}
        };
        public static string GetInputActionName(this ItemType st)
        {
            return _inputNameDico[st];
        }
    }
}