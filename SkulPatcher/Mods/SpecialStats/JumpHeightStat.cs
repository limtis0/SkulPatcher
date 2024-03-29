﻿using Characters;
using Characters.Movements;
using HarmonyLib;
namespace SkulPatcher.Mods.SpecialStats
{
    public class JumpHeightStat : SpecialStat
    {
        public static readonly Stat.Kind kind = CreateKind("JumpHeight");
        public static readonly Stat.Category category = CreateCategory("JumpHeight");

        public override double Value { get => base.Value; set => base.Value = value / 100; }

        public JumpHeightStat(double value) : base(value)
        {
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override string Title => "[Movement] JumpHeight";

        public override double MinValue => 0;

        public override double MaxValue => 750;

        public override double DefaultValue => 100;

        public override string Abbreviation => "%";

        public override void Attach()
        {
            JumpHeightPatch.enabled = true;
            JumpHeightPatch.multiplier = (float)Value;
        }

        public override void Detach()
        {
            JumpHeightPatch.enabled = false;
        }

        [HarmonyPatch(typeof(Movement), nameof(Movement.Jump))]
        private static class JumpHeightPatch
        {
            public static bool enabled = false;
            public static float multiplier;

            private static void Prefix(ref float jumpHeight, Movement __instance)
            {
                if (enabled && __instance == ModConfig.Level.player.movement)
                {
                    jumpHeight *= multiplier;
                }
            }
        }
    }
}
