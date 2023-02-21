using Characters.Gear;
using HarmonyLib;
using Services;
using System;
using System.Collections.Generic;

namespace SkulPatcher.Patches
{
    [HarmonyPatch(typeof(GearManager), nameof(GearManager.GetItemToTake), new Type[] { typeof(Random), typeof(Rarity) })]
    public static class DuplicateItemsPatch
    {
        private static Traverse itemInstances;
        private static List<Gear> savedInstances;
        private static readonly List<Gear> emptyList = new();

        public static void Prefix(GearManager __instance)
        {
            if (!ModConfig.allowDuplicateItems)
                return;

            itemInstances = Traverse.Create(__instance).Field("_itemInstances");
            savedInstances = itemInstances.GetValue() as List<Gear>;
            itemInstances.SetValue(emptyList);
        }

        public static void Postfix()
        {
            if (!ModConfig.allowDuplicateItems)
                return;

            itemInstances.SetValue(savedInstances);
        }
    }

    [HarmonyPatch(typeof(GearManager), nameof(GearManager.GetWeaponToTake), new Type[] { typeof(Random), typeof(Rarity) })]
    public static class DuplicateSkullsPatch
    {
        private static Traverse skullInstances;
        private static List<Gear> savedInstances;
        private static readonly List<Gear> emptyList = new();

        public static void Prefix(GearManager __instance)
        {
            if (!ModConfig.allowDuplicateSkulls)
                return;

            skullInstances = Traverse.Create(__instance).Field("_weaponInstances");
            savedInstances = skullInstances.GetValue() as List<Gear>;
            skullInstances.SetValue(emptyList);
        }

        public static void Postfix()
        {
            if (!ModConfig.allowDuplicateSkulls)
                return;

            skullInstances.SetValue(savedInstances);
        }
    }
}
