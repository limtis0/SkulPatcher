using Characters;
using Characters.Abilities;
using Hardmode.Darktech;
using Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkulPatcher
{
    public static class StatFuncs
    {
        private static Stat.Values prevAttached;

        // Stat.Category.Fixed - Add fixed amount (1/1);
        // Stat.Category.Constant - Same as .Fixed;
        // Stat.Category.Final - Not working;
        // Stat.Category.Percent - Multiply by percentage (1/100);
        // Stat.Category.PercentPoint - Add perecent point (1/100);
        public static readonly (Stat.Category category, Stat.Kind kind, string name)[] stats = new[]
        {
             (Stat.Category.Fixed, Stat.Kind.Health, "Health"),
             (Stat.Category.Final, Stat.Kind.Health, "Health"),
             (Stat.Category.Constant, Stat.Kind.Health, "Health"),
             (Stat.Category.Percent, Stat.Kind.Health, "Health"),
             (Stat.Category.PercentPoint, Stat.Kind.Health, "Health"),
             (Stat.Category.PercentPoint, Stat.Kind.AttackDamage, "AttackDamage"),
             (Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, "PhysicalAttackDamage"),
             (Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, "MagicAttackDamage"),
             // (Stat.Kind.TakingDamage, "TakingDamage"),  // Doesn't seem to do anything
             // (Stat.Kind.TakingPhysicalDamage, "TakingPhysicalDamage"),  // Doesn't seem to do anything
             // (Stat.Kind.TakingMagicDamage, "TakingMagicDamage"),  // Doesn't seem to do anything
             (Stat.Category.PercentPoint, Stat.Kind.AttackSpeed, "AttackSpeed"),
             (Stat.Category.PercentPoint, Stat.Kind.MovementSpeed, "MovementSpeed"),
             (Stat.Category.PercentPoint, Stat.Kind.CriticalChance, "CriticalChance"),
             (Stat.Category.PercentPoint, Stat.Kind.CriticalDamage, "CriticalDamage"),
             // (Stat.Kind.EvasionChance, "EvasionChance"),  // Doesn't seem to do anything
             // (Stat.Kind.MeleeEvasionChance, "MeleeEvasionChance"),  // Doesn't seem to do anything
             // (Stat.Kind.RangedEvasionChance, "RangedEvasionChance"),  // Doesn't seem to do anything
             // (Stat.Kind.ProjectileEvasionChance, "ProjectileEvasionChance"),  // Doesn't seem to do anything
             // (Stat.Kind.StoppingResistance, "StoppingResistance"),  // Doesn't seem to do anything
             // (Stat.Kind.KnockbackResistance, "KnockbackResistance"),  // Doesn't seem to do anything
             // (Stat.Kind.StatusResistance, "StatusResistance"),  // Doesn't seem to do anything
             // (Stat.Kind.StoppingPower, "StoppingPower"),  // Doesn't seem to do anything
             (Stat.Category.PercentPoint, Stat.Kind.BasicAttackDamage, "BasicAttackDamage"),
             (Stat.Category.PercentPoint, Stat.Kind.SkillAttackDamage, "SkillAttackDamage"),
             (Stat.Category.PercentPoint, Stat.Kind.CooldownSpeed, "CooldownSpeed"),
             (Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, "SkillCooldownSpeed"),
             (Stat.Category.PercentPoint, Stat.Kind.DashCooldownSpeed, "DashCooldownSpeed"),
             (Stat.Category.PercentPoint, Stat.Kind.SwapCooldownSpeed, "SwapCooldownSpeed"),
             (Stat.Category.PercentPoint, Stat.Kind.EssenceCooldownSpeed, "EssenceCooldownSpeed"),
             (Stat.Category.PercentPoint, Stat.Kind.BuffDuration, "BuffDuration"),
             (Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, "BasicAttackSpeed"),
             (Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, "SkillAttackSpeed"),
             (Stat.Category.PercentPoint, Stat.Kind.CharacterSize, "CharacterSize"),
             (Stat.Category.PercentPoint, Stat.Kind.DashDistance, "DashDistance"),
             // (Stat.Kind.StunResistance, "StunResistance"),  // Doesn't seem to do anything
             // (Stat.Kind.FreezeResistance, "FreezeResistance"),  // Doesn't seem to do anything
             // (Stat.Kind.BurnResistance, "BurnResistance"),  // Doesn't seem to do anything
             // (Stat.Kind.BleedResistance, "BleedResistance"),  // Doesn't seem to do anything
             // (Stat.Kind.PoisonResistance, "PoisonResistance"),  // Doesn't seem to do anything
             (Stat.Category.PercentPoint, Stat.Kind.PoisonTickFrequency, "PoisonTickFrequency"),  // Can't tell if does anything
             (Stat.Category.PercentPoint, Stat.Kind.BleedDamage, "BleedDamage"),
             (Stat.Category.PercentPoint, Stat.Kind.EmberDamage, "EmberDamage"),
             (Stat.Category.PercentPoint, Stat.Kind.FreezeDuration, "FreezeDuration"),
             (Stat.Category.PercentPoint, Stat.Kind.StunDuration, "StunDuration"),
             (Stat.Category.PercentPoint, Stat.Kind.SpiritAttackCooldownSpeed, "SpiritAttackCooldownSpeed"),
             (Stat.Category.PercentPoint, Stat.Kind.ProjectileAttackDamage, "ProjectileAttackDamage"),
             (Stat.Category.PercentPoint, Stat.Kind.TakingHealAmount, "TakingHealAmount"),
             // (Stat.Kind.ChargingSpeed, "ChargingSpeed"),  // Doesn't seem to do anything
             (Stat.Category.PercentPoint, Stat.Kind.IgnoreDamageReduction, "IgnoreDamageReduction"),  // Can't tell if does anything
        };

        public static void SetBuff(bool[] toggles, int[] values)
        {
            if (toggles.Length != stats.Length || values.Length != stats.Length)
                throw new Exception("LengthError");
            
            List<Stat.Value> statValues = new();
            
            for (int i = 0; i < stats.Length; i++)
            {
                if (toggles[i])
                    statValues.Add(new Stat.Value(stats[i].category, stats[i].kind, values[i]));
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

        private static void Attach(Stat.Values values) => ModConfig.Level.player.stat.AttachValues(values);

        private static void Detach(Stat.Values values) => ModConfig.Level.player.stat.DetachValues(values);
    }
}