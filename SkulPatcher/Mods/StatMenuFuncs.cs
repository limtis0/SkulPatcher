using Characters;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UI.Inventory;

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
        public static readonly (Stat.Category category, Stat.Kind kind, string name)[] stats = new[]
        {
            // Game stats
            (Stat.Category.Fixed, Stat.Kind.Health, "Health"),
            (Stat.Category.Percent, Stat.Kind.Health, "Health"),

            (Stat.Category.Percent, Stat.Kind.AttackDamage, "AttackDamage"),
            (Stat.Category.PercentPoint, Stat.Kind.AttackDamage, "AttackDamage"),

            (Stat.Category.Percent, Stat.Kind.PhysicalAttackDamage, "PhysicalAttackDamage"),
            (Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, "PhysicalAttackDamage"),

            (Stat.Category.Percent, Stat.Kind.MagicAttackDamage, "MagicAttackDamage"),
            (Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, "MagicAttackDamage"),

            (Stat.Category.Percent, Stat.Kind.TakingDamage, "TakingDamage"),
            (Stat.Category.PercentPoint, Stat.Kind.TakingDamage, "TakingDamage"),

            (Stat.Category.Percent, Stat.Kind.TakingPhysicalDamage, "TakingPhysicalDamage"),
            (Stat.Category.PercentPoint, Stat.Kind.TakingPhysicalDamage, "TakingPhysicalDamage"),

            (Stat.Category.Percent, Stat.Kind.TakingMagicDamage, "TakingMagicDamage"),
            (Stat.Category.PercentPoint, Stat.Kind.TakingMagicDamage, "TakingMagicDamage"),

            (Stat.Category.Percent, Stat.Kind.AttackSpeed, "AttackSpeed"),
            (Stat.Category.PercentPoint, Stat.Kind.AttackSpeed, "AttackSpeed"),

            (Stat.Category.Percent, Stat.Kind.MovementSpeed, "MovementSpeed"),
            (Stat.Category.PercentPoint, Stat.Kind.MovementSpeed, "MovementSpeed"),

            (Stat.Category.Percent, Stat.Kind.CriticalChance, "CriticalChance"),
            (Stat.Category.PercentPoint, Stat.Kind.CriticalChance, "CriticalChance"),

            (Stat.Category.Percent, Stat.Kind.CriticalDamage, "CriticalDamage"),
            (Stat.Category.PercentPoint, Stat.Kind.CriticalDamage, "CriticalDamage"),

            (Stat.Category.Fixed, Stat.Kind.BasicAttackDamage, "BasicAttackDamage"),
            (Stat.Category.Percent, Stat.Kind.BasicAttackDamage, "BasicAttackDamage"),

            (Stat.Category.Fixed, Stat.Kind.SkillAttackDamage, "SkillAttackDamage"),
            (Stat.Category.Percent, Stat.Kind.SkillAttackDamage, "SkillAttackDamage"),

            (Stat.Category.Percent, Stat.Kind.CooldownSpeed, "CooldownSpeed"),
            (Stat.Category.PercentPoint, Stat.Kind.CooldownSpeed, "CooldownSpeed"),

            (Stat.Category.Percent, Stat.Kind.SkillCooldownSpeed, "SkillCooldownSpeed"),
            (Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, "SkillCooldownSpeed"),

            (Stat.Category.Percent, Stat.Kind.DashCooldownSpeed, "DashCooldownSpeed"),
            (Stat.Category.PercentPoint, Stat.Kind.DashCooldownSpeed, "DashCooldownSpeed"),

            (Stat.Category.Percent, Stat.Kind.SwapCooldownSpeed, "SwapCooldownSpeed"),
            (Stat.Category.PercentPoint, Stat.Kind.SwapCooldownSpeed, "SwapCooldownSpeed"),

            (Stat.Category.Percent, Stat.Kind.EssenceCooldownSpeed, "EssenceCooldownSpeed"),
            (Stat.Category.PercentPoint, Stat.Kind.EssenceCooldownSpeed, "EssenceCooldownSpeed"),

            (Stat.Category.Percent, Stat.Kind.BuffDuration, "BuffDuration"),
            (Stat.Category.PercentPoint, Stat.Kind.BuffDuration, "BuffDuration"),

            (Stat.Category.Percent, Stat.Kind.BasicAttackSpeed, "BasicAttackSpeed"),
            (Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, "BasicAttackSpeed"),

            (Stat.Category.Percent, Stat.Kind.SkillAttackSpeed, "SkillAttackSpeed"),
            (Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, "SkillAttackSpeed"),

            (Stat.Category.Percent, Stat.Kind.CharacterSize, "CharacterSize"),
            (Stat.Category.PercentPoint, Stat.Kind.CharacterSize, "CharacterSize"),

            (Stat.Category.Percent, Stat.Kind.DashDistance, "DashDistance"),
            (Stat.Category.PercentPoint, Stat.Kind.DashDistance, "DashDistance"),

            (Stat.Category.Percent, Stat.Kind.PoisonTickFrequency, "PoisonTickFrequency"),  // Can't tell if does anything
            (Stat.Category.PercentPoint, Stat.Kind.PoisonTickFrequency, "PoisonTickFrequency"),  // Can't tell if does anything

            (Stat.Category.Percent, Stat.Kind.BleedDamage, "BleedDamage"),
            (Stat.Category.PercentPoint, Stat.Kind.BleedDamage, "BleedDamage"),

            (Stat.Category.Percent, Stat.Kind.EmberDamage, "EmberDamage"),
            (Stat.Category.PercentPoint, Stat.Kind.EmberDamage, "EmberDamage"),

            (Stat.Category.Percent, Stat.Kind.FreezeDuration, "FreezeDuration"),
            (Stat.Category.PercentPoint, Stat.Kind.FreezeDuration, "FreezeDuration"),

            (Stat.Category.Percent, Stat.Kind.StunDuration, "StunDuration"),
            (Stat.Category.PercentPoint, Stat.Kind.StunDuration, "StunDuration"),

            (Stat.Category.Percent, Stat.Kind.SpiritAttackCooldownSpeed, "SpiritAttackCooldownSpeed"),
            (Stat.Category.PercentPoint, Stat.Kind.SpiritAttackCooldownSpeed, "SpiritAttackCooldownSpeed"),

            (Stat.Category.Percent, Stat.Kind.ProjectileAttackDamage, "ProjectileAttackDamage"),
            (Stat.Category.PercentPoint, Stat.Kind.ProjectileAttackDamage, "ProjectileAttackDamage"),

            (Stat.Category.Percent, Stat.Kind.TakingHealAmount, "TakingHealAmount"),
            (Stat.Category.PercentPoint, Stat.Kind.TakingHealAmount, "TakingHealAmount"),

            (Stat.Category.Percent, Stat.Kind.ChargingSpeed, "ChargingSpeed"),  // Can't tell if does anything
            (Stat.Category.PercentPoint, Stat.Kind.ChargingSpeed, "ChargingSpeed"),  // Can't tell if does anything

            // Special stats
            (GravityStat.category, GravityStat.kind, "CurrentSkullGravity"),
            (IgnoreGravityStat.category, IgnoreGravityStat.kind, "CurrentSkullIgnoreGravity"),
            (MaxFallSpeedStat.category, MaxFallSpeedStat.kind, "CurrentSkullMaxFallSpeed"),
            (AccelerationStat.category, AccelerationStat.kind, "CurrentSkullAcceleration"),
            (FrictionStat.category, FrictionStat.kind, "CurrentSkullFriction"),

            (KeepMovingStat.category, KeepMovingStat.kind, "CurrentSkullKeepMoving"),
            (IgnorePushStat.category, IgnorePushStat.kind, "CurrentSkullIgnorePush"),
        };

        private static readonly Dictionary<Stat.Kind, Type> specialStats = new()
        {
            { GravityStat.kind, typeof(GravityStat) },
            { IgnoreGravityStat.kind, typeof(IgnoreGravityStat) },
            { MaxFallSpeedStat.kind, typeof(MaxFallSpeedStat) },
            { AccelerationStat.kind, typeof(AccelerationStat) },
            { FrictionStat.kind, typeof(FrictionStat) },

            { KeepMovingStat.kind, typeof(KeepMovingStat) },
            { IgnorePushStat.kind, typeof(IgnorePushStat) },
        };

        public static readonly Dictionary<Stat.Category, (double minValue, double maxValue, double defaultValue, string abbreviation)> statLimitInfo = new()
        {
            // Game stats
            { Stat.Category.Fixed, (-1000, 5000, 0, "p") },
            { Stat.Category.Percent, (-100, 1000, 100, "%") },
            { Stat.Category.PercentPoint, (-1000, 5000, 0, "%p") },

            // Special stats
            { GravityStat.category, (0, 500, 100, "%") },
            { IgnoreGravityStat.category, (0, 1, 0, "≡") },
            { FrictionStat.category, (0, 500, 100, "%") },
            { AccelerationStat.category, (0, 500, 100, "%") },
            { MaxFallSpeedStat.category, (0, 200, 25, "≡") },

            { KeepMovingStat.category, (0, 1, 0, "≡") },
            { IgnorePushStat.category, (0, 1, 0, "≡") },
        };

        public static void SetStats((bool toApply, double statValue)[] statValuesToApply)  // Function for StatMenu.cs
        {
            if (!ModConfig.IsInGame)
                return;

            // Setup
            List<Stat.Value> statValues = new();
            ResetSpecialStats();
            
            for (int i = 0; i < stats.Length; i++)
            {
                if (statValuesToApply[i].toApply)
                {
                    Stat.Category category = stats[i].category;
                    Stat.Kind kind = stats[i].kind;
                    double statValue = ScaleValueForCategory(category, statValuesToApply[i].statValue);

                    // Game stats
                    if (Stat.Kind.values.Contains(kind))
                    {
                        statValues.Add(new Stat.Value(category, kind, statValue));
                    }
                    // Special stats
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
            // Game stats
            Stat.Category.PercentPoint,
            Stat.Category.Percent,

            // Special stats
            GravityStat.category,
            AccelerationStat.category,
            FrictionStat.category,
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