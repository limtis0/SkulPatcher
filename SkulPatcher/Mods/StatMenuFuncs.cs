using Characters;
using SkulPatcher.Mods.SpecialStats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkulPatcher
{
    public static class StatMenuFuncs
    {
        private static Stat.Values prevAttachedStats;
        private static readonly List<SpecialStat> prevAttachedSpecialStats = new();

        /*
        / Stat.Category.Fixed - Add fixed amount (1/1);
        / Stat.Category.Percent - Multiply by percentage (1/100);
        / Stat.Category.PercentPoint - Add perecent point (1/100);
        */
        public static readonly (Stat.Category category, Stat.Kind kind, string name)[] stats;

        private static readonly Dictionary<Stat.Kind, Type> specialStats;

        public static readonly Dictionary<Stat.Category, (double minValue, double maxValue, double defaultValue, string abbreviation)> statLimitInfo;

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
            statLimitInfo.Add(Stat.Category.Fixed, (-1000, 5000, 0, "p"));
            statLimitInfo.Add(Stat.Category.Percent, (-100, 1000, 100, "%"));
            statLimitInfo.Add(Stat.Category.PercentPoint, (-1000, 5000, 0, "%p"));

            // Stat list
            stats = new[]
            {
                (Stat.Category.Fixed, Stat.Kind.Health, "[General] Health"),
                (Stat.Category.Percent, Stat.Kind.Health, "[General] Health"),

                (Stat.Category.Percent, Stat.Kind.AttackDamage, "[General] AttackDamage"),
                (Stat.Category.PercentPoint, Stat.Kind.AttackDamage, "[General] AttackDamage"),

                (Stat.Category.Percent, Stat.Kind.PhysicalAttackDamage, "[General] PhysicalAttackDamage"),
                (Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, "[General] PhysicalAttackDamage"),

                (Stat.Category.Percent, Stat.Kind.MagicAttackDamage, "[General] MagicAttackDamage"),
                (Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, "[General] MagicAttackDamage"),

                (Stat.Category.Percent, Stat.Kind.TakingDamage, "[General] TakingDamage"),
                (Stat.Category.PercentPoint, Stat.Kind.TakingDamage, "[General] TakingDamage"),

                (Stat.Category.Percent, Stat.Kind.TakingPhysicalDamage, "[General] TakingPhysicalDamage"),
                (Stat.Category.PercentPoint, Stat.Kind.TakingPhysicalDamage, "[General] TakingPhysicalDamage"),

                (Stat.Category.Percent, Stat.Kind.TakingMagicDamage, "[General] TakingMagicDamage"),
                (Stat.Category.PercentPoint, Stat.Kind.TakingMagicDamage, "[General] TakingMagicDamage"),

                (Stat.Category.Percent, Stat.Kind.AttackSpeed, "[General] AttackSpeed"),
                (Stat.Category.PercentPoint, Stat.Kind.AttackSpeed, "[General] AttackSpeed"),

                (Stat.Category.Percent, Stat.Kind.MovementSpeed, "[General] MovementSpeed"),
                (Stat.Category.PercentPoint, Stat.Kind.MovementSpeed, "[General] MovementSpeed"),

                (Stat.Category.Percent, Stat.Kind.CriticalChance, "[General] CriticalChance"),
                (Stat.Category.PercentPoint, Stat.Kind.CriticalChance, "[General] CriticalChance"),

                (Stat.Category.Percent, Stat.Kind.CriticalDamage, "[General] CriticalDamage"),
                (Stat.Category.PercentPoint, Stat.Kind.CriticalDamage, "[General] CriticalDamage"),

                (Stat.Category.Fixed, Stat.Kind.BasicAttackDamage, "[General] BasicAttackDamage"),
                (Stat.Category.Percent, Stat.Kind.BasicAttackDamage, "[General] BasicAttackDamage"),

                (Stat.Category.Fixed, Stat.Kind.SkillAttackDamage, "[General] SkillAttackDamage"),
                (Stat.Category.Percent, Stat.Kind.SkillAttackDamage, "[General] SkillAttackDamage"),

                (Stat.Category.Percent, Stat.Kind.CooldownSpeed, "[General] CooldownSpeed"),
                (Stat.Category.PercentPoint, Stat.Kind.CooldownSpeed, "[General] CooldownSpeed"),

                (Stat.Category.Percent, Stat.Kind.SkillCooldownSpeed, "[General] SkillCooldownSpeed"),
                (Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, "[General] SkillCooldownSpeed"),

                (Stat.Category.Percent, Stat.Kind.DashCooldownSpeed, "[General] DashCooldownSpeed"),
                (Stat.Category.PercentPoint, Stat.Kind.DashCooldownSpeed, "[General] DashCooldownSpeed"),

                (Stat.Category.Percent, Stat.Kind.SwapCooldownSpeed, "[General] SwapCooldownSpeed"),
                (Stat.Category.PercentPoint, Stat.Kind.SwapCooldownSpeed, "[General] SwapCooldownSpeed"),

                (Stat.Category.Percent, Stat.Kind.EssenceCooldownSpeed, "[General] EssenceCooldownSpeed"),
                (Stat.Category.PercentPoint, Stat.Kind.EssenceCooldownSpeed, "[General] EssenceCooldownSpeed"),

                (Stat.Category.Percent, Stat.Kind.BuffDuration, "[General] BuffDuration"),
                (Stat.Category.PercentPoint, Stat.Kind.BuffDuration, "[General] BuffDuration"),

                (Stat.Category.Percent, Stat.Kind.BasicAttackSpeed, "[General] BasicAttackSpeed"),
                (Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, "[General] BasicAttackSpeed"),

                (Stat.Category.Percent, Stat.Kind.SkillAttackSpeed, "[General] SkillAttackSpeed"),
                (Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, "[General] SkillAttackSpeed"),

                (Stat.Category.Percent, Stat.Kind.CharacterSize, "[General] CharacterSize"),
                (Stat.Category.PercentPoint, Stat.Kind.CharacterSize, "[General] CharacterSize"),

                (Stat.Category.Percent, Stat.Kind.DashDistance, "[General] DashDistance"),
                (Stat.Category.PercentPoint, Stat.Kind.DashDistance, "[General] DashDistance"),

                (Stat.Category.Percent, Stat.Kind.PoisonTickFrequency, "[General] PoisonTickFrequency"),  // Can't tell if does anything
                (Stat.Category.PercentPoint, Stat.Kind.PoisonTickFrequency, "[General] PoisonTickFrequency"),  // Can't tell if does anything

                (Stat.Category.Percent, Stat.Kind.BleedDamage, "[General] BleedDamage"),
                (Stat.Category.PercentPoint, Stat.Kind.BleedDamage, "[General] BleedDamage"),

                (Stat.Category.Percent, Stat.Kind.EmberDamage, "[General] EmberDamage"),
                (Stat.Category.PercentPoint, Stat.Kind.EmberDamage, "[General] EmberDamage"),

                (Stat.Category.Percent, Stat.Kind.FreezeDuration, "[General] FreezeDuration"),
                (Stat.Category.PercentPoint, Stat.Kind.FreezeDuration, "[General] FreezeDuration"),

                (Stat.Category.Percent, Stat.Kind.StunDuration, "[General] StunDuration"),
                (Stat.Category.PercentPoint, Stat.Kind.StunDuration, "[General] StunDuration"),

                (Stat.Category.Percent, Stat.Kind.SpiritAttackCooldownSpeed, "[General] SpiritAttackCooldownSpeed"),
                (Stat.Category.PercentPoint, Stat.Kind.SpiritAttackCooldownSpeed, "[General] SpiritAttackCooldownSpeed"),

                (Stat.Category.Percent, Stat.Kind.ProjectileAttackDamage, "[General] ProjectileAttackDamage"),
                (Stat.Category.PercentPoint, Stat.Kind.ProjectileAttackDamage, "[General] ProjectileAttackDamage"),

                (Stat.Category.Percent, Stat.Kind.TakingHealAmount, "[General] TakingHealAmount"),
                (Stat.Category.PercentPoint, Stat.Kind.TakingHealAmount, "[General] TakingHealAmount"),

                (Stat.Category.Percent, Stat.Kind.ChargingSpeed, "[General] ChargingSpeed"),  // Can't tell if does anything
                (Stat.Category.PercentPoint, Stat.Kind.ChargingSpeed, "[General] ChargingSpeed"),  // Can't tell if does anything
            };

            // Special Stats in alphabetic order
            stats = stats.Concat(specialStatsEnumerable.OrderBy(stat => stat.Title).Select(stat => (stat.Category, stat.Kind, stat.Title))).ToArray();
        }

        public static void SetStats((bool toApply, double statValue)[] statValuesToApply)  // Function for StatMenu.cs
        {
            if (!ModConfig.IsInGame)
                return;

            List<Stat.Value> statValues = new();
            ResetSpecialStats();
            
            for (int i = 0; i < stats.Length; i++)
            {
                if (statValuesToApply[i].toApply)
                {
                    Stat.Category category = stats[i].category;
                    Stat.Kind kind = stats[i].kind;
                    double statValue = ScaleValueForCategory(category, statValuesToApply[i].statValue);

                    if (Stat.Kind.values.Contains(kind))
                    {
                        statValues.Add(new Stat.Value(category, kind, statValue));
                    }
                    else
                    {
                        SetSpecialStat(kind, statValue);
                    }
                }    
            }

            SetGameStats(new Stat.Values(statValues.ToArray()));
        }

        private static void SetGameStats(Stat.Values values)
        {
            if (prevAttachedStats is not null)
                Detach(prevAttachedStats);

            prevAttachedStats = values;
            Attach(values);
        }

        private static void SetSpecialStat(Stat.Kind kind, double value)
        {
            SpecialStat specialStat = (SpecialStat)Activator.CreateInstance(specialStats[kind], new object[] { value });

            prevAttachedSpecialStats.Add(specialStat);
            specialStat.Attach();
        }

        private static void ResetSpecialStats()
        {
            foreach (SpecialStat specialStat in prevAttachedSpecialStats)
            {
                specialStat.Detach();
            }

            prevAttachedSpecialStats.Clear();
        }

        private static readonly Stat.Category[] divideBy100 = new[] 
        {
            Stat.Category.PercentPoint,
            Stat.Category.Percent,
        };

        private static double ScaleValueForCategory(Stat.Category category, double value)
        {
            if (divideBy100.Contains(category))
                return value / 100;

            return value;
        }

        private static void Attach(Stat.Values values) => ModConfig.Level.player.stat.AttachValues(values);

        private static void Detach(Stat.Values values) => ModConfig.Level.player.stat.DetachValues(values);
    }
}