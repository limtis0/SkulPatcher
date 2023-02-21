using HarmonyLib;
using Services;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SkulPatcher.Patches
{
    [HarmonyPatch]
    public static class BoostRarityPatch
    {
        public static void Prefix(Random random, ref Rarity rarity)
        {
            if (!ModConfig.luckBoostOn)
                return;

            if (ModConfig.luckBoostContinuous)
            {
                // Continue to roll for boost if rarity has been boosted successfully
                Rarity temp;
                do
                {
                    temp = rarity;
                    rarity = BoostRarityWithProbability(random, rarity, ModConfig.luckBoostPercent);
                }
                while (rarity != temp);
            }
            else
            {
                rarity = BoostRarityWithProbability(random, rarity, ModConfig.luckBoostPercent);
            }
        }

        public static Rarity BoostRarityWithProbability(Random random, Rarity rarity, int probability)
        {
            if (random.Next(1, 100) <= probability)
            {
                switch (rarity)
                {
                    case Rarity.Common:
                        return Rarity.Rare;

                    case Rarity.Rare:
                        return Rarity.Unique;

                    case Rarity.Unique:
                        return Rarity.Legendary;
                }
            }
            return rarity;
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
