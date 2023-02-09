using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Weapons;
using Data;
using GameResources;
using SkulPatcher.Misc;
using System;
using UnityEngine;

namespace SkulPatcher
{
    public class Menu : MonoBehaviour
    {
        private static bool showMenu = true;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F7))
            {
                showMenu = !showMenu;
                Cursor.visible = showMenu;
            }
        }

        // Menu config
        private static int menuWidth = Screen.width / 4;
        private static int menuHeight = Screen.height / 3;
        private const int menuCornerSpace = 20;

        private Rect windowRect = new Rect(Screen.width - menuWidth - menuCornerSpace, menuCornerSpace, menuWidth, menuHeight);
        private static int unit = Screen.height / 25;  // Default unit for sizing
        private static int fontSize = (int)(unit * 0.8);

        // Scroll views
        private static Vector2 itemScrollVec = Vector2.zero;
        private static Vector2 skullScrollVec = Vector2.zero;
        private static Vector2 essenceScrollVec = Vector2.zero;

        public void Start()
        {
            GUI.skin.toggle.alignment = TextAnchor.MiddleLeft;
        }

        public void OnGUI()
        {
            if (!showMenu)
                return;

            // Dynamic sizing
            menuWidth = Screen.width / 4;
            menuHeight = (int)(Screen.height * 0.9);
            unit = Screen.height / 50;
            fontSize = (int)(unit * 0.75);

            GUI.skin.label.fontSize = fontSize;
            GUI.skin.toggle.fontSize = fontSize;
            GUI.skin.button.fontSize = fontSize;
            GUI.skin.textField.fontSize = fontSize;

            windowRect.width = menuWidth;
            windowRect.height = menuHeight;
            windowRect = GUI.Window(0, windowRect, FillMenu, "SkulPatcher");
        }

        private void FillMenu(int windowID)
        {
            int row = 0;

            GUI.DragWindow(new Rect(0, 0, menuWidth, unit));
            row++;

            // Luck boost
            Config.luckBoostOn = GUI.Toggle(new Rect(unit, unit * row * 1.5f, menuWidth - unit, unit),
                                     Config.luckBoostOn,
                                     $"Boost item rarity with {Config.luckBoostPercent}% chance");
            row++;

            Config.luckBoostPercent = (int)GUI.HorizontalSlider(new Rect(unit, unit * row * 1.5f, menuWidth - 2 * unit, unit), Config.luckBoostPercent, 0, 100);
            row++;

            Config.luckBoostContinuous = GUI.Toggle(new Rect(unit, unit * row * 1.5f, menuWidth - unit, unit),
                                     Config.luckBoostContinuous,
                                     $"Continue boosting rarity if the roll was successful");
            row += 2;


            // Gold multiplier
            Config.goldMultOn = GUI.Toggle(new Rect(unit, unit * row * 1.5f, menuWidth - unit, unit),
                                     Config.goldMultOn,
                                     $"Multiply incoming Gold by x{Config.goldMult}");
            row++;

            Config.goldMult = (int)GUI.HorizontalSlider(new Rect(unit, unit * row * 1.5f, menuWidth - 2 * unit, unit), Config.goldMult, 1, 50);
            row++;


            // Dark Quartz multiplier
            Config.dQuartzMultOn = GUI.Toggle(new Rect(unit, unit * row * 1.5f, menuWidth - unit, unit),
                                     Config.dQuartzMultOn,
                                     $"Multiply incoming Dark Quartz by x{Config.dQuartzMult}");
            row++;

            Config.dQuartzMult = (int)GUI.HorizontalSlider(new Rect(unit, unit * row * 1.5f, menuWidth - 2 * unit, unit), Config.dQuartzMult, 1, 50);
            row++;


            // Bones multiplier
            Config.bonesMultOn = GUI.Toggle(new Rect(unit, unit * row * 1.5f, menuWidth - unit, unit),
                                     Config.bonesMultOn,
                                     $"Multiply incoming Bones by x{Config.bonesMult}");
            row++;

            Config.bonesMult = (int)GUI.HorizontalSlider(new Rect(unit, unit * row * 1.5f, menuWidth - 2 * unit, unit), Config.bonesMult, 1, 50);
            row++;


            // Heart Quartz multiplier
            Config.hQuartzMultOn = GUI.Toggle(new Rect(unit, unit * row * 1.5f, menuWidth - unit, unit),
                                     Config.hQuartzMultOn,
                                     $"Multiply incoming Heart Quartz by x{Config.hQuartzMult}");
            row++;

            Config.hQuartzMult = (int)GUI.HorizontalSlider(new Rect(unit, unit * row * 1.5f, menuWidth - 2 * unit, unit), Config.hQuartzMult, 1, 50);
            row++;


            // Gold value
            GUI.Label(new Rect(unit, unit * row * 1.5f, unit * 5, unit * 1.2f), "Gold");
            GameData.Currency.gold.balance = Convert.ToInt32(
                GUI.TextField(new Rect(unit, unit * (row + 1) * 1.5f, unit * 4, unit), GameData.Currency.gold.balance.ToString()));

            // Dark Quartz value
            GUI.Label(new Rect(unit * 6.5f, unit * row * 1.5f, unit * 5, unit * 1.2f), "Dark Quartz");
            GameData.Currency.darkQuartz.balance = Convert.ToInt32(
                GUI.TextField(new Rect(unit * 6.5f, unit * (row + 1) * 1.5f, unit * 4, unit), GameData.Currency.darkQuartz.balance.ToString()));

            // Bones value
            GUI.Label(new Rect(unit * 12f, unit * row * 1.5f, unit * 5, unit * 1.2f), "Bones");
            GameData.Currency.bone.balance = Convert.ToInt32(
                GUI.TextField(new Rect(unit * 12f, unit * (row + 1) * 1.5f, unit * 4, unit), GameData.Currency.bone.balance.ToString()));

            // Heart Quartz value
            GUI.Label(new Rect(unit * 17.5f, unit * row * 1.5f, unit * 5, unit * 1.2f), "Heart Quartz");
            GameData.Currency.heartQuartz.balance = Convert.ToInt32(
                GUI.TextField(new Rect(unit * 17.5f, unit * (row + 1) * 1.5f, unit * 4, unit), GameData.Currency.heartQuartz.balance.ToString()));

            row += 3;


            // Gear spawn

            float scrollWidth = menuWidth / 2 - unit;
            float scrollHeight = unit * 8;

            GUI.Label(new Rect(unit, unit * row * 1.5f, scrollWidth, unit), "Spawn item");
            GUI.Label(new Rect(scrollWidth + unit, unit * row * 1.5f, scrollWidth, unit), "Spawn skull");
            row++;

            // Items
            itemScrollVec = GUI.BeginScrollView(new Rect(unit, unit * row * 1.5f, scrollWidth, scrollHeight),
                                                itemScrollVec,
                                                new Rect(0, 0, scrollWidth, unit * 1.5f * GearSpawn.gear.items.Count),
                                                GUIStyle.none,
                                                GUIStyle.none);
            int tempRow = 0;
            foreach (ItemReference itemRef in GearSpawn.gear.items)
            {
                if (GUI.Button(new Rect(0, unit * tempRow * 1.5f, scrollWidth, unit), 
                    $"{Localization.GetLocalizedString(itemRef.displayNameKey)}"))
                {
                    GearSpawn.SpawnGear<Item>(itemRef);
                }

                tempRow++;
            }
            GUI.EndScrollView();


            // Skulls
            skullScrollVec = GUI.BeginScrollView(new Rect(scrollWidth + unit, unit * row * 1.5f, scrollWidth, scrollHeight),
                                                skullScrollVec,
                                                new Rect(0, 0, scrollWidth, unit * 1.5f * GearSpawn.gear.weapons.Count),
                                                GUIStyle.none,
                                                GUIStyle.none);
            tempRow = 0;
            foreach (WeaponReference skullRef in GearSpawn.gear.weapons)
            {
                if (GUI.Button(new Rect(0, unit * tempRow * 1.5f, scrollWidth, unit), 
                    $"{Localization.GetLocalizedString(skullRef.displayNameKey)}"))
                {
                    GearSpawn.SpawnGear<Weapon>(skullRef);
                }
                tempRow++;
            }
            GUI.EndScrollView();


            // Quintessences
            row += 6;
            GUI.Label(new Rect(scrollWidth + unit, unit * row * 1.5f, scrollWidth, unit), "Spawn quintessence");
            essenceScrollVec = GUI.BeginScrollView(new Rect(scrollWidth + unit + 1, (unit + 1) * row * 1.5f, scrollWidth, scrollHeight),
                                                essenceScrollVec,
                                                new Rect(0, 0, scrollWidth, (unit + 1) * 1.5f * GearSpawn.gear.essences.Count),
                                                GUIStyle.none,
                                                GUIStyle.none);
            tempRow = 0;
            foreach (EssenceReference essenceRef in GearSpawn.gear.essences)
            {
                if (GUI.Button(new Rect(0, unit * tempRow * 1.5f, scrollWidth, unit), 
                    $"{Localization.GetLocalizedString(essenceRef.displayNameKey)}"))
                {
                    GearSpawn.SpawnGear<Quintessence>(essenceRef);
                }
                tempRow++;
            }
            GUI.EndScrollView();


            // Easy mode
            Config.forceEasyModeOn = GUI.Toggle(new Rect(unit, unit * row * 1.5f, scrollWidth, unit),
                         Config.forceEasyModeOn,
                         $"Force easy-mode");
            row++;


            // God mode
            Config.godmodeOn = GUI.Toggle(new Rect(unit, unit * row * 1.5f, scrollWidth, unit),
                                          Config.godmodeOn,
                                          $"God mode");
            if (Config.godmodeOn != Config.godmodePreviousState)
            {
                Godmode.Set();
                Config.godmodePreviousState = Config.godmodeOn;
            }
            row++;

            // Turbo-attack
            Config.turboAttackOn = GUI.Toggle(new Rect(unit, unit * row * 1.5f, scrollWidth, unit),
                         Config.turboAttackOn,
                         $"Turbo-attack");
            row++;

            Config.turboDashOn = GUI.Toggle(new Rect(unit, unit * row * 1.5f, scrollWidth, unit),
                         Config.turboDashOn,
                         $"Turbo-dash");
        }
    }
}
