using Characters.Gear.Items;
using Characters.Player;
using Data;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkulPatcher.Mods
{
    [HarmonyPatch]
    public static class ItemInventoryFuncs
    {
        #region Save

        private const int InvPageSize = 9;
        private const int MaxInvPageCount = 10;

        // Getters are called CONSTANTLY while loading, stroing the values instead of re-allocating memory for them
        private static StringDataArray saveItems;
        private static FloatDataArray saveItemStacks;
        private static IntDataArray saveItemKeywords1;
        private static IntDataArray saveItemKeywords2;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(GameData.Save), nameof(GameData.Save.items), MethodType.Getter)]
        private static bool GetItemsSave(ref StringDataArray __result)
        {
            saveItems ??= new StringDataArray("Save/items", MaxInvPageCount * InvPageSize);
            __result = saveItems;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(GameData.Save), nameof(GameData.Save.itemStacks), MethodType.Getter)]
        private static bool GetItemsStacksSave(ref FloatDataArray __result)
        {
            saveItemStacks ??= new FloatDataArray("Save/itemStacks", MaxInvPageCount * InvPageSize);
            __result = saveItemStacks;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(GameData.Save), nameof(GameData.Save.itemKeywords1), MethodType.Getter)]
        private static bool GetItemsKeywords1Save(ref IntDataArray __result)
        {
            saveItemKeywords1 ??= new IntDataArray("Save/itemKeywords1", MaxInvPageCount * InvPageSize);
            __result = saveItemKeywords1;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(GameData.Save), nameof(GameData.Save.itemKeywords2), MethodType.Getter)]
        private static bool GetItemsKeywords2Save(ref IntDataArray __result)
        {
            saveItemKeywords2 ??= new IntDataArray("Save/itemKeywords2", MaxInvPageCount * InvPageSize);
            __result = saveItemKeywords2;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Inventory), nameof(Inventory.Save))]
        [HarmonyPatch(typeof(Inventory), nameof(Inventory.LoadSavedItemsFromPreloader))]
        private static void SetInventoryPagesOnSaveOrLoadPrefix(out int __state)
        {
            ModConfig.Inventory.item.items.RemoveAll(i => i is null);

            __state = ModConfig.inventoryPagesCount;  // Save inventoryPagesCount to set it back in postfix
            ModConfig.inventoryPagesCount = MaxInvPageCount;
            SetInventoryPagesCount();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Inventory), nameof(Inventory.Save))]
        [HarmonyPatch(typeof(Inventory), nameof(Inventory.LoadSavedItemsFromPreloader))]
        private static void SetInventoryPagesOnSaveOrLoadPostfix(int __state)
        {
            ModConfig.inventoryPagesCount = Math.Max(ResolveSavedInventoryPagesCount(), __state);
            SetInventoryPagesCount();
        }

        private static int ResolveSavedInventoryPagesCount()
        {
            int firstNullIndex = saveItems._dataArray.TakeWhile(s => !string.IsNullOrEmpty(s.value)).Count();
            int InvPagesCount = Math.DivRem(firstNullIndex, InvPageSize, out int rem);

            if (rem > 0)
                InvPagesCount++;

            return rem == 0 ? InvPagesCount : InvPagesCount++;
        }

        #endregion

        #region Funcs

        public static void SetInventoryPagesCount()
        {
            if (!ModConfig.IsInGame)
                return;

            List<Item> items = ModConfig.Inventory.item.items;
            if (items.Count / InvPageSize < ModConfig.inventoryPagesCount)
            {
                items.AddRange(Enumerable.Repeat<Item>(null, (ModConfig.inventoryPagesCount - (items.Count / InvPageSize)) * InvPageSize));
            }
            else
            {
                while (items.Count / InvPageSize != ModConfig.inventoryPagesCount)
                    items.RemoveRange(items.Count - InvPageSize, InvPageSize);
            }
        }

        public static void InventorySetPrevPage()
        {
            if (!ModConfig.IsInGame)
                return;

            List<Item> items = ModConfig.Inventory.item.items;
            int preLastPageCount = items.Count - InvPageSize;

            List<Item> savedItems = items.Skip(preLastPageCount).Take(InvPageSize).ToList();
            items.RemoveRange(preLastPageCount, InvPageSize);
            items.InsertRange(0, savedItems);

            ModConfig.UI.inventory.UpdateGearInfo();
        }

        public static void InventorySetNextPage()
        {
            if (!ModConfig.IsInGame)
                return;

            List<Item> items = ModConfig.Inventory.item.items;

            List<Item> savedItems = items.Take(InvPageSize).ToList();
            items.RemoveRange(0, InvPageSize);
            items.AddRange(savedItems);

            ModConfig.UI.inventory.UpdateGearInfo();
        }

        #endregion
    }
}
