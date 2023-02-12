using HarmonyLib;
using Level;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SkulPatcher.Patches
{
    // Idea taken from https://github.com/Tobi-Mob/Skul.Mod/blob/master/README.md#future-ideas

    [HarmonyPatch]
    public static class GeneratedPath
    {
        public static int bossRoomIndex;
        public static (PathNode, PathNode)[] bossRoomNodes;
        public static (PathNode, PathNode) bossRoomNodes2;

        public static bool newPathGenerated;

        public static void Postfix(StageInfo __instance)
        {
            if (!Config.bossRushOn)
                return;

            bossRoomIndex = __instance._path.Length - 2;
            
            bossRoomNodes = new (PathNode, PathNode)[2];
            bossRoomNodes[0] = __instance._path[bossRoomIndex];
            if (bossRoomIndex != 0)
                bossRoomNodes[1] = __instance._path[bossRoomIndex - 1];

            newPathGenerated = true;
        }

        public static MethodBase TargetMethod() => AccessTools.Method(typeof(StageInfo), "GeneratePath");
    }

    [HarmonyPatch(typeof(LevelManager), nameof(LevelManager.InvokeOnMapChangedAndFadeIn))]
    public static class BossRushPatch
    {
        public static void Postfix()
        {
            if (!Config.bossRushOn)
                return;

            if (!GeneratedPath.newPathGenerated)
                return;

            Chapter chapter = Config.level.currentChapter;
            
            switch (chapter.type)
            {
                case Chapter.Type.Test:
                case Chapter.Type.Castle:
                case Chapter.Type.HardmodeCastle:
                    return;
            }

            if (GeneratedPath.bossRoomNodes.Contains(chapter.currentStage.currentMapPath))
                return;

            MethodInfo m = typeof(Chapter).GetMethod("LoadStage", BindingFlags.NonPublic | BindingFlags.Instance);
            m.Invoke(Config.level.currentChapter, new object[] { GeneratedPath.bossRoomIndex, 0 });

            GeneratedPath.newPathGenerated = false;
        }
    }
}
