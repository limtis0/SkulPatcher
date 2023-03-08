using Characters.Gear;
using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Weapons;
using Characters.Player;
using GameResources;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SkulPatcher
{
    public static class GearMenuFuncs
    {
        private const int InvPageSize = 9;

        public static void SpawnGear<T>(GearReference gearRef) where T : Gear
        {
            if (!ModConfig.IsInGame)
                return;

            GearRequest request = gearRef.LoadAsync();
            request.WaitForCompletion();

            T gear = (T)request.asset;
            gear.name = gearRef.name;

            gear = (T)ModConfig.Level.DropGear(gear, ModConfig.Level.player.transform.position);

            if (ModConfig.autoEquipSpawnedGear)
                TryEquipGear(gear);
        }

        private static void TryEquipGear<T>(T gear) where T : Gear
        {
            Type gearType = typeof(T);

            if (gearType == typeof(Item))
            {
                Inventory.item.TryEquip(gear as Item);
            }
            else if (gearType == typeof(Weapon))
            {
                Inventory.weapon.Polymorph(gear as Weapon);
                Inventory.weapon.Equip(gear as Weapon);
            }
            else if (gearType == typeof(Quintessence))
            {
                Inventory.quintessence.EquipAt(gear as Quintessence, 0);
            }
        }

        public static void RerollAbilities()
        {
            if (!ModConfig.IsInGame)
                return;

            Inventory.weapon.current.RerollSkills();
        }

        public static void SetInventoryPagesCount()
        {
            if (!ModConfig.IsInGame)
                return;

            List<Item> items = Inventory.item.items;
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

            List<Item> items = Inventory.item.items;
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

            List<Item> items = Inventory.item.items;

            List<Item> savedItems = items.Take(InvPageSize).ToList();
            items.RemoveRange(0, InvPageSize);
            items.AddRange(savedItems);

            ModConfig.UI.inventory.UpdateGearInfo();
        }

        public static void SetSkullLimit()
        {
            if (!ModConfig.IsInGame)
                return;

            Weapon[] newArray = Inventory.weapon.weapons;
            Array.Resize(ref newArray, ModConfig.skullLimit);

            new Traverse(Inventory.weapon).Field("weapons").SetValue(newArray);
        }

        public static Inventory Inventory => ModConfig.Level.player.playerComponents.inventory;
    }
}
