using Level;
using SkulPatcher.Patches;
using UnityEngine;

namespace SkulPatcher.UI
{
    public static class RouteMenu
    {
        public static void Fill(int _)
        {
            GUI.DragWindow(dragWindowRect);

            ModConfig.bossRushOn = GUI.Toggle(bossRushToggleRect, ModConfig.bossRushOn, "Boss rush");
            if (ModConfig.bossRushOn != ModConfig.bossRushOnPreviousState)
            {
                ModConfig.bossRushOnPreviousState = ModConfig.bossRushOn;
                BossRushPatch.Toggle();
            }

            ModConfig.bossRushSkipRewards = GUI.Toggle(bossRushSkipRewardsRect, ModConfig.bossRushSkipRewards, "Skip rewards");

            ModConfig.bossRushIncludeShops = GUI.Toggle(bossRushIncludeShopsRect, ModConfig.bossRushIncludeShops, "Include shops");
            if (ModConfig.bossRushIncludeShops != ModConfig.bossRushShopsPreviousState)
            {
                ModConfig.bossRushShopsPreviousState = ModConfig.bossRushIncludeShops;
                BossRushPatch.Toggle();
            }

            ModConfig.bossRushIncludeArachne = GUI.Toggle(bossRushIncludeArachneRect, ModConfig.bossRushIncludeArachne, "Include Arachne");
            if (ModConfig.bossRushIncludeArachne != ModConfig.bossRushArachnePreviousState)
            {
                ModConfig.bossRushArachnePreviousState = ModConfig.bossRushIncludeArachne;
                BossRushPatch.Toggle();
            }

            if (GUI.Button(chapter1ButtonRect, "Chapter 1"))
                BossRushPatch.LoadChapter(Chapter.Type.Chapter1);

            if (GUI.Button(chapter2ButtonRect, "Chapter 2"))
                BossRushPatch.LoadChapter(Chapter.Type.Chapter2);

            if (GUI.Button(chapter3ButtonRect, "Chapter 3"))
                BossRushPatch.LoadChapter(Chapter.Type.Chapter3);

            if (GUI.Button(chapter4ButtonRect, "Chapter 4"))
                BossRushPatch.LoadChapter(Chapter.Type.Chapter4);

            if (GUI.Button(chapter5ButtonRect, "Chapter 5"))
                BossRushPatch.LoadChapter(Chapter.Type.Chapter5);

            if (GUI.Button(nextMapButtonRect, "Next map ▶"))
                ModConfig.Level.LoadNextMap();

            if (GUI.Button(nextStageButtonRect, "Next stage ▶▶"))
                ModConfig.Level.LoadNextStage();
        }

        public static Rect windowRect;

        private static int menuWidth;
        private static int menuHeight;

        private static Rect dragWindowRect;

        private static Rect bossRushToggleRect;
        private static Rect bossRushSkipRewardsRect;
        private static Rect bossRushIncludeShopsRect;
        private static Rect bossRushIncludeArachneRect;

        private static Rect chapter1ButtonRect;
        private static Rect chapter2ButtonRect;
        private static Rect chapter3ButtonRect;
        private static Rect chapter4ButtonRect;
        private static Rect chapter5ButtonRect;

        private static Rect nextMapButtonRect;
        private static Rect nextStageButtonRect;

        public static void Resize()
        {
            menuWidth = Screen.width / 6;
            menuHeight = Menu.unit * 11;

            windowRect = new Rect(20, 20, menuWidth, menuHeight);
            int row = 0;

            dragWindowRect = new Rect(0, 0, menuWidth, Menu.unit);
            row++;

            bossRushToggleRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, menuWidth / 2 - Menu.unit, Menu.unit);
            bossRushSkipRewardsRect = new Rect(menuWidth / 2, Menu.unit * row * 1.5f, menuWidth / 2 - Menu.unit, Menu.unit);
            row++;

            bossRushIncludeShopsRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, menuWidth / 2 - Menu.unit, Menu.unit);
            bossRushIncludeArachneRect = new Rect(menuWidth / 2, Menu.unit * row * 1.5f, menuWidth / 2 - Menu.unit, Menu.unit);
            row += 2;

            chapter1ButtonRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, 3.91f * Menu.unit, Menu.unit);
            chapter2ButtonRect = new Rect(5.41f * Menu.unit, Menu.unit * row * 1.5f, 3.91f * Menu.unit, Menu.unit);
            chapter3ButtonRect = new Rect(9.82f * Menu.unit, Menu.unit * row * 1.5f, 3.91f * Menu.unit, Menu.unit);
            row++;

            chapter4ButtonRect = new Rect(3.20f * Menu.unit, Menu.unit * row * 1.5f, 3.91f * Menu.unit, Menu.unit);
            chapter5ButtonRect = new Rect(7.63f * Menu.unit, Menu.unit * row * 1.5f, 3.91f * Menu.unit, Menu.unit);
            row++;

            nextMapButtonRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, 6.36f * Menu.unit, Menu.unit);
            nextStageButtonRect = new Rect(7.86f * Menu.unit, Menu.unit * row * 1.5f, 6.36f * Menu.unit, Menu.unit);
        }
    }
}
