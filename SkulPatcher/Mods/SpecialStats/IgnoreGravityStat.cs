using Characters;
using HarmonyLib;
using System;

namespace SkulPatcher
{
    public class IgnoreGravityStat : SpecialStat
    {
        private const bool defaultValue = false;

        public static readonly Stat.Kind kind = CreateKind("IgnoreGravity");
        public static readonly Stat.Category category = CreateCategory("IgnoreGravity");

        public IgnoreGravityStat(double value) : base(value)
        {
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override void Attach()
        {
            new Traverse(ModConfig.Level.player.movement.config).Field("ignoreGravity").SetValue(Value != 0);
        }

        public override void Detach()
        {
            if (ModConfig.Level.player is null)
                return;

            new Traverse(ModConfig.Level.player.movement.config).Field("ignoreGravity").SetValue(defaultValue);
        }
    }
}
