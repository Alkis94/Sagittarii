using UnityEngine;

namespace Factories
{
    public static class MenuFactory
    {
        private static GameObject PauseMenu;
        private static GameObject VictoryMenu;
        private static GameObject DefeatMenu;

        public static GameObject someMenu;



        static MenuFactory()
        {
            PauseMenu = Resources.Load("Menus/PauseMenu") as GameObject;
            VictoryMenu = Resources.Load("Menus/VictoryMenu") as GameObject;
            DefeatMenu = Resources.Load("Menus/DefeatMenu") as GameObject;
        }

        public static void CallPauseMenu()
        {
            CreateMenuAndPause(PauseMenu);
        }

        public static void CallVictoryMenu()
        {
            CreateMenuAndPause(VictoryMenu);
        }

        public static void CallDefeatMenu()
        {
            CreateMenuAndPause(DefeatMenu);
        }

        private static GameObject CreateMenuAndPause(GameObject Menu)
        {

            GameState.PauseGame();
            someMenu = GameObject.Instantiate(Menu);
            someMenu.transform.position = new Vector2(Camera.main.pixelHeight / 2, Camera.main.pixelWidth / 2);
            return someMenu;
        }

        public static void DestroyMenuAndUnpause()
        {
            GameObject.Destroy(someMenu);
            GameState.UnpauseGame();
        }
    }
}
