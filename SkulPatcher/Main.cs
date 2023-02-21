using BepInEx;
using HarmonyLib;
using SkulPatcher.Patches;
using SkulPatcher.UI;
using System.Reflection;
using UnityEngine;

namespace SkulPatcher
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Main : BaseUnityPlugin
    {
        public void Awake()
        {
            ModConfig.harmony = new Harmony("com.limtis.SkulPatcher");
            ModConfig.harmony.PatchAll(Assembly.GetExecutingAssembly());

            BossRushPatch.PatchAll();

            ModConfig.menu = new GameObject();
            ModConfig.menu.AddComponent<Menu>();
            DontDestroyOnLoad(ModConfig.menu);
        }
    }
}
