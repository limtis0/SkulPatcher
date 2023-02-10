using Characters.Abilities;
using Characters.Player;
using HarmonyLib;
using System.Reflection;

namespace SkulPatcher.Patches
{
    [HarmonyPatch]
    public static class EasyPatch
    {
        public static bool Prefix(PlayerEasyModeBuff __instance)
        {
            if (!Config.forceEasyModeOn)
                return true;

            Traverse traverse = Traverse.Create(__instance);
            bool? attached = traverse.Field("_attached").GetValue() as bool?;
            if (attached is false)
            {
                // Attach ability
                (traverse.Field("_abilityAttacher").GetValue() as AbilityAttacher).StartAttach();
                traverse.Field("_attached").SetValue(true);
            }
            return false;  // Skip original method
        }

        public static MethodBase TargetMethod() => AccessTools.Method(typeof(PlayerEasyModeBuff), "Update");
    }
}
