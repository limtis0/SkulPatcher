using HarmonyLib;
using SkulPatcher.Misc;
using System.Reflection;
using UnityEngine;

namespace SkulPatcher
{
    public static class Main
    {
        public static GameObject menu;
        public static Harmony harmony;

        public static void Init()
        {
            harmony = new Harmony("com.limtis.SkulPatcher");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            menu = new GameObject();
            menu.AddComponent<Menu>();
            Object.DontDestroyOnLoad(menu);

            TurboActions.Init();
        }
    }
}
