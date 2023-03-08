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

                (Category.Percent, Kind.AttackDamage, "[Combat] AttackDamage"),
                (Category.PercentPoint, Kind.AttackDamage, "[Combat] AttackDamage"),

                (Category.Percent, Kind.PhysicalAttackDamage, "[Combat] PhysicalAttackDamage"),
                (Category.PercentPoint, Kind.PhysicalAttackDamage, "[Combat] PhysicalAttackDamage"),

                (Category.Percent, Kind.MagicAttackDamage, "[Combat] MagicAttackDamage"),
                (Category.PercentPoint, Kind.MagicAttackDamage, "[Combat] MagicAttackDamage"),

                (Category.Percent, Kind.TakingDamage, "[Combat] TakingDamage"),
                (Category.PercentPoint, Kind.TakingDamage, "[Combat] TakingDamage"),

                (Category.Percent, Kind.TakingPhysicalDamage, "[Combat] TakingPhysicalDamage"),
                (Category.PercentPoint, Kind.TakingPhysicalDamage, "[Combat] TakingPhysicalDamage"),

                (Category.Percent, Kind.TakingMagicDamage, "[Combat] TakingMagicDamage"),
                (Category.PercentPoint, Kind.TakingMagicDamage, "[Combat] TakingMagicDamage"),

                (Category.Percent, Kind.AttackSpeed, "[Combat] AttackSpeed"),
                (Category.PercentPoint, Kind.AttackSpeed, "[Combat] AttackSpeed"),

                (Category.Percent, Kind.MovementSpeed, "[Movement] MovementSpeed"),
                (Category.PercentPoint, Kind.MovementSpeed, "[Movement] MovementSpeed"),

                (Category.Percent, Kind.CriticalChance, "[Combat] CriticalChance"),
                (Category.PercentPoint, Kind.CriticalChance, "[Combat] CriticalChance"),

                (Category.Percent, Kind.CriticalDamage, "[Combat] CriticalDamage"),
                (Category.PercentPoint, Kind.CriticalDamage, "[Combat] CriticalDamage"),

                (Category.Percent, Kind.BasicAttackDamage, "[Combat] BasicAttackDamage"),
                (Category.PercentPoint, Kind.BasicAttackDamage, "[Combat] BasicAttackDamage"),

                (Category.Percent, Kind.SkillAttackDamage, "[Combat] SkillAttackDamage"),
                (Category.PercentPoint, Kind.SkillAttackDamage, "[Combat] SkillAttackDamage"),

                (Category.Percent, Kind.CooldownSpeed, "[General] CooldownSpeed"),
                (Category.PercentPoint, Kind.CooldownSpeed, "[General] CooldownSpeed"),

                (Category.Percent, Kind.SkillCooldownSpeed, "[Combat] SkillCooldownSpeed"),
                (Category.PercentPoint, Kind.SkillCooldownSpeed, "[Combat] SkillCooldownSpeed"),

                (Category.Percent, Kind.DashCooldownSpeed, "[Movement] DashCooldownSpeed"),
                (Category.PercentPoint, Kind.DashCooldownSpeed, "[Movement] DashCooldownSpeed"),

                (Category.Percent, Kind.SwapCooldownSpeed, "[Combat] SwapCooldownSpeed"),
                (Category.PercentPoint, Kind.SwapCooldownSpeed, "[Combat] SwapCooldownSpeed"),

                (Category.Percent, Kind.EssenceCooldownSpeed, "[Combat] EssenceCooldownSpeed"),
                (Category.PercentPoint, Kind.EssenceCooldownSpeed, "[Combat] EssenceCooldownSpeed"),

                (Category.Percent, Kind.BuffDuration, "[General] BuffDuration"),
                (Category.PercentPoint, Kind.BuffDuration, "[General] BuffDuration"),

                (Category.Percent, Kind.BasicAttackSpeed, "[Combat] BasicAttackSpeed"),
                (Category.PercentPoint, Kind.BasicAttackSpeed, "[Combat] BasicAttackSpeed"),

                (Category.Percent, Kind.SkillAttackSpeed, "[Combat] SkillAttackSpeed"),
                (Category.PercentPoint, Kind.SkillAttackSpeed, "[Combat] SkillAttackSpeed"),

                (Category.Percent, Kind.CharacterSize, "[General] CharacterSize"),
                (Category.PercentPoint, Kind.CharacterSize, "[General] CharacterSize"),

                (Category.Percent, Kind.DashDistance, "[Movement] DashDistance"),
                (Category.PercentPoint, Kind.DashDistance, "[Movement] DashDistance"),

                (Category.Percent, Kind.PoisonTickFrequency, "[Combat] PoisonTickFrequency"),
                (Category.PercentPoint, Kind.PoisonTickFrequency, "[Combat] PoisonTickFrequency"),

                (Category.Percent, Kind.BleedDamage, "[Combat] BleedDamage"),
                (Category.PercentPoint, Kind.BleedDamage, "[Combat] BleedDamage"),

                (Category.Percent, Kind.EmberDamage, "[Combat] EmberDamage"),
                (Category.PercentPoint, Kind.EmberDamage, "[Combat] EmberDamage"),

                (Category.Percent, Kind.FreezeDuration, "[Combat] FreezeDuration"),
                (Category.PercentPoint, Kind.FreezeDuration, "[Combat] FreezeDuration"),

                (Category.Percent, Kind.StunDuration, "[Combat] StunDuration"),
                (Category.PercentPoint, Kind.StunDuration, "[Combat] StunDuration"),

                (Category.Percent, Kind.SpiritAttackCooldownSpeed, "[Combat] SpiritAttackCooldownSpeed"),
                (Category.PercentPoint, Kind.SpiritAttackCooldownSpeed, "[Combat] SpiritAttackCooldownSpeed"),

                (Category.Percent, Kind.ProjectileAttackDamage, "[Combat] ProjectileAttackDamage"),
                (Category.PercentPoint, Kind.ProjectileAttackDamage, "[Combat] ProjectileAttackDamage"),

                (Category.Percent, Kind.TakingHealAmount, "[General] TakingHealAmount"),
                (Category.PercentPoint, Kind.TakingHealAmount, "[General] TakingHealAmount"),

                (Category.Percent, Kind.ChargingSpeed, "[Combat] ChargingSpeed"),
                (Category.PercentPoint, Kind.ChargingSpeed, "[Combat] ChargingSpeed"),
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
                switch ((Chapter.Type)__instance._currentChapterIndex)
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