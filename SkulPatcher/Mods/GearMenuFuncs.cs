using Characters.Gear;
using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Weapons;
using GameResources;
using HarmonyLib;
using System;

namespace SkulPatcher
{
    public static class GearMenuFuncs
    {
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
                ModConfig.Inventory.item.TryEquip(gear as Item);
            }
            else if (gearType == typeof(Weapon))
            {
                ModConfig.Inventory.weapon.Polymorph(gear as Weapon);
                ModConfig.Inventory.weapon.Equip(gear as Weapon);
            }
            else if (gearType == typeof(Quintessence))
            {
                ModConfig.Inventory.quintessence.EquipAt(gear as Quintessence, 0);
            }
        }

        public static void RerollAbilities()
        {
            if (!ModConfig.IsInGame)
                return;

            ModConfig.Inventory.weapon.current.RerollSkills();
        }

        public static void SetSkullLimit()
        {
            if (!ModConfig.IsInGame)
                return;

            Weapon[] newArray = ModConfig.Inventory.weapon.weapons;
            Array.Resize(ref newArray, ModConfig.skullLimit);

            new Traverse(ModConfig.Inventory.weapon).Field("weapons").SetValue(newArray);
        }
    }
}
