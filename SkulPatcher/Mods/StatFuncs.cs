using Characters;
using System;
using System.Collections.Generic;

namespace SkulPatcher
{
    public static class StatFuncs
    {
        private static Stat.Values prevAttached;

        /*
        / Stat.Category.Fixed - Add fixed amount (1/1);
        / Stat.Category.Constant - Same as .Fixed;
        / Stat.Category.Final - Not working;
        / Stat.Category.Percent - Multiply by percentage (1/100);
        / Stat.Category.PercentPoint - Add perecent point (1/100);
        */
        public static readonly (Stat.Category category, Stat.Kind kind, string name)[] stats = new[]
        {
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
        };

        public static readonly Dictionary<Stat.Category, (int minValue, int maxValue, int defaultValue, string abbreviation)> statConsts = new()
        {
            {Stat.Category.Fixed, (-1000, 5000, 0, "p") },
            {Stat.Category.Percent, (-100, 1000, 100, "%") },
            {Stat.Category.PercentPoint, (-1000, 5000, 0, "pp") },
        };

        public static void SetBuff((bool toApply, int statValue)[] values)  // Function specifically for StatMenu.cs
        {
            if (values.Length != stats.Length)
                throw new Exception("StatValues are not the same length as StatFuncs.stats");
            
            List<Stat.Value> statValues = new();
            
            for (int i = 0; i < stats.Length; i++)
            {
                if (values[i].toApply)
                    statValues.Add(new Stat.Value(stats[i].category, stats[i].kind, ScaleValueForCategory(stats[i].category, values[i].statValue)));
            }

            SetBuff(new Stat.Values(statValues.ToArray()));
        }

        public static void SetBuff(Stat.Values values)
        {
            if (!ModConfig.IsInGame)
                return;

            if (prevAttached is not null)
                Detach(prevAttached);

            Attach(values);
            prevAttached = values;
        }

        private static double ScaleValueForCategory(Stat.Category category, int value)
        {
            if (category == Stat.Category.PercentPoint || category == Stat.Category.Percent)
                return (double)value / 100;

            return value;
        }

        private static void Attach(Stat.Values values) => ModConfig.Level.player.stat.AttachValues(values);

        private static void Detach(Stat.Values values) => ModConfig.Level.player.stat.DetachValues(values);
    }
}