using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace SkulPatcher
{
    public static class Main
    {
        public static GameObject cheatObject;
        public static Harmony harmony;

        public static void Init()
        {
            harmony = new Harmony("com.limtis.SkulPatcher");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            cheatObject = new GameObject();
            cheatObject.AddComponent<Menu>();
            Object.DontDestroyOnLoad(cheatObject);
        }
    }
}
