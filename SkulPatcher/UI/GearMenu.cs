using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Weapons;
using GameResources;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SkulPatcher.UI
{
    public class GearMenu
    {
        private static readonly List<ItemReference> items;
        private static readonly List<WeaponReference> skulls;
        private static readonly List<EssenceReference> essences;

        private static readonly Dictionary<ItemReference, string> itemLocalization;
        private static readonly Dictionary<WeaponReference, string> skullLocalization;
        private static readonly Dictionary<EssenceReference, string> essenceLocalization;

        static GearMenu()
        {
            items = Config.Gear.items.OrderBy(itemRef => GetLocalizedGearName(itemRef.displayNameKey)).ToList();  // Sort alphabetically
            skulls = Config.Gear.weapons.ToList();
            essences = Config.Gear.essences.OrderBy(essenceRef => GetLocalizedGearName(essenceRef.displayNameKey)).ToList();  // Sort alphabetically

            itemLocalization = new Dictionary<ItemReference, string>();
            skullLocalization = new Dictionary<WeaponReference, string>();
            essenceLocalization = new Dictionary<EssenceReference, string>();

            foreach (ItemReference item in items)
                itemLocalization.Add(item, GetLocalizedGearName(item.displayNameKey));

            foreach (WeaponReference skull in skulls)
                skullLocalization.Add(skull, GetLocalizedGearName(skull.displayNameKey));

            foreach (EssenceReference essence in essences)
                essenceLocalization.Add(essence, GetLocalizedGearName(essence.displayNameKey));
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

            int itemInd = 0;
            foreach (ItemReference itemRef in items)
            {
                if (GUI.Button(itemScrollButtonsRects[itemInd++], itemLocalization[itemRef]))
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

            int skullInd = 0;
            foreach (WeaponReference skullRef in skulls)
            {
                if (GUI.Button(skullScrollButtonsRects[skullInd++], skullLocalization[skullRef]))
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

            int essenceInd = 0;
            foreach (EssenceReference essenceRef in essences)
            {
                if (GUI.Button(essenceScrollButtonsRects[essenceInd++], essenceLocalization[essenceRef]))
                {
                    GearFuncs.SpawnGear<Quintessence>(essenceRef);
                }
            }

            GUI.EndScrollView();


            // Auto-equip spawned gear
            Config.autoEquipSpawnedGear = GUI.Toggle(autoEquipGearToggleRect, Config.autoEquipSpawnedGear, "Auto-equip spawned gear");

            // Item limit
            GUI.Label(itemLimitLabelRect, $"Item limit ({Config.itemLimit})");
            Config.itemLimit = (int)GUI.HorizontalSlider(itemLimitSliderRect, Config.itemLimit, 9f, 32f);

            // Skull limit
            GUI.Label(skullLimitLabelRect, $"Skull limit ({Config.skullLimit})");
            Config.skullLimit = (int)GUI.HorizontalSlider(skullLimitSliderRect, Config.skullLimit, 2f, 32f);

            // Apply changes
            if (GUI.Button(applyChangesButton, "Apply changes"))
            {
                GearFuncs.SetItemLimit();
                GearFuncs.SetSkullLimit();
            }

            // Reroll abilities
            if (GUI.Button(rerollAbilitiesButtonRect, "Reroll abilities"))
                GearFuncs.RerollAbilities();

        }

        // Menu elements
        public static Rect windowRect;

        private static int menuWidth;
        private static int menuHeight;

        private static Rect dragWindowRect;

        private static Rect spawnItemLabelRect;
        private static Rect spawnSkullLabelRect;

        private static Rect itemScrollPosRect;
        private static Rect itemScrollViewRect;
        private static Vector2 itemScrollVec = Vector2.zero;
        private static List<Rect> itemScrollButtonsRects;

        private static Rect skullScrollPosRect;
        private static Rect skullScrollViewRect;
        private static Vector2 skullScrollVec = Vector2.zero;
        private static List<Rect> skullScrollButtonsRects;

        private static Rect essenceLabelRect;

        private static Rect essenceScrollPosRect;
        private static Rect essenceScrollViewRect;
        private static Vector2 essenceScrollVec = Vector2.zero;
        private static List<Rect> essenceScrollButtonsRects;

        private static Rect rerollAbilitiesButtonRect;

        private static Rect autoEquipGearToggleRect;

        private static Rect itemLimitLabelRect;
        private static Rect itemLimitSliderRect;

        private static Rect skullLimitLabelRect;
        private static Rect skullLimitSliderRect;

        private static Rect applyChangesButton;

        public static void Resize()
        {
            menuWidth = Screen.width / 4;
            menuHeight = Menu.unit * 24;

            windowRect = new Rect(20, 11 * Menu.unit + 20, menuWidth, menuHeight);
            int row = 0;

            dragWindowRect = new Rect(0, 0, menuWidth, Menu.unit);
            row++;

            // Gear spawn
            float scrollWidth = menuWidth / 2 - Menu.unit;
            float scrollHeight = Menu.unit * 9;


            // Item spawn
            spawnItemLabelRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, scrollWidth, Menu.unit);
            spawnSkullLabelRect = new Rect(scrollWidth + Menu.unit, Menu.unit * row * 1.5f, scrollWidth, Menu.unit);
            row++;

            itemScrollPosRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, scrollWidth, scrollHeight);
            itemScrollViewRect = new Rect(0, 0, scrollWidth, Menu.unit * 1.5f * Config.Gear.items.Count);

            itemScrollButtonsRects = new List<Rect>();
            for (int i = 0; i < Config.Gear.items.Count; i++)
            {
                itemScrollButtonsRects.Add(new Rect(0, Menu.unit * i * 1.5f, scrollWidth, Menu.unit));
            }


            // Skull spawn
            skullScrollPosRect = new Rect(scrollWidth + Menu.unit, Menu.unit * row * 1.5f, scrollWidth, scrollHeight);
            skullScrollViewRect = new Rect(0, 0, scrollWidth, Menu.unit * 1.5f * Config.Gear.weapons.Count);

            skullScrollButtonsRects = new List<Rect>();
            for (int i = 0; i < Config.Gear.weapons.Count; i++)
            {
                skullScrollButtonsRects.Add(new Rect(0, Menu.unit * i * 1.5f, scrollWidth, Menu.unit));
            }
            row += 7;


            // Essence spawn
            essenceLabelRect = essenceLabelRect = new Rect(scrollWidth + Menu.unit, Menu.unit * row * 1.5f, scrollWidth, Menu.unit);
            row++;

            essenceScrollPosRect = new Rect(scrollWidth + Menu.unit, Menu.unit * row * 1.5f, scrollWidth, scrollHeight);
            essenceScrollViewRect = new Rect(0, 0, scrollWidth, Menu.unit * 1.5f * Config.Gear.essences.Count);

            essenceScrollButtonsRects = new List<Rect>();
            for (int i = 0; i < Config.Gear.essences.Count; i++)
            {
                essenceScrollButtonsRects.Add(new Rect(0, Menu.unit * i * 1.5f, scrollWidth, Menu.unit));
            }
            row--;

            // Auto-equip
            autoEquipGearToggleRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, scrollWidth, Menu.unit);
            row++;

            // Item limit
            itemLimitLabelRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, scrollWidth, Menu.unit);
            row++;

            itemLimitSliderRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, scrollWidth - Menu.unit, Menu.unit);
            row++;

            // Skull limit
            skullLimitLabelRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, scrollWidth, Menu.unit);
            row++;

            skullLimitSliderRect = new Rect(Menu.unit, Menu.unit * row * 1.5f, scrollWidth - Menu.unit, Menu.unit);
            row++;

            // Apply changes
            applyChangesButton = new Rect(Menu.unit * 1.5f, Menu.unit * row * 1.5f, scrollWidth - (Menu.unit * 2), Menu.unit);
            row++;

            // Reroll abilities
            rerollAbilitiesButtonRect = new Rect(Menu.unit * 1.5f, Menu.unit * row * 1.5f, scrollWidth - (Menu.unit * 2), Menu.unit);
        }
    }
}
