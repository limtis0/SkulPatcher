using Data;
using System;
using UnityEngine;

namespace SkulPatcher.UI
{
    public class Menu : MonoBehaviour
    {
        public static int unit;
        public static int fontSize;

        private bool init = false;
        private bool showMenu = true;
        private bool showRouteMenu = false;
        private bool showGearMenu = false;

        private int screenWidthPrevious;
        private int screenHeightPrevious;

        public void Start()
        {
            StartCoroutine(Godmode.Coroutine());
            StartCoroutine(TurboActions.Coroutine());
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F7))
            {
                showMenu = !showMenu;
                Cursor.visible = showMenu;
            }
        }

        public void OnGUI()
        {
            if (!showMenu)
                return;

            if (!init || Screen.height != screenHeightPrevious || Screen.width != screenWidthPrevious)
            {
                CalculateSizing();

                Resize();
                RouteMenu.Resize();
                GearMenu.Resize();

                init = true;
                screenHeightPrevious = Screen.height;
                screenWidthPrevious = Screen.width;
            }

            windowRect = GUI.Window(0, windowRect, Fill, "SkulPatcher - F7 to toggle");

            if (showGearMenu)
                GearMenu.windowRect = GUI.Window(1, GearMenu.windowRect, GearMenu.Fill, "Gear menu");

            if (showRouteMenu)
                RouteMenu.windowRect = GUI.Window(2, RouteMenu.windowRect, RouteMenu.Fill, "Route menu");
        }

        private void Fill(int _)
        {
            GUI.DragWindow(dragWindowRect);

            // Luck boost
            Config.luckBoostOn = GUI.Toggle(luckBoostToggleRect,
                                            Config.luckBoostOn,
                                            $"Boost gear rarity with {Config.luckBoostPercent}% chance");

            Config.luckBoostPercent = (int)GUI.HorizontalSlider(luckBoostSliderRect, Config.luckBoostPercent, 0, 100);

            Config.luckBoostContinuous = GUI.Toggle(luckBoostContinuousToggleRect,
                                                    Config.luckBoostContinuous,
                                                    $"Continue boosting rarity if the roll was successful");

            // Duplicate gear
            Config.allowDuplicateItems = GUI.Toggle(duplicateItemsToggleRect, Config.allowDuplicateItems, "Allow duplicate items");
            Config.allowDuplicateSkulls = GUI.Toggle(duplicateSkullsToggleRect, Config.allowDuplicateSkulls, "Allow duplicate skulls");


            // Gold multiplier
            Config.goldMultOn = GUI.Toggle(goldMultToggleRect,
                                           Config.goldMultOn,
                                           $"Multiply incoming Gold by x{Config.goldMultValue}");

            Config.goldMultValue = (int)GUI.HorizontalSlider(goldMultSliderRect, Config.goldMultValue, 1, 50);


            // Dark quartz multiplier
            Config.dQuartzMultOn = GUI.Toggle(dQuartzMultToggleRect,
                                              Config.dQuartzMultOn,
                                              $"Multiply incoming Dark Quartz by x{Config.dQuartzMultValue}");

            Config.dQuartzMultValue = (int)GUI.HorizontalSlider(dQuartzMultSliderRect, Config.dQuartzMultValue, 1, 50);


            // Bones multiplier
            Config.bonesMultOn = GUI.Toggle(bonesMultToggleRect,
                                            Config.bonesMultOn,
                                            $"Multiply incoming Bones by x{Config.bonesMultValue}");

            Config.bonesMultValue = (int)GUI.HorizontalSlider(bonesMultSliderRect, Config.bonesMultValue, 1, 50);


            // Heart quartz multiplier
            Config.hQuartzMultOn = GUI.Toggle(hQuartzMultToggleRect,
                                              Config.hQuartzMultOn,
                                              $"Multiply incoming Heart Quartz by x{Config.hQuartzMultValue}");

            Config.hQuartzMultValue = (int)GUI.HorizontalSlider(hQuartzMultSliderRect, Config.hQuartzMultValue, 1, 50);


            // Gold value
            GUI.Label(goldLabelRect, "Gold");
            GameData.Currency.gold.balance = Convert.ToInt32(GUI.TextField(goldTextFieldRect, GameData.Currency.gold.balance.ToString()));

            // Dark Quartz value
            GUI.Label(dQuartzLabelRect, "Dark Quartz");
            GameData.Currency.darkQuartz.balance = Convert.ToInt32(GUI.TextField(dQuartzTextFieldRect, GameData.Currency.darkQuartz.balance.ToString()));

            // Bones value
            GUI.Label(bonesLabelRect, "Bones");
            GameData.Currency.bone.balance = Convert.ToInt32(GUI.TextField(bonesTextFieldRect, GameData.Currency.bone.balance.ToString()));

            // Heart Quartz value
            GUI.Label(hQuartzLabelRect, "Heart Quartz");
            GameData.Currency.heartQuartz.balance = Convert.ToInt32(GUI.TextField(hQuartzTextFieldRect, GameData.Currency.heartQuartz.balance.ToString()));

            // Easy mode
            Config.forceEasyModeOn = GUI.Toggle(easyModeToggleRect,
                                                Config.forceEasyModeOn,
                                                $"Force easy-mode");

            // God mode
            Config.godmodeOn = GUI.Toggle(godModeToggleRect,
                                          Config.godmodeOn,
                                          $"God mode");

            // Turbo-attack
            Config.turboAttackOn = GUI.Toggle(turboAttackToggleRect,
                                              Config.turboAttackOn,
                                              $"Turbo-attack");

            // Turbo-dash
            Config.turboDashOn = GUI.Toggle(turboDashToggleRect,
                                            Config.turboDashOn,
                                            $"Turbo-dash");

            if (GUI.Button(gearMenuButtonRect, "Gear menu"))
                showGearMenu = !showGearMenu;

            if (GUI.Button(routeMenuButtonRect, "Route menu"))
                showRouteMenu = !showRouteMenu;

            if (GUI.Button(saveConfigButtonRect, "Save config"))
                Config.Save();
        }


        // Menu elements
        private int menuWidth;
        private int menuHeight;

        private Rect dragWindowRect;
        private Rect windowRect;

        private Rect luckBoostToggleRect;
        private Rect luckBoostSliderRect;
        private Rect luckBoostContinuousToggleRect;

        private Rect duplicateItemsToggleRect;
        private Rect duplicateSkullsToggleRect;

        private Rect goldMultToggleRect;
        private Rect goldMultSliderRect;

        private Rect dQuartzMultToggleRect;
        private Rect dQuartzMultSliderRect;

        private Rect bonesMultToggleRect;
        private Rect bonesMultSliderRect;

        private Rect hQuartzMultToggleRect;
        private Rect hQuartzMultSliderRect;

        private Rect goldLabelRect;
        private Rect goldTextFieldRect;

        private Rect dQuartzLabelRect;
        private Rect dQuartzTextFieldRect;

        private Rect bonesLabelRect;
        private Rect bonesTextFieldRect;

        private Rect hQuartzLabelRect;
        private Rect hQuartzTextFieldRect;

        private Rect easyModeToggleRect;
        private Rect godModeToggleRect;

        private Rect turboAttackToggleRect;
        private Rect turboDashToggleRect;

        private Rect gearMenuButtonRect;
        private Rect routeMenuButtonRect;
        private Rect saveConfigButtonRect;

        private void CalculateSizing()
        {
            unit = Screen.height / 50;
            fontSize = (int)(unit * 0.75);

            GUI.skin.label.fontSize = fontSize;
            GUI.skin.toggle.fontSize = fontSize;
            GUI.skin.button.fontSize = fontSize;
            GUI.skin.textField.fontSize = fontSize;
        }

        private void Resize()
        {

            menuWidth = Screen.width / 4;
            menuHeight = unit * 32;

            windowRect = new Rect(Screen.width - menuWidth - 20, 20, menuWidth, menuHeight);
            int row = 0;

            dragWindowRect = new Rect(0, 0, menuWidth, unit);
            row++;


            // Luck boost
            luckBoostToggleRect = new Rect(unit, unit * row * 1.5f, menuWidth - unit, unit);
            row++;

            luckBoostSliderRect = new Rect(unit, unit * row * 1.5f, menuWidth - 2 * unit, unit);
            row++;

            luckBoostContinuousToggleRect = new Rect(unit, unit * row * 1.5f, menuWidth - unit, unit);
            row++;

            duplicateItemsToggleRect = new Rect(unit, unit * row * 1.5f, menuWidth / 2 - unit, unit);
            duplicateSkullsToggleRect = new Rect(menuWidth / 2, unit * row * 1.5f, menuWidth / 2 - unit, unit);
            row += 2;


            // Gold multiplier
            goldMultToggleRect = new Rect(unit, unit * row * 1.5f, menuWidth - unit, unit);
            row++;

            goldMultSliderRect = new Rect(unit, unit * row * 1.5f, menuWidth - 2 * unit, unit);
            row++;

            // Dark quartz multiplier
            dQuartzMultToggleRect = new Rect(unit, unit * row * 1.5f, menuWidth - unit, unit);
            row++;

            dQuartzMultSliderRect = new Rect(unit, unit * row * 1.5f, menuWidth - 2 * unit, unit);
            row++;

            // Bones multiplier
            bonesMultToggleRect = new Rect(unit, unit * row * 1.5f, menuWidth - unit, unit);
            row++;

            bonesMultSliderRect = new Rect(unit, unit * row * 1.5f, menuWidth - 2 * unit, unit);
            row++;

            // Heart quartz multiplier
            hQuartzMultToggleRect = new Rect(unit, unit * row * 1.5f, menuWidth - unit, unit);
            row++;

            hQuartzMultSliderRect = new Rect(unit, unit * row * 1.5f, menuWidth - 2 * unit, unit);
            row++;


            // Gold value
            goldLabelRect = new Rect(unit, unit * row * 1.5f, unit * 5, unit * 1.2f);
            goldTextFieldRect = new Rect(unit, unit * (row + 1) * 1.5f, unit * 4, unit);

            // Dark quartz value
            dQuartzLabelRect = new Rect(unit * 6.5f, unit * row * 1.5f, unit * 5, unit * 1.2f);
            dQuartzTextFieldRect = new Rect(unit * 6.5f, unit * (row + 1) * 1.5f, unit * 4, unit);

            // Bones value
            bonesLabelRect = new Rect(unit * 12f, unit * row * 1.5f, unit * 5, unit * 1.2f);
            bonesTextFieldRect = new Rect(unit * 12f, unit * (row + 1) * 1.5f, unit * 4, unit);

            // Heart quartz value
            hQuartzLabelRect = new Rect(unit * 17.5f, unit * row * 1.5f, unit * 5, unit * 1.2f);
            hQuartzTextFieldRect = new Rect(unit * 17.5f, unit * (row + 1) * 1.5f, unit * 4, unit);
            row += 3;

            float halfWidth = menuWidth / 2 - Menu.unit;

            // Easy mode
            easyModeToggleRect = new Rect(unit, unit * row * 1.5f, halfWidth, unit);
            row++;

            // God mode
            godModeToggleRect = new Rect(unit, unit * row * 1.5f, halfWidth, unit);
            row++;

            // Turbo-attack
            turboAttackToggleRect = new Rect(unit, unit * row * 1.5f, halfWidth, unit);
            row++;

            // Turbo-attack
            turboDashToggleRect = new Rect(unit, unit * row * 1.5f, halfWidth, unit);
            row -= 4;

            // Route menu
            routeMenuButtonRect = new Rect(halfWidth + unit, (unit + 0.5f) * row * 1.5f + unit, halfWidth, unit);
            row++;

            // Gear menu
            gearMenuButtonRect = new Rect(halfWidth + unit, (unit + 0.5f) * row * 1.5f + unit, halfWidth, unit);
            row += 2;

            // Save config
            saveConfigButtonRect = new Rect(halfWidth + unit, (unit + 0.5f) * row * 1.5f + unit, halfWidth, unit);
        }
    }
}
