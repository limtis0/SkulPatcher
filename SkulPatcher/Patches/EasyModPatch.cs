using Characters.Player;
using HarmonyLib;
using System.Reflection;

namespace SkulPatcher.Patches
{
    [HarmonyPatch]
    public static class EasyModPatch
    {
        public static bool Prefix(PlayerEasyModeBuff __instance)
        {
            if (!ModConfig.forceEasyModeOn)
                return true;

            if (!__instance._attached)
            {
                __instance._abilityAttacher.StartAttach();
                __instance._attached = true;
            }
            return false;
        }

        public static MethodBase TargetMethod() => AccessTools.Method(typeof(PlayerEasyModeBuff), "Update");
    }
}
