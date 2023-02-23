using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Weapons;
using GameResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SkulPatcher.UI
{
    public static class GearMenu
    {
        private static bool gearInitialized = false;

        private static ItemReference[] items;
        private static WeaponReference[] skulls;
        private static EssenceReference[] essences;

        private static Dictionary<ItemReference, string> itemLocalization;
        private static Dictionary<WeaponReference, string> skullLocalization;
        private static Dictionary<EssenceReference, string> essenceLocalization;

        private static string searchText = string.Empty;
        private static string searchTextPreviousState = searchText;

        private static bool filterLegendary = true;
        private static bool filterLegendaryPreviousState = filterLegendary;

        private static bool filterUnique = true;
        private static bool filterUniquePreviousState = filterUnique;

        private static bool filterRare = true;
        private static bool filterRarePreviousState = filterRare;

        private static bool filterCommon = true;
        private static bool filterCommonPreviousState = filterCommon;

        private static void InitGear()
        {
            if (gearInitialized)
                return;

            gearInitialized = true;

            itemLocalization = new();
            skullLocalization = new();
            essenceLocalization = new();

            // Fill localization
            foreach (ItemReference itemRef in ModConfig.Gear.items)
                itemLocalization.Add(itemRef, GetLocalizedGearName(itemRef.displayNameKey));

            foreach (WeaponReference skull in ModConfig.Gear.weapons)
                skullLocalization.Add(skull, GetLocalizedGearName(skull.displayNameKey));

            foreach (EssenceReference essence in ModConfig.Gear.essences)
                essenceLocalization.Add(essence, GetLocalizedGearName(essence.displayNameKey));

            // Fill gear lists
            items = ModConfig.Gear.items.OrderBy(itemRef => itemLocalization[itemRef]).ToArray();  // Sort alphabetically
            skulls = ModConfig.Gear.weapons.ToArray();
            essences = ModConfig.Gear.essences.OrderBy(essenceRef => essenceLocalization[essenceRef]).ToArray();  // Sort alphabetically

            // Init gear-related rects
            itemScrollButtonsRects = Enumerable.Repeat(new Rect(), ModConfig.Gear.items.Count).ToArray();
            itemScrollButtonsVisible = Enumerable.Repeat(true, ModConfig.Gear.items.Count).ToArray();

            skullScrollButtonsRects = Enumerable.Repeat(new Rect(), ModConfig.Gear.weapons.Count).ToArray();
            skullScrollButtonsVisible = Enumerable.Repeat(true, ModConfig.Gear.weapons.Count).ToArray();

            essenceScrollButtonsRects = Enumerable.Repeat(new Rect(), ModConfig.Gear.essences.Count).ToArray();
            essenceScrollButtonsVisible = Enumerable.Repeat(true, ModConfig.Gear.essences.Count).ToArray();
        }

        private static string GetLocalizedGearName(string displayNameKey)
        {
            string localized = Localization.GetLocalizedString(displayNameKey);
            return string.IsNullOrEmpty(localized) ? displayNameKey : localized;
        }

        public static void Fill(int _)
        {
            GUI.DragWindow(dragWindowRect);

            // Gear spawn
            GUI.Label(spawnItemLabelRect, "Spawn item");
            GUI.Label(spawnSkullLabelRect, "Spawn skull");


            // Items
            itemScrollVec = GUI.BeginScrollView(itemScrollPosRect,
                                                itemScrollVec,
                                                itemScrollViewRect,
                                                GUIStyle.none,
                                                GUIStyle.none);

            for (int i = 0; i < items.Length; i++)
            {
                if (!itemScrollButtonsVisible[i])
                    continue;

                ItemReference itemRef = items[i];

                if (GUI.Button(itemScrollButtonsRects[i], itemLocalization[itemRef]))
                {
                    GearFuncs.SpawnGear<Item>(itemRef);
                }
            }

            GUI.EndScrollView();


            // Skulls
            skullScrollVec = GUI.BeginScrollView(skullScrollPosRect,
                                                 skullScrollVec,
                                                 skullScrollViewRect,
                                                 GUIStyle.none,
                                                 GUIStyle.none);

            for (int i = 0; i < skulls.Length; i++)
            {
                if (!skullScrollButtonsVisible[i])
                    continue;

                WeaponReference skullRef = skulls[i];

                if (GUI.Button(skullScrollButtonsRects[i], skullLocalization[skullRef]))
                {
                    GearFuncs.SpawnGear<Weapon>(skullRef);
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

            for (int i = 0; i < essences.Length; i++)
            {
                if (!essenceScrollButtonsVisible[i])
                    continue;

                EssenceReference essenceRef = essences[i];

                if (GUI.Button(essenceScrollButtonsRects[i], essenceLocalization[essenceRef]))
                {
                    GearFuncs.SpawnGear<Quintessence>(essenceRef);
                }
            }

            GUI.EndScrollView();


            // Auto-equip spawned gear
            ModConfig.autoEquipSpawnedGear = GUI.Toggle(autoEquipGearToggleRect, ModConfig.autoEquipSpawnedGear, "Auto-equip spawned gear");


            // Item limit
            GUI.Label(itemLimitLabelRect, $"Item limit ({ModConfig.itemLimit})");
            ModConfig.itemLimit = (int)GUI.HorizontalSlider(itemLimitSliderRect, ModConfig.itemLimit, 9f, 32f);

            // Skull limit
            GUI.Label(skullLimitLabelRect, $"Skull limit ({ModConfig.skullLimit})");
            ModConfig.skullLimit = (int)GUI.HorizontalSlider(skullLimitSliderRect, ModConfig.skullLimit, 2f, 32f);


            // Apply changes
            if (GUI.Button(applyChangesButton, "Apply changes"))
            {
                GearFuncs.SetItemLimit();
                GearFuncs.SetSkullLimit();
            }

            // Reroll abilities
            if (GUI.Button(rerollAbilitiesButtonRect, "Reroll abilities"))
                GearFuncs.RerollAbilities();


            // Filter label
            GUI.Label(filterLabelRect, "Filter:");

            // Filter toggles
            filterLegendary = GUI.Toggle(filterLegendaryRect, filterLegendary, "Legendary");
            if (filterLegendary != filterLegendaryPreviousState)
            {
                filterLegendaryPreviousState = filterLegendary;
                SetViewRects();
            }

            filterUnique = GUI.Toggle(filterUniqueRect, filterUnique, "Antique");
            if (filterUnique != filterUniquePreviousState)
            {
                filterUniquePreviousState = filterUnique;
                SetViewRects();
            }

            filterRare = GUI.Toggle(filterRareRect, filterRare, "Rare");
            if (filterRare != filterRarePreviousState)
            {
                filterRarePreviousState = filterRare;
                SetViewRects();
            }

            filterCommon = GUI.Toggle(filterCommonRect, filterCommon, "Common");
            if (filterCommon != filterCommonPreviousState)
            {
                filterCommonPreviousState = filterCommon;
                SetViewRects();
            }


            // Search field
            GUI.Label(searchLabelRect, "Search:");
            searchText = GUI.TextField(searchFieldRect, searchText);
            
            if (searchText != searchTextPreviousState)
            {
                searchTextPreviousState = searchText;
                SetViewRects();
            }


            // Duplicate gear
            ModConfig.allowDuplicateItems = GUI.Toggle(duplicateItemsToggleRect, ModConfig.allowDuplicateItems, "Allow duplicate items");
            ModConfig.allowDuplicateSkulls = GUI.Toggle(duplicateSkullsToggleRect, ModConfig.allowDuplicateSkulls, "Allow duplicate skulls");
        }

        // Menu elements
        public static Rect windowRect;

        private static int menuWidth;
        private static int menuHeight;

        private static int listWidth;

        private static Rect dragWindowRect;

        private static Rect spawnItemLabelRect;
        private static Rect spawnSkullLabelRect;

        private static Rect itemScrollPosRect;
        private static Rect itemScrollViewRect;
        private static Vector2 itemScrollVec = Vector2.zero;
        private static Rect[] itemScrollButtonsRects;
        private static bool[] itemScrollButtonsVisible;

        private static Rect skullScrollPosRect;
        private static Rect skullScrollViewRect;
        private static Vector2 skullScrollVec = Vector2.zero;
        private static Rect[] skullScrollButtonsRects;
        private static bool[] skullScrollButtonsVisible;

        private static Rect essenceLabelRect;

        private static Rect essenceScrollPosRect;
        private static Rect essenceScrollViewRect;
        private static Vector2 essenceScrollVec = Vector2.zero;
        private static Rect[] essenceScrollButtonsRects;
        private static bool[] essenceScrollButtonsVisible;

        private static Rect rerollAbilitiesButtonRect;

        private static Rect autoEquipGearToggleRect;

        private static Rect itemLimitLabelRect;
        private static Rect itemLimitSliderRect;

        private static Rect skullLimitLabelRect;
        private static Rect skullLimitSliderRect;

        private static Rect applyChangesButton;

        private static Rect filterLabelRect;

        private static Rect filterLegendaryRect;
        private static Rect filterUniqueRect;
        private static Rect filterRareRect;
        private static Rect filterCommonRect;

        private static Rect searchLabelRect;
        private static Rect searchFieldRect;

        private static Rect duplicateItemsToggleRect;
        private static Rect duplicateSkullsToggleRect;

        public static void Resize()
        {
            menuWidth = Screen.width / 4;
            menuHeight = Menu.unit * 35;

            windowRect = new Rect(Menu.unit, 13 * Menu.unit, menuWidth, menuHeight);
            int row = 0;

            dragWindowRect = new Rect(0, 0, menuWidth, Menu.unit);
            row++;

            // Gear spawn
            InitGear();

            listWidth = menuWidth / 2 - Menu.unit;
            float listHeight = Menu.unit * 9;

            SetViewRects();

            // Item spawn
            spawnItemLabelRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, listWidth, Menu.unit);
            spawnSkullLabelRect = new Rect(listWidth + Menu.unit, Menu.unit * row * 1.5f, listWidth, Menu.unit);
            row++;

            itemScrollPosRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, listWidth, listHeight);

            // Skull spawn
            skullScrollPosRect = new Rect(listWidth + Menu.unit, Menu.unit * row * 1.5f, listWidth, listHeight);
            row += 7;

            // Essence spawn
            essenceLabelRect = essenceLabelRect = new Rect(listWidth + Menu.unit, Menu.unit * row * 1.5f, listWidth, Menu.unit);
            row++;

            essenceScrollPosRect = new Rect(listWidth + Menu.unit, Menu.unit * row * 1.5f, listWidth, listHeight);
            row--;

            // Auto-equip
            autoEquipGearToggleRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, listWidth, Menu.unit);
            row++;

            // Item limit
            itemLimitLabelRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, listWidth, Menu.unit);
            row++;

            itemLimitSliderRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, listWidth - Menu.unit, Menu.unit);
            row++;

            // Skull limit
            skullLimitLabelRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, listWidth, Menu.unit);
            row++;

            skullLimitSliderRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, listWidth - Menu.unit, Menu.unit);
            row++;

            // Apply changes
            applyChangesButton = new Rect(Menu.unit * 1.5f, Menu.unit * row * 1.5f, listWidth - (Menu.unit * 2), Menu.unit);
            row++;

            // Reroll abilities
            rerollAbilitiesButtonRect = new Rect(Menu.unit * 1.5f, Menu.unit * row * 1.5f, listWidth - (Menu.unit * 2), Menu.unit);
            row += 2;

            // Filter label
            filterLabelRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, listWidth, Menu.unit);
            row++;

            // Filter toggles
            filterLegendaryRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, listWidth, Menu.unit);
            row++;

            filterUniqueRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, listWidth, Menu.unit);
            row++;

            filterRareRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, listWidth, Menu.unit);
            row++;

            filterCommonRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, listWidth, Menu.unit);
            row++;

            // Search field
            searchLabelRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, Menu.unit * 3, Menu.unit);
            searchFieldRect = new Rect(Menu.unit * 4, Menu.unit * row * 1.5f, listWidth - Menu.unit * 4, Menu.unit);
            row -= 5;

            // Duplicate gear
            duplicateItemsToggleRect = new Rect(listWidth + Menu.unit, Menu.unit * row * 1.5f, listWidth, Menu.unit);
            row++;

            duplicateSkullsToggleRect = new Rect(listWidth + Menu.unit, Menu.unit * row * 1.5f, listWidth, Menu.unit);
        }

        private static void SetViewRects()
        {
            itemScrollViewRect = FilterGearAndGetViewRect(items, itemLocalization, itemScrollButtonsVisible, itemScrollButtonsRects);
            skullScrollViewRect = FilterGearAndGetViewRect(skulls, skullLocalization, skullScrollButtonsVisible, skullScrollButtonsRects);
            essenceScrollViewRect = FilterGearAndGetViewRect(essences, essenceLocalization, essenceScrollButtonsVisible, essenceScrollButtonsRects);
        }

        private static Rect FilterGearAndGetViewRect<T>(T[] gear, Dictionary<T, string> localization, bool[] toShow, Rect[] elements) where T : GearReference
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
            for (int i = 0; i < gear.Length; i++)
            {
                if (!IsGearFilteredByRarity(gear[i]) || !regex.Match(localization[gear[i]]).Success)
                {
                    toShow[i] = false;
                    continue;
                }

                toShow[i] = true;

                elements[i] = new Rect(0, elementRow * Menu.unit * 1.5f, listWidth, Menu.unit);
                elementRow++;
            }

            return new Rect(0, 0, listWidth, Menu.unit * 1.5f * elementRow);
        }

        private static bool IsGearFilteredByRarity<T>(T gear) where T : GearReference
        {
            return gear.rarity switch
            {
                Rarity.Legendary => filterLegendary,
                Rarity.Unique => filterUnique,
                Rarity.Rare => filterRare,
                Rarity.Common => filterCommon,
                _ => true,
            };
        }
    }
}
