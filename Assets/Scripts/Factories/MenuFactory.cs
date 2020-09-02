using UnityEngine;

namespace Factories
{
    public static class MenuFactory
    {
        public static GameObject DefeatMenu { get; private set; }
        public static GameObject QuickQuitMenu { get; private set; }
        private static GameObject someMenu;



        static MenuFactory()
        {
            DefeatMenu = Resources.Load("Menus/DefeatMenu") as GameObject;
            QuickQuitMenu = Resources.Load("Menus/QuickQuitMenu") as GameObject;
        }

        public static GameObject CreateMenuAndPause(GameObject Menu)
        {
            GameManager.GameState = GameStateEnum.paused;
            someMenu = GameObject.Instantiate(Menu);
            someMenu.transform.position = new Vector2(Camera.main.pixelHeight / 2, Camera.main.pixelWidth / 2);
            return someMenu;
        }

        public static void DestroyMenuAndUnpause()
        {
            GameObject.Destroy(someMenu);
            GameManager.GameState = GameStateEnum.unpaused;
        }
    }
}
