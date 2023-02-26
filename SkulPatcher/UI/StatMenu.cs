using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SkulPatcher.UI
{
    public static class StatMenu
    {
        // Init array of "fixed" size
        private static readonly (bool toApply, double statValue)[] statValues = Enumerable.Repeat((false, 0d), StatMenuFuncs.stats.Length).ToArray();
        private static readonly bool[] showStatInList = Enumerable.Repeat(true, StatMenuFuncs.stats.Length).ToArray();

        private static string searchText = string.Empty;
        private static string searchTextPreviousState = searchText;

        static StatMenu() => SetDefaults();

        private static void SetDefaults()
        {
            for (int i = 0; i < StatMenuFuncs.stats.Length; i++)
            {
                statValues[i].toApply = false;
                statValues[i].statValue = StatMenuFuncs.statLimitInfo[StatMenuFuncs.stats[i].category].defaultValue;
            }
        }

        public static void Fill(int _)
        {
            GUI.DragWindow(dragWindowRect);

            statScrollVec = GUI.BeginScrollView(statScrollPosRect, statScrollVec, statScrollViewRect, GUIStyle.none, GUIStyle.none);
            
            for (int i = 0; i < StatMenuFuncs.stats.Length; i++)
            {
                if (!showStatInList[i])
                    continue;

                var (category, _, name) = StatMenuFuncs.stats[i];
                var (minValue, maxValue, _, abbreviation) = StatMenuFuncs.statLimitInfo[category];

                statValues[i].toApply = GUI.Toggle(statScrollToggleRects[i],
                                                   statValues[i].toApply,
                                                   $"{name} ({statValues[i].statValue}{abbreviation})");

                statValues[i].statValue = Math.Round(GUI.HorizontalSlider(statScrollSliderRects[i],
                                                                          (float)statValues[i].statValue,
                                                                          (float)minValue,
                                                                          (float)maxValue));
            }

            GUI.EndScrollView();

            if (GUI.Button(applyChangesButtonRect, "Apply changes"))
                StatMenuFuncs.SetStats(statValues);

            if (GUI.Button(resetButtonRect, "Reset"))
            {
                SetDefaults();
                StatMenuFuncs.SetStats(statValues);
            }

            GUI.Label(searchLabelRect, "Search:");
            searchText = GUI.TextField(searchFieldRect, searchText);
            if (searchText != searchTextPreviousState)
            {
                searchTextPreviousState = searchText;
                statScrollViewRect = FilterStatsAndGetViewRect();
            }
        }

        public static Rect windowRect;

        private static int menuWidth;
        private static int menuHeight;
        private static float listWidth;

        private static Rect dragWindowRect;

        private static Vector2 statScrollVec = Vector2.zero;
        private static Rect statScrollPosRect;
        private static Rect statScrollViewRect;
        private static readonly Rect[] statScrollToggleRects = Enumerable.Repeat(new Rect(), StatMenuFuncs.stats.Length).ToArray();
        private static readonly Rect[] statScrollSliderRects = Enumerable.Repeat(new Rect(), StatMenuFuncs.stats.Length).ToArray();

        private static Rect applyChangesButtonRect;
        private static Rect resetButtonRect;

        private static Rect searchLabelRect;
        private static Rect searchFieldRect;

        public static void Resize()
        {
            menuWidth = (int)(Screen.width / 2.5);
            menuHeight = (int)(Menu.unit * 18.5f);

            windowRect = new Rect(Screen.width - menuWidth - Menu.unit, Menu.unit * 26.5f, menuWidth, menuHeight);
            int row = 0;

            dragWindowRect = new Rect(0, 0, menuWidth, Menu.unit);
            row++;

            listWidth = menuWidth - Menu.unit * 2;
            float listHeight = Menu.unit * 14;

            statScrollPosRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, listWidth, listHeight);
            statScrollViewRect = FilterStatsAndGetViewRect();

            row += 10;
            applyChangesButtonRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, Menu.unit * 8, Menu.unit);
            resetButtonRect = new Rect(Menu.unit * 10, Menu.unit * row * 1.5f, Menu.unit * 5, Menu.unit);

            searchLabelRect = new Rect(menuWidth - Menu.unit * 12, Menu.unit * row * 1.5f, Menu.unit * 3, Menu.unit);
            searchFieldRect = new Rect(menuWidth - Menu.unit * 9, Menu.unit * row * 1.5f, Menu.unit * 8, Menu.unit);
        }

        private static Rect FilterStatsAndGetViewRect()
        {
            string pattern = searchText;

            if (string.IsNullOrEmpty(pattern))
                pattern = ".+";

            Regex regex;
            try
            {
                regex = new(pattern, RegexOptions.IgnoreCase);
            }
            catch (ArgumentException)
            {
                regex = new(".+", RegexOptions.IgnoreCase);
            }

            int elementRow = 0;
            for (int i = 0; i < statValues.Length; i++)
            {
                if (!regex.Match(StatMenuFuncs.stats[i].name).Success)
                {
                    showStatInList[i] = false;
                    continue;
                }

                showStatInList[i] = true;

                statScrollToggleRects[i] = new Rect(0, Menu.unit * (elementRow * 2) * 1.5f, listWidth, Menu.unit);
                statScrollSliderRects[i] = new Rect(0, Menu.unit * (elementRow * 2 + 1) * 1.5f, listWidth, Menu.unit);
                elementRow++;
            }

            return new Rect(0, 0, listWidth, Menu.unit * 3 * elementRow);
        }
    }
}
