using HarmonyLib;
using SkulPatcher.Patches;
using SkulPatcher.UI;
using System.Reflection;
using UnityEngine;

namespace SkulPatcher
{
    public static class Main
    {
        public static void Init()
        {
            Config.harmony = new Harmony("com.limtis.SkulPatcher");
            Config.harmony.PatchAll(Assembly.GetExecutingAssembly());

            BossRushPatch.PatchAll();

            Config.menu = new GameObject();
            Config.menu.AddComponent<Menu>();
            Object.DontDestroyOnLoad(Config.menu);
        }
    }
}
