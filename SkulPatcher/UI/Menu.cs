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
        private bool showStatMenu = false;

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
            if (!showMenu || ModConfig.Gear is null)
                return;

            if (!init || Screen.height != screenHeightPrevious || Screen.width != screenWidthPrevious)
            {
                CalculateSizing();

                Resize();
                MapMenu.Resize();
                GearMenu.Resize();
                StatMenu.Resize();

                init = true;
                screenHeightPrevious = Screen.height;
                screenWidthPrevious = Screen.width;
            }

            windowRect = GUI.Window(0, windowRect, Fill, "SkulPatcher - F7 to toggle");

            if (showGearMenu)
                GearMenu.windowRect = GUI.Window(1, GearMenu.windowRect, GearMenu.Fill, "Gear menu");

            if (showRouteMenu)
                MapMenu.windowRect = GUI.Window(2, MapMenu.windowRect, MapMenu.Fill, "Map menu");

            if (showStatMenu)
                StatMenu.windowRect = GUI.Window(3, StatMenu.windowRect, StatMenu.Fill, "Stat menu");
        }

        private void Fill(int _)
        {
            GUI.DragWindow(dragWindowRect);

            // Gold multiplier
            ModConfig.goldMultOn = GUI.Toggle(goldMultToggleRect,
                                           ModConfig.goldMultOn,
                                           $"Multiply incoming Gold by x{ModConfig.goldMultValue:f1}");

            ModConfig.goldMultValue = RoundToZeroFive(GUI.HorizontalSlider(goldMultSliderRect, ModConfig.goldMultValue, 0, 20));


            // Dark quartz multiplier
            ModConfig.dQuartzMultOn = GUI.Toggle(dQuartzMultToggleRect,
                                              ModConfig.dQuartzMultOn,
                                              $"Multiply incoming Dark Quartz by x{ModConfig.dQuartzMultValue:f1}");

            ModConfig.dQuartzMultValue = RoundToZeroFive(GUI.HorizontalSlider(dQuartzMultSliderRect, ModConfig.dQuartzMultValue, 0, 20));


            // Bones multiplier
            ModConfig.bonesMultOn = GUI.Toggle(bonesMultToggleRect,
                                            ModConfig.bonesMultOn,
                                            $"Multiply incoming Bones by x{ModConfig.bonesMultValue:f1}");

            ModConfig.bonesMultValue = RoundToZeroFive(GUI.HorizontalSlider(bonesMultSliderRect, ModConfig.bonesMultValue, 0, 20));


            // Heart quartz multiplier
            ModConfig.hQuartzMultOn = GUI.Toggle(hQuartzMultToggleRect,
                                              ModConfig.hQuartzMultOn,
                                              $"Multiply incoming Heart Quartz by x{ModConfig.hQuartzMultValue:f1}");

            ModConfig.hQuartzMultValue = RoundToZeroFive(GUI.HorizontalSlider(hQuartzMultSliderRect, ModConfig.hQuartzMultValue, 0, 20));


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
            ModConfig.forceEasyModeOn = GUI.Toggle(easyModeToggleRect,
                                                ModConfig.forceEasyModeOn,
                                                $"Force easy-mode");

            // God mode
            ModConfig.godmodeOn = GUI.Toggle(godModeToggleRect,
                                          ModConfig.godmodeOn,
                                          $"God mode");


            // Turbo-attack
            ModConfig.turboAttackOn = GUI.Toggle(turboAttackToggleRect,
                                              ModConfig.turboAttackOn,
                                              $"Turbo-attack");

            // Turbo-dash
            ModConfig.turboDashOn = GUI.Toggle(turboDashToggleRect,
                                            ModConfig.turboDashOn,
                                            $"Turbo-dash");


            // Buttons
            if (GUI.Button(routeMenuButtonRect, "Map menu"))
                showRouteMenu = !showRouteMenu;

            if (GUI.Button(gearMenuButtonRect, "Gear menu"))
                showGearMenu = !showGearMenu;

            if (GUI.Button(statMenuButtonRect, "Stat menu"))
                showStatMenu = !showStatMenu;

            if (GUI.Button(saveConfigButtonRect, "Save config"))
                ModConfig.Save();
        }

        private static float RoundToZeroFive(float num) => Mathf.Round(num * 2) / 2;


        // Menu elements
        private int menuWidth;
        private int menuHeight;

        private Rect dragWindowRect;
        private Rect windowRect;

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

        private Rect routeMenuButtonRect;
        private Rect gearMenuButtonRect;
        private Rect statMenuButtonRect;
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
            menuHeight = (int) (unit * 24.5f);

            windowRect = new Rect(Screen.width - menuWidth - unit, unit, menuWidth, menuHeight);
            int row = 0;

            dragWindowRect = new Rect(0, 0, menuWidth, unit);
            row++;

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
            row++;

            // Stat menu
            statMenuButtonRect = new Rect(halfWidth + unit, (unit + 0.5f) * row * 1.5f + unit, halfWidth, unit);
            row++;

            // Save config
            saveConfigButtonRect = new Rect(halfWidth + unit, (unit + 0.5f) * row * 1.5f + unit, halfWidth, unit);
        }
    }
}
