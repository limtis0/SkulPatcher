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
using System.Reflection;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SkulPatcher
{
    public static class GearFuncs
    {
        public static void AwaitGear()
        {
            if (ModConfig.Gear is not null)
                return;

            MethodInfo preloadGearMethod = typeof(GameResourceLoader).GetMethod("PreloadGear", BindingFlags.NonPublic | BindingFlags.Instance);
            preloadGearMethod.Invoke(GameResourceLoader.instance, new object[] { });

            new Traverse(GameResourceLoader.instance).Field<AsyncOperationHandle<GearResource>>("_gearHandle").Value.WaitForCompletion();
        }

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

        public static void SetItemLimit()
        {
            if (!ModConfig.IsInGame)
                return;

            List<Item> items = Inventory.item.items;
            if (items.Count < ModConfig.itemLimit)
            {
                items.AddRange(Enumerable.Repeat<Item>(null, ModConfig.itemLimit - items.Count));
            }
            else
            {
                while (items.Count != ModConfig.itemLimit)
                    items.RemoveAt(items.Count - 1);
            }
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
