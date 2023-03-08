using Data;
using HarmonyLib;
using Level;
using System.Collections.Generic;
using System.Linq;

namespace SkulPatcher.Patches
{
    // Idea to make this was taken from https://github.com/Tobi-Mob/Skul.Mod/blob/master/README.md#future-ideas

    /// <summary>
    /// The system for generating a path is working in a strange manner:
    /// Firstly, the Chapter is loaded;
    /// 
    /// Each Chapter is divided into two Stages (StageInfo chapter.currentStage): the first one (stageIndex == 0) is loaded on initialization,
    /// the second one (stageIndex == 1) is loaded after you reach the end of the first Stage
    /// (Terminal room after fighting the Adventurers (Last element of currentStage._path));
    /// 
    /// The end of the second Stage is located in the Terminal room right before the Boss-fight;
    /// 
    /// Furthermore, the Boss-fight is not a part of a second Stage (but a part of a Path), so it must be called via chapter.NextStage();
    /// After the Boss-fight a new chapter is loaded and the procedure described above repeats.
    /// 
    /// Each stage has its Path randomly generated. Path is an array of (PathNode, PathNode) tuples.
    /// Each of the PathNodes represents a room the gate is leading to;
    /// 
    /// To load a room from Path, chapter.ChangeMap() method can be called;
    /// 
    /// This leads to a lot of edge-cases which need to be covered in order to provide a flexible system for changing the route;
    /// </summary>

    [HarmonyPatch]
    public static class BossRushPatch
    {
        private static bool newStage;
        private static bool newMap;

        private static (PathNode, PathNode)[] originalPath = new (PathNode, PathNode)[] { };
        private static readonly Queue<(int pathIndex, Gate.Type type)> pathQueue = new();

        private static readonly Gate.Type[] bossGates = new Gate.Type[] { Gate.Type.Adventurer, Gate.Type.Boss };

        private static bool IsInStartChapter => ModConfig.Level.currentChapter.type == Chapter.Type.Chapter1 || ModConfig.Level.currentChapter.type == Chapter.Type.HardmodeChapter1;

        private static bool IsInEndChapter => ModConfig.Level.currentChapter.type == Chapter.Type.Chapter5 || ModConfig.Level.currentChapter.type == Chapter.Type.HardmodeChapter5;

        private const int HealAmountAfterBossFight = 100;

        public static void Toggle()
        {
            if (ModConfig.bossRushOn)
            {
                BuildPathQueue();
                newMap = false;
                newStage = false;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(StageInfo), nameof(StageInfo.GeneratePath))]
        private static void OnNewStagePostfix(StageInfo __instance)
        {
            originalPath = __instance._path;

            if (!ModConfig.bossRushOn)
                return;

            BuildPathQueue();
        }

        private static void BuildPathQueue()
        {
            pathQueue.Clear();
            newStage = true;

            for (int pathIndex = 0; pathIndex < originalPath.Length; pathIndex++)
            {
                // We don't need to check both of the gates, as either they are equal or the second one is null
                Gate.Type gate = originalPath[pathIndex].Item1.gate;

                if (bossGates.Contains(gate))
                {
                    pathQueue.Enqueue((pathIndex, gate));
                    continue;
                }

                // Shop gate is represented as Gate.Type.Npc
                if (ModConfig.bossRushIncludeShops && gate == Gate.Type.Npc)
                {
                    pathQueue.Enqueue((pathIndex, gate));
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(LevelManager), nameof(LevelManager.InvokeOnMapChangedAndFadeIn))]
        private static void OnMapChangedPostfix()
        {
            if (!ModConfig.bossRushOn)
                return;

            // The last chapter has no Gate.Type.Boss room, so the boss must be called manually
            if (IsInEndChapter)
            {
                ModConfig.Level.LoadNextStage();
                return;
            }

            // For Castle/Tutorial/DarkAbilityRoom
            if (pathQueue.Count == 0)
                return;

            if (newStage)
            {
                newStage = false;
                newMap = false;

                // Skip the first map of the chapter, if arachne is not needed
                if (ModConfig.bossRushIncludeArachne && !IsInStartChapter)
                    return;
            }

            // If changed the map in previous call
            if (newMap)
            {
                newMap = false;
                return;
            }

            Chapter chapter = ModConfig.Level.currentChapter;

            int pathIndex;
            Gate.Type type;

            // While ahead of the generated path
            do (pathIndex, type) = pathQueue.Dequeue();
            while (chapter.currentStage.pathIndex > pathIndex && pathQueue.Count != 0);

            // If already moved to the next stage through the door
            if (pathIndex == chapter.currentStage.pathIndex)
                return;

            if (type == Gate.Type.Boss)
            {
                ModConfig.Level.LoadNextStage();
                return;
            }

            LoadMap(pathIndex);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(BossChest), nameof(BossChest.InteractWith))]
        [HarmonyPatch(typeof(LevelManager), nameof(LevelManager.InvokeOnActivateMapReward))]
        private static void OnMapRewardsPostfix()
        {
            if (!ModConfig.bossRushOn || !ModConfig.bossRushSkipRewards)
                return;

            // On boss-reward skip
            Gate.Type gate = ModConfig.Level.currentChapter.currentStage.currentMapPath.node1.gate;
            if (gate == Gate.Type.None)
            {
                ModConfig.Level.player.health.Heal(HealAmountAfterBossFight);
            }

            ModConfig.Level.LoadNextMap();
        }

        private static void LoadMap(int pathIndex)
        {
            newMap = true;

            Chapter chapter = ModConfig.Level.currentChapter;

            chapter.currentStage.pathIndex = pathIndex;
            chapter.ChangeMap(chapter.currentStage.currentMapPath.node1);
        }

        public static void LoadChapter(Chapter.Type chapterType)
        {
            bool convertToHardmode = false;

            if (GameData.HardmodeProgress.hardmode && (int)chapterType >= 3 && (int)chapterType <= 7)
                convertToHardmode = true;

            pathQueue.Clear();
            ModConfig.Level.Load(convertToHardmode ? chapterType + 6 : chapterType);
        }
    }
}
