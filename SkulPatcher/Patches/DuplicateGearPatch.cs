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
        private static readonly List<Gear> emptyList = new();

        public static void Prefix(GearManager __instance, ref List<Gear> __state)
        {
            if (!ModConfig.allowDuplicateItems)
                return;

            __state = __instance._itemInstances;
            new Traverse(__instance).Field("_itemInstances").SetValue(emptyList);
        }

        public static void Postfix(GearManager __instance, List<Gear> __state)
        {
            if (!ModConfig.allowDuplicateItems)
                return;

            new Traverse(__instance).Field("_itemInstances").SetValue(__state);
        }
    }

    [HarmonyPatch(typeof(GearManager), nameof(GearManager.GetWeaponToTake), new Type[] { typeof(Random), typeof(Rarity) })]
    public static class DuplicateSkullsPatch
    {
        private static readonly List<Gear> emptyList = new();

        public static void Prefix(GearManager __instance, ref List<Gear> __state)
        {
            if (!ModConfig.allowDuplicateSkulls)
                return;

            __state = __instance._weaponInstances;
            new Traverse(__instance).Field("_weaponInstances").SetValue(emptyList);
        }

        public static void Postfix(GearManager __instance, List<Gear> __state)
        {
            if (!ModConfig.allowDuplicateSkulls)
                return;

            new Traverse(__instance).Field("_weaponInstances").SetValue(__state);
        }
    }
}
