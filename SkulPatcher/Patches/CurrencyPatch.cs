using Data;
using HarmonyLib;
using System;

namespace SkulPatcher.Patches
{
    [HarmonyPatch(typeof(GameData.Currency), nameof(GameData.Currency.Earn), new Type[] { typeof(double) })]
    public static class CurrencyPatch
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
                    if (Config.goldMultOn)
                        amount *= Config.goldMult;
                    break;

                case dQuartzKey:
                    if (Config.dQuartzMultOn)
                        amount *= Config.dQuartzMult;
                    break;

                case boneKey:
                    if (Config.bonesMultOn)
                        amount *= Config.bonesMult;
                    break;

                case hQuartzKey:
                    if (Config.hQuartzMultOn)
                        amount *= Config.hQuartzMult;
                    break;
            }
        }
    }
}
