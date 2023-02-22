using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SkulPatcher.UI
{
    public static class StatMenu
    {
        private static (bool toApply, int statValue)[] statValues = Enumerable.Repeat((false, 0), StatFuncs.stats.Length).ToArray();  // Init array of "fixed" size

        static StatMenu() => SetDefaults();

        private static void SetDefaults()
        {
            for (int i = 0; i < StatFuncs.stats.Length; i++)
            {
                statValues[i].toApply = false;
                statValues[i].statValue = StatFuncs.statConsts[StatFuncs.stats[i].category].defaultValue;
            }
        }

        public static void Fill(int _)
        {
            GUI.DragWindow(dragWindowRect);

            statScrollVec = GUI.BeginScrollView(statScrollPosRect, statScrollVec, statScrollViewRect, GUIStyle.none, GUIStyle.none);
            
            for (int i = 0; i < StatFuncs.stats.Length; i++)
            {
                var (category, _, name) = StatFuncs.stats[i];
                var (minValue, maxValue, _, abbreviation) = StatFuncs.statConsts[category];

                statValues[i].toApply = GUI.Toggle(statScrollToggleRects[i],
                                                   statValues[i].toApply,
                                                   $"{name} ({statValues[i].statValue:+0;-#}{abbreviation})");

                statValues[i].statValue = (int)GUI.HorizontalSlider(statScrollSliderRects[i],
                                                                    statValues[i].statValue,
                                                                    minValue,
                                                                    maxValue);
            }

            GUI.EndScrollView();

            if (GUI.Button(applyChangesButtonRect, "Apply changes"))
                StatFuncs.SetBuff(statValues);

            if (GUI.Button(resetButtonRect, "Reset"))
            {
                SetDefaults();
                StatFuncs.SetBuff(statValues);
            }
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
        private static Rect resetButtonRect;

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
            applyChangesButtonRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, Menu.unit * 8, Menu.unit);
            resetButtonRect = new Rect(Menu.unit * 10, Menu.unit * row * 1.5f, Menu.unit * 5, Menu.unit);
        }
    }
}
