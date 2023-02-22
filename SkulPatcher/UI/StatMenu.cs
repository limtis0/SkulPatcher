using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

namespace SkulPatcher.UI
{
    public static class StatMenu
    {
        private static bool[] statToggles = Enumerable.Repeat(true, StatFuncs.stats.Length).ToArray();
        private static int[] statValues = Enumerable.Repeat(0, StatFuncs.stats.Length).ToArray();

        public static void Fill(int _)
        {
            GUI.DragWindow(dragWindowRect);

            statScrollVec = GUI.BeginScrollView(statScrollPosRect, statScrollVec, statScrollViewRect, GUIStyle.none, GUIStyle.none);
            
            for (int i = 0; i < StatFuncs.stats.Length; i++)
            {
                statToggles[i] = GUI.Toggle(statScrollToggleRects[i], statToggles[i], $"{StatFuncs.stats[i].name} ({statValues[i]}pp)");
                statValues[i] = (int) GUI.HorizontalSlider(statScrollSliderRects[i], statValues[i], -1000, 1000);
            }

            GUI.EndScrollView();

            if (GUI.Button(applyChangesButtonRect, "Apply changes"))
                StatFuncs.SetBuff(statToggles, statValues);
        }

        public static Rect windowRect;

        private static int menuWidth;
        private static int menuHeight;

        private static Rect dragWindowRect;

        private static Vector2 statScrollVec = Vector2.zero;
        private static Rect statScrollPosRect;
        private static Rect statScrollViewRect;
        private static List<Rect> statScrollToggleRects;
        private static List<Rect> statScrollSliderRects;

        private static Rect applyChangesButtonRect;

        public static void Resize()
        {
            menuWidth = (int)(Screen.width / 2.5);
            menuHeight = Menu.unit * 14;

            windowRect = new Rect(20, Screen.height - menuHeight - 20, menuWidth, menuHeight);
            int row = 0;

            dragWindowRect = new Rect(0, 0, menuWidth, Menu.unit);
            row++;

            float listWidth = menuWidth - Menu.unit * 2;
            float listHeight = Menu.unit * 10f;

            statScrollPosRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, listWidth, listHeight);
            statScrollViewRect = new Rect(0, 0, listWidth, Menu.unit * 3 * StatFuncs.stats.Length);

            statScrollToggleRects = new();
            statScrollSliderRects = new();
            for (int statRow = 0; statRow < StatFuncs.stats.Length; statRow++)
            {
                statScrollToggleRects.Add(new Rect(0, Menu.unit * (statRow * 2) * 1.5f, listWidth, Menu.unit));
                statScrollSliderRects.Add(new Rect(0, Menu.unit * (statRow * 2 + 1) * 1.5f, listWidth, Menu.unit));
            }

            row += 7;
            applyChangesButtonRect = new Rect(Menu.unit * 1.5f, Menu.unit * row * 1.5f, Menu.unit * 8, Menu.unit);
        }
    }
}
