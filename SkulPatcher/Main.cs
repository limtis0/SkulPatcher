using BepInEx;
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
            ModConfig.harmony = new(PluginInfo.PLUGIN_GUID);
            ModConfig.harmony.PatchAll(Assembly.GetExecutingAssembly());

            BossRushPatch.PatchAll();

            GameObject menu = new();
            ModConfig.menu = menu.AddComponent<Menu>();
            DontDestroyOnLoad(menu);
        }
    }
}
