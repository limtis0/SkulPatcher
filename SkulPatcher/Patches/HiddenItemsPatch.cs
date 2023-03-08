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
            ("MagicalWand", Rarity.Rare),
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

                SetObtainability(hiddenItems, __instance._items, ModConfig.allowHiddenGear);
                SetObtainability(hiddenSkulls, __instance._weapons, ModConfig.allowHiddenGear);
                SetObtainability(hiddenEssences, __instance._quintessences, ModConfig.allowHiddenGear);
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
