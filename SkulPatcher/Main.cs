using BepInEx;
using HarmonyLib;
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
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

            GameObject menu = new();
            ModConfig.menu = menu.AddComponent<Menu>();
            DontDestroyOnLoad(menu);
        }
    }
}
