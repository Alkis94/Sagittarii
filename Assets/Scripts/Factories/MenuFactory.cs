using UnityEngine;

namespace Factories
{
    public static class MenuFactory
    {
        public static GameObject PauseMenu { get; private set; }
        public static GameObject DefeatMenu { get; private set; }
        private static GameObject someMenu;



        static MenuFactory()
        {
            PauseMenu = Resources.Load("Menus/PauseMenu") as GameObject;
            DefeatMenu = Resources.Load("Menus/DefeatMenu") as GameObject;
        }

        public static GameObject CreateMenuAndPause(GameObject Menu)
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
