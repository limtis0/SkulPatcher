using HarmonyLib;
using Level;
using SkulPatcher.Mods.SpecialStats;
using SkulPatcher.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using static Characters.Stat;

namespace SkulPatcher
{
    public static class StatMenuFuncs
    {
        private static Values prevAttachedStats;
        private static readonly Dictionary<Kind, SpecialStat> prevAttachedSpecialStats = new();

        /*
        / Stat.Category.Fixed - Add fixed amount (1/1);
        / Stat.Category.Percent - Multiply by percentage (1/100);
        / Stat.Category.PercentPoint - Add perecent point (1/100);
        */
        public static readonly (Category category, Kind kind, string name)[] statList;

        private static readonly Dictionary<Kind, Type> specialStats;

        public static readonly Dictionary<Category, (double minValue, double maxValue, double defaultValue, string abbreviation)> statLimitInfo;

        static StatMenuFuncs()
        {
            // Instances of all Special Stats classes
            IEnumerable<SpecialStat> specialStatsEnumerable =
                typeof(SpecialStat).Assembly.GetTypes()  // From all types in assembly
                .Where(t => t.IsSubclassOf(typeof(SpecialStat)) && !t.IsAbstract)  // Get types that are subclass of SpecialStat
                .Select(t => (SpecialStat)Activator.CreateInstance(t, new object[] { 0 }));  // Get instances of those types

            // <Stat.Kind => Type> Dictionary for creating an instanace
            specialStats = specialStatsEnumerable.ToDictionary(stat => stat.Kind, stat => stat.GetType());

            // Slider limits and text (UI)
            statLimitInfo = specialStatsEnumerable.ToDictionary(stat => stat.Category, s => (s.MinValue, s.MaxValue, s.DefaultValue, s.Abbreviation));
            statLimitInfo.Add(Category.Fixed, (-1000, 5000, 0, "p"));
            statLimitInfo.Add(Category.Percent, (-100, 1000, 100, "%"));
            statLimitInfo.Add(Category.PercentPoint, (-1000, 5000, 0, "%p"));

            // Stat list
            statList = new[]
            {
                (Category.Fixed, Kind.Health, "[General] Health"),
                (Category.Percent, Kind.Health, "[General] Health"),

                (Category.Percent, Kind.AttackDamage, "[General] AttackDamage"),
                (Category.PercentPoint, Kind.AttackDamage, "[General] AttackDamage"),

                (Category.Percent, Kind.PhysicalAttackDamage, "[General] PhysicalAttackDamage"),
                (Category.PercentPoint, Kind.PhysicalAttackDamage, "[General] PhysicalAttackDamage"),

                (Category.Percent, Kind.MagicAttackDamage, "[General] MagicAttackDamage"),
                (Category.PercentPoint, Kind.MagicAttackDamage, "[General] MagicAttackDamage"),

                (Category.Percent, Kind.TakingDamage, "[General] TakingDamage"),
                (Category.PercentPoint, Kind.TakingDamage, "[General] TakingDamage"),

                (Category.Percent, Kind.TakingPhysicalDamage, "[General] TakingPhysicalDamage"),
                (Category.PercentPoint, Kind.TakingPhysicalDamage, "[General] TakingPhysicalDamage"),

                (Category.Percent, Kind.TakingMagicDamage, "[General] TakingMagicDamage"),
                (Category.PercentPoint, Kind.TakingMagicDamage, "[General] TakingMagicDamage"),

                (Category.Percent, Kind.AttackSpeed, "[General] AttackSpeed"),
                (Category.PercentPoint, Kind.AttackSpeed, "[General] AttackSpeed"),

                (Category.Percent, Kind.MovementSpeed, "[General] MovementSpeed"),
                (Category.PercentPoint, Kind.MovementSpeed, "[General] MovementSpeed"),

                (Category.Percent, Kind.CriticalChance, "[General] CriticalChance"),
                (Category.PercentPoint, Kind.CriticalChance, "[General] CriticalChance"),

                (Category.Percent, Kind.CriticalDamage, "[General] CriticalDamage"),
                (Category.PercentPoint, Kind.CriticalDamage, "[General] CriticalDamage"),

                (Category.Fixed, Kind.BasicAttackDamage, "[General] BasicAttackDamage"),
                (Category.Percent, Kind.BasicAttackDamage, "[General] BasicAttackDamage"),

                (Category.Fixed, Kind.SkillAttackDamage, "[General] SkillAttackDamage"),
                (Category.Percent, Kind.SkillAttackDamage, "[General] SkillAttackDamage"),

                (Category.Percent, Kind.CooldownSpeed, "[General] CooldownSpeed"),
                (Category.PercentPoint, Kind.CooldownSpeed, "[General] CooldownSpeed"),

                (Category.Percent, Kind.SkillCooldownSpeed, "[General] SkillCooldownSpeed"),
                (Category.PercentPoint, Kind.SkillCooldownSpeed, "[General] SkillCooldownSpeed"),

                (Category.Percent, Kind.DashCooldownSpeed, "[General] DashCooldownSpeed"),
                (Category.PercentPoint, Kind.DashCooldownSpeed, "[General] DashCooldownSpeed"),

                (Category.Percent, Kind.SwapCooldownSpeed, "[General] SwapCooldownSpeed"),
                (Category.PercentPoint, Kind.SwapCooldownSpeed, "[General] SwapCooldownSpeed"),

                (Category.Percent, Kind.EssenceCooldownSpeed, "[General] EssenceCooldownSpeed"),
                (Category.PercentPoint, Kind.EssenceCooldownSpeed, "[General] EssenceCooldownSpeed"),

                (Category.Percent, Kind.BuffDuration, "[General] BuffDuration"),
                (Category.PercentPoint, Kind.BuffDuration, "[General] BuffDuration"),

                (Category.Percent, Kind.BasicAttackSpeed, "[General] BasicAttackSpeed"),
                (Category.PercentPoint, Kind.BasicAttackSpeed, "[General] BasicAttackSpeed"),

                (Category.Percent, Kind.SkillAttackSpeed, "[General] SkillAttackSpeed"),
                (Category.PercentPoint, Kind.SkillAttackSpeed, "[General] SkillAttackSpeed"),

                (Category.Percent, Kind.CharacterSize, "[General] CharacterSize"),
                (Category.PercentPoint, Kind.CharacterSize, "[General] CharacterSize"),

                (Category.Percent, Kind.DashDistance, "[General] DashDistance"),
                (Category.PercentPoint, Kind.DashDistance, "[General] DashDistance"),

                (Category.Percent, Kind.PoisonTickFrequency, "[General] PoisonTickFrequency"),  // Can't tell if does anything
                (Category.PercentPoint, Kind.PoisonTickFrequency, "[General] PoisonTickFrequency"),  // Can't tell if does anything

                (Category.Percent, Kind.BleedDamage, "[General] BleedDamage"),
                (Category.PercentPoint, Kind.BleedDamage, "[General] BleedDamage"),

                (Category.Percent, Kind.EmberDamage, "[General] EmberDamage"),
                (Category.PercentPoint, Kind.EmberDamage, "[General] EmberDamage"),

                (Category.Percent, Kind.FreezeDuration, "[General] FreezeDuration"),
                (Category.PercentPoint, Kind.FreezeDuration, "[General] FreezeDuration"),

                (Category.Percent, Kind.StunDuration, "[General] StunDuration"),
                (Category.PercentPoint, Kind.StunDuration, "[General] StunDuration"),

                (Category.Percent, Kind.SpiritAttackCooldownSpeed, "[General] SpiritAttackCooldownSpeed"),
                (Category.PercentPoint, Kind.SpiritAttackCooldownSpeed, "[General] SpiritAttackCooldownSpeed"),

                (Category.Percent, Kind.ProjectileAttackDamage, "[General] ProjectileAttackDamage"),
                (Category.PercentPoint, Kind.ProjectileAttackDamage, "[General] ProjectileAttackDamage"),

                (Category.Percent, Kind.TakingHealAmount, "[General] TakingHealAmount"),
                (Category.PercentPoint, Kind.TakingHealAmount, "[General] TakingHealAmount"),

                (Category.Percent, Kind.ChargingSpeed, "[General] ChargingSpeed"),  // Can't tell if does anything
                (Category.PercentPoint, Kind.ChargingSpeed, "[General] ChargingSpeed"),  // Can't tell if does anything
            };

            // Special Stats in alphabetic order
            statList = statList.Concat(specialStatsEnumerable.OrderBy(stat => stat.Title).Select(stat => (stat.Category, stat.Kind, stat.Title))).ToArray();
        }

        public static void SetStats((bool toApply, double value)[] values)  // Function for StatMenu.cs
        {
            if (!ModConfig.IsInGame)
                return;

            List<Value> statValues = new();

            ResetStats();

            for (int i = 0; i < statList.Length; i++)
            {
                if (values[i].toApply)
                {
                    Category category = statList[i].category;
                    Kind kind = statList[i].kind;
                    double statValue = ScaleValueForCategory(category, values[i].value);

                    // If one of in-game stats
                    if (Kind.values.Contains(kind))
                    {
                        statValues.Add(new Value(category, kind, statValue));
                    }
                    // Special stats
                    else
                    {
                        SetSpecialStatValue(kind, statValue);
                    }
                }
            }

            SetInGameStats(new Values(statValues.ToArray()));
        }

        private static double ScaleValueForCategory(Category category, double value)
        {
            if (category == Category.PercentPoint || category == Category.Percent)
                return value / 100;

            return value;
        }


        public static void SetSpecialStat(Kind kind, bool toApplyUI, double value)
        {
            int statIndex = statList.Select((stat, index) => (stat, index)).First(v => v.stat.kind == kind).index;

            StatMenu.SetStatValue(statIndex, toApplyUI, value);
            SetSpecialStatValue(kind, value);
        }

        private static void SetInGameStats(Values values)
        {
            prevAttachedStats = values;
            ModConfig.Level.player.stat.AttachValues(values);
        }

        private static void SetSpecialStatValue(Kind kind, double value)
        {
            prevAttachedSpecialStats.TryGetValue(kind, out SpecialStat stat);

            if (stat is null)
            {
                CreateSpecialStat(kind, value);
            }
            else
            {
                stat.Value = value;
            }
        }

        private static void CreateSpecialStat(Kind kind, double value)
        {
            SpecialStat specialStat = (SpecialStat)Activator.CreateInstance(specialStats[kind], new object[] { value });

            prevAttachedSpecialStats.Add(kind, specialStat);
            specialStat.Attach();
        }

        private static void ResetStats()
        {
            if (prevAttachedStats is not null)
                ModConfig.Level.player.stat.DetachValues(prevAttachedStats);

            foreach (KeyValuePair<Kind, SpecialStat> specialStat in prevAttachedSpecialStats)
            {
                specialStat.Value.Detach();
            }

            prevAttachedSpecialStats.Clear();
        }

        [HarmonyPatch(typeof(LevelManager), nameof(LevelManager.InvokeOnMapChangedAndFadeIn))]
        private static class ResetStatsOnCastle
        {
            public static void Postfix(LevelManager __instance)
            {
                switch ((Chapter.Type)new Traverse(__instance).Field("_currentChapterIndex").GetValue())
                {
                    case Chapter.Type.Castle:
                    case Chapter.Type.HardmodeCastle:
                        ResetStats();
                        break;
                }
            }
        }
    }
}