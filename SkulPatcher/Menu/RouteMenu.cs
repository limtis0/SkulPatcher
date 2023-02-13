using UnityEngine;

namespace SkulPatcher
{
    public static class RouteMenu
    {
        public static void FillMenu(int windowID)
        {
            GUI.DragWindow(dragWindowRect);

            Config.bossRushOn = GUI.Toggle(bossRushToggleRect, Config.bossRushOn, "Boss rush");
            Config.bossRushAllowNPC = GUI.Toggle(bossRushNPCToggleRect, Config.bossRushAllowNPC, "Don't skip NPC & Shops");
        }

        
        public static Rect windowRect;

        private static int menuWidth;
        private static int menuHeight;

        // Menu elements
        private static Rect dragWindowRect;

        private static Rect bossRushToggleRect;
        private static Rect bossRushNPCToggleRect;

        public static void Resize()
        {
            menuWidth = Screen.width / 6;
            menuHeight = Menu.unit * 14;

            windowRect = new Rect(20, 20, menuWidth, menuHeight);
            int row = 0;

            dragWindowRect = new Rect(0, 0, menuWidth, Menu.unit);
            row++;

            bossRushToggleRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, menuWidth / 2 - Menu.unit, Menu.unit);
            bossRushNPCToggleRect = new Rect(menuWidth / 2, Menu.unit * row * 1.5f, menuWidth / 2 - Menu.unit, Menu.unit);
        }
    }
}
