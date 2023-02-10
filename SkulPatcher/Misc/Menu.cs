using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Weapons;
using Data;
using GameResources;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkulPatcher
{
    public class Menu : MonoBehaviour
    {
        private bool init = false;
        private bool showMenu = true;

        // Menu config
        private int screenWidthPrevious;
        private int screenHeightPrevious;

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
                Resize();

                init = true;
                screenHeightPrevious = Screen.height;
                screenWidthPrevious = Screen.width;
            }

            windowRect = GUI.Window(0, windowRect, FillMenu, "SkulPatcher - F7 to toggle");
        }

        private void FillMenu(int windowID)
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
                                           $"Multiply incoming Gold by x{Config.goldMult}");

            Config.goldMult = (int)GUI.HorizontalSlider(goldMultSliderRect, Config.goldMult, 1, 50);


            // Dark quartz multiplier
            Config.dQuartzMultOn = GUI.Toggle(dQuartzMultToggleRect,
                                              Config.dQuartzMultOn,
                                              $"Multiply incoming Dark Quartz by x{Config.dQuartzMult}");

            Config.dQuartzMult = (int)GUI.HorizontalSlider(dQuartzMultSliderRect, Config.dQuartzMult, 1, 50);


            // Bones multiplier
            Config.bonesMultOn = GUI.Toggle(bonesMultToggleRect,
                                            Config.bonesMultOn,
                                            $"Multiply incoming Bones by x{Config.bonesMult}");

            Config.bonesMult = (int)GUI.HorizontalSlider(bonesMultSliderRect, Config.bonesMult, 1, 50);


            // Heart quartz multiplier
            Config.hQuartzMultOn = GUI.Toggle(hQuartzMultToggleRect,
                                     Config.hQuartzMultOn,
                                     $"Multiply incoming Heart Quartz by x{Config.hQuartzMult}");

            Config.hQuartzMult = (int)GUI.HorizontalSlider(hQuartzMultSliderRect, Config.hQuartzMult, 1, 50);


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

            // Gear spawn
            float scrollWidth = menuWidth / 2 - unit;

            GUI.Label(spawnItemLabelRect, "Spawn item");
            GUI.Label(spawnSkullLabelRect, "Spawn skull");

            // Items
            itemScrollVec = GUI.BeginScrollView(itemScrollPosRect,
                                                itemScrollVec,
                                                itemScrollViewRect,
                                                GUIStyle.none,
                                                GUIStyle.none);

            for (int i = 0; i < Config.gear.items.Count; i++)
            {
                ItemReference itemRef = Config.gear.items[i];
                if (GUI.Button(itemScrollButtonsRects[i], Localization.GetLocalizedString(itemRef.displayNameKey)))
                {
                    GearSpawn.SpawnGear<Item>(itemRef);
                }
            }

            GUI.EndScrollView();


            // Skulls
            skullScrollVec = GUI.BeginScrollView(skullScrollPosRect,
                                                 skullScrollVec,
                                                 skullScrollViewRect,
                                                 GUIStyle.none,
                                                 GUIStyle.none);

            for (int i = 0; i < Config.gear.weapons.Count; i++)
            {
                WeaponReference skullRef = Config.gear.weapons[i];
                if (GUI.Button(skullScrollButtonsRects[i], Localization.GetLocalizedString(skullRef.displayNameKey)))
                {
                    GearSpawn.SpawnGear<Weapon>(skullRef);
                }
            }

            GUI.EndScrollView();


            // Quintessences
            GUI.Label(essenceLabelRect, "Spawn quintessence");
            essenceScrollVec = GUI.BeginScrollView(essenceScrollPosRect,
                                                   essenceScrollVec,
                                                   essenceScrollViewRect,
                                                   GUIStyle.none,
                                                   GUIStyle.none);

            for (int i = 0; i < Config.gear.essences.Count; i++)
            {
                EssenceReference essenceRef = Config.gear.essences[i];
                if (GUI.Button(essenceScrollButtonsRects[i], Localization.GetLocalizedString(essenceRef.displayNameKey)))
                {
                    GearSpawn.SpawnGear<Quintessence>(essenceRef);
                }
            }

            GUI.EndScrollView();


            // Easy mode
            Config.forceEasyModeOn = GUI.Toggle(easyModeToggleRect,
                                                Config.forceEasyModeOn,
                                                $"Force easy-mode");

            // God mode
            Config.godmodeOn = GUI.Toggle(godModeToggleRect,
                                          Config.godmodeOn,
                                          $"God mode");

            if (Config.godmodeOn != Config.godmodePreviousState)
            {
                Godmode.Set();
                Config.godmodePreviousState = Config.godmodeOn;
            }

            // Turbo-attack
            Config.turboAttackOn = GUI.Toggle(turboAttackToggleRect,
                                              Config.turboAttackOn,
                                              $"Turbo-attack");

            // Turbo-dash
            Config.turboDashOn = GUI.Toggle(turboDashToggleRect,
                                            Config.turboDashOn,
                                            $"Turbo-dash");
        }

        // Sizing values
        private int menuWidth;
        private int menuHeight;

        private int unit;
        private int fontSize;

        // Menu elements
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

        private Rect spawnItemLabelRect;
        private Rect spawnSkullLabelRect;

        private Rect itemScrollPosRect;
        private Rect itemScrollViewRect;
        private Vector2 itemScrollVec = Vector2.zero;
        private List<Rect> itemScrollButtonsRects;

        private Rect skullScrollPosRect;
        private Rect skullScrollViewRect;
        private Vector2 skullScrollVec = Vector2.zero;
        private List<Rect> skullScrollButtonsRects;

        private Rect essenceLabelRect;

        private Rect essenceScrollPosRect;
        private Rect essenceScrollViewRect;
        private Vector2 essenceScrollVec = Vector2.zero;
        private List<Rect> essenceScrollButtonsRects;

        private Rect easyModeToggleRect;
        private Rect godModeToggleRect;

        private Rect turboAttackToggleRect;
        private Rect turboDashToggleRect;

        private void Resize()
        {
            menuWidth = Screen.width / 4;
            menuHeight = (int)(Screen.height * 0.9);
            unit = Screen.height / 50;
            fontSize = (int)(unit * 0.75);

            GUI.skin.label.fontSize = fontSize;
            GUI.skin.toggle.fontSize = fontSize;
            GUI.skin.button.fontSize = fontSize;
            GUI.skin.textField.fontSize = fontSize;

            windowRect = new Rect(Screen.width - menuWidth - 20, 20, menuWidth, menuHeight)
            {
                width = menuWidth,
                height = menuHeight
            };

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


            // Gear spawn
            float scrollWidth = menuWidth / 2 - unit;
            float scrollHeight = unit * 8;

            // Item spawn
            spawnItemLabelRect = new Rect(unit, unit * row * 1.5f, scrollWidth, unit);
            spawnSkullLabelRect = new Rect(scrollWidth + unit, unit * row * 1.5f, scrollWidth, unit);
            row++;

            itemScrollPosRect = new Rect(unit, unit * row * 1.5f, scrollWidth, scrollHeight);
            itemScrollViewRect = new Rect(0, 0, scrollWidth, unit * 1.5f * Config.gear.items.Count);

            itemScrollButtonsRects = new List<Rect>();
            for (int i = 0; i < Config.gear.items.Count; i++)
            {
                itemScrollButtonsRects.Add(new Rect(0, unit * i * 1.5f, scrollWidth, unit));
            }

            // Skull spawn
            skullScrollPosRect = new Rect(scrollWidth + unit, unit * row * 1.5f, scrollWidth, scrollHeight);
            skullScrollViewRect = new Rect(0, 0, scrollWidth, unit * 1.5f * Config.gear.weapons.Count);

            skullScrollButtonsRects = new List<Rect>();
            for (int i = 0; i < Config.gear.weapons.Count; i++)
            {
                skullScrollButtonsRects.Add(new Rect(0, unit * i * 1.5f, scrollWidth, unit));
            }

            row += 6;

            // Essence spawn
            essenceLabelRect = essenceLabelRect = new Rect(scrollWidth + unit, unit * row * 1.5f, scrollWidth, unit);

            essenceScrollPosRect = new Rect(scrollWidth + unit + 1, (unit + 1) * row * 1.5f, scrollWidth, scrollHeight);
            essenceScrollViewRect = new Rect(0, 0, scrollWidth, (unit + 1) * 1.5f * Config.gear.essences.Count);

            essenceScrollButtonsRects = new List<Rect>();
            for (int i = 0; i < Config.gear.essences.Count; i++)
            {
                essenceScrollButtonsRects.Add(new Rect(0, unit * i * 1.5f, scrollWidth, unit));
            }

            // Easy mode
            easyModeToggleRect = new Rect(unit, unit * row * 1.5f, scrollWidth, unit);
            row++;

            // God mode
            godModeToggleRect = new Rect(unit, unit * row * 1.5f, scrollWidth, unit);
            row++;

            // Turbo-attack
            turboAttackToggleRect = new Rect(unit, unit * row * 1.5f, scrollWidth, unit);
            row++;

            // Turbo-attack
            turboDashToggleRect = new Rect(unit, unit * row * 1.5f, scrollWidth, unit);
        }
    }
}
