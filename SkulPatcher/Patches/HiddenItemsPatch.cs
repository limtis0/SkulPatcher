using GameResources;
using HarmonyLib;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SkulPatcher.Patches
{
    [HarmonyPatch]
    public class HiddenItemsPatch
    {
        private static readonly List<(string gearName, Rarity rarity)> hiddenItems = new()
        {
            ("LeoniasGrace", Rarity.Common),
        };

        private static readonly List<(string gearName, Rarity rarity)> hiddenSkulls = new()
        {
            ("PlagueDoctor", Rarity.Unique),
        };

        private static readonly List<(string gearName, Rarity rarity)> hiddenEssences = new()
        {
            ("Kiriz", Rarity.Legendary),
        };

        private static bool init = false;

        public static void Prefix(GearManager __instance)
        {
            EnumArray<Rarity, ItemReference[]> items = new Traverse(__instance).Field("_items").GetValue() as EnumArray<Rarity, ItemReference[]>;
            EnumArray<Rarity, WeaponReference[]> skulls = new Traverse(__instance).Field("_weapons").GetValue() as EnumArray<Rarity, WeaponReference[]>;
            EnumArray<Rarity, EssenceReference[]> essences = new Traverse(__instance).Field("_quintessences").GetValue() as EnumArray<Rarity, EssenceReference[]>;

            // Future-proof, if anything gets added to the game
            if (!init)
            {
                init = true;

                RemoveIfGloballyObtainable(hiddenItems, ModConfig.Gear.items);
                RemoveIfGloballyObtainable(hiddenSkulls, ModConfig.Gear.weapons);
                RemoveIfGloballyObtainable(hiddenEssences, ModConfig.Gear.essences);
            }

            if (ModConfig.allowHiddenGear != ModConfig.allowHiddenGearPreviousState)
            {
                ModConfig.allowHiddenGearPreviousState = ModConfig.allowHiddenGear;

                SetObtainability(hiddenItems, items, ModConfig.allowHiddenGear);
                SetObtainability(hiddenSkulls, skulls, ModConfig.allowHiddenGear);
                SetObtainability(hiddenEssences, essences, ModConfig.allowHiddenGear);
            }
        }

        private static void SetObtainability<T>(List<(string gearName, Rarity rarity)> gearNames,
                                                EnumArray<Rarity, T[]> gearByRarity,
                                                bool obtainability) where T : GearReference
        {
            foreach (var (gearName, rarity) in gearNames)
            {
                GearReference gearRef = gearByRarity[rarity].First(gearRef => gearRef.name == gearName);

                gearRef.obtainable = obtainability;
                gearRef.needUnlock = !obtainability;
            }
        }

        private static void RemoveIfGloballyObtainable<T>(List<(string gearName, Rarity rarity)> gearNames, IEnumerable<T> globalGearList) where T : GearReference
        {
            foreach ((string gearName, Rarity rarity) pair in gearNames.ToList())
            {
                T gearRef = globalGearList.Where(gearRef => gearRef.name == pair.gearName).FirstOrDefault();

                if (gearRef is null || (gearRef.obtainable && !gearRef.needUnlock))
                {
                    gearNames.Remove(pair);

                    UnityEngine.Debug.LogWarning($"{pair.gearName} either is already obtainable or has an invalid name");
                }
            }
        }

        public static IEnumerable<MethodBase> TargetMethods()
        {
            Type[] overloadSignature = new Type[] { typeof(Random), typeof(Rarity) };
            yield return AccessTools.Method(typeof(GearManager), nameof(GearManager.GetItemToTake), overloadSignature);
            yield return AccessTools.Method(typeof(GearManager), nameof(GearManager.GetWeaponToTake), overloadSignature);
            yield return AccessTools.Method(typeof(GearManager), nameof(GearManager.GetQuintessenceToTake), overloadSignature);
            yield return AccessTools.Method(typeof(GearManager), nameof(GearManager.GetWeaponByCategory));
        }
    }
}
