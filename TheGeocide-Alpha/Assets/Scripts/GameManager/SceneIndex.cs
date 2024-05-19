namespace Assets.Scripts.GameManager
{
    public enum SceneIndex
    {
        Root = 0,
        TitleScreen = 1,
        Hub = 2,
        ActI_Birth_lvl1 = 3,
        ActI_Temple_lvl2 = 4,
        ActI_Contact_lvl3 = 5,
        ActI_GoldMiner_lvl4 = 6,
        ActI_DrugDealer_lvl5 = 7,
        ActI_BossCamp_lvl6 = 8,




        Final_lvl =28 //!!!! Keep level in order of apparition,increment last level each time !!!!
                    //!!!! Add Traduction un localization table !!!!
    }

    public static class SceneIndexExtensions
    {
        public static bool IsRoot(this SceneIndex si)
        {
            return si == SceneIndex.Root;
        }

        public static string GetTradKey(this SceneIndex si)
        {
            return $"GUI_MENU_SAVE_LEVEL_{(int)si}";
        }
    }
}