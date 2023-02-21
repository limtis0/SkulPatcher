using Data;
using HarmonyLib;
using System;

namespace SkulPatcher.Patches
{
    [HarmonyPatch(typeof(GameData.Currency), nameof(GameData.Currency.Earn), new Type[] { typeof(double) })]
    public static class CurrencyMultPatch
    {
        private const string goldKey = "gold";
        private const string dQuartzKey = "darkQuartz";
        private const string boneKey = "bone";
        private const string hQuartzKey = "heartQuartz";

        public static void Prefix(GameData.Currency __instance, ref double amount)
        {
            // Get _key private property of Currency instance
            string key = Traverse.Create(__instance).Field("_key").GetValue() as string;

            switch (key)
            {
                case goldKey:
                    if (ModConfig.goldMultOn)
                        amount *= ModConfig.goldMultValue;
                    break;

                case dQuartzKey:
                    if (ModConfig.dQuartzMultOn)
                        amount *= ModConfig.dQuartzMultValue;
                    break;

                case boneKey:
                    if (ModConfig.bonesMultOn)
                        amount *= ModConfig.bonesMultValue;
                    break;

                case hQuartzKey:
                    if (ModConfig.hQuartzMultOn)
                        amount *= ModConfig.hQuartzMultValue;
                    break;
            }
        }
    }
}
