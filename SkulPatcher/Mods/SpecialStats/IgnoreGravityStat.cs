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

        public IgnoreGravityStat(Character owner, double value) : base(owner, value)
        {
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override void Attach()
        {
            new Traverse(Owner.movement.config).Field("ignoreGravity").SetValue(Value != 0);
        }

        public override void Detach()
        {
            if (Owner is null)
                return;

            new Traverse(Owner.movement.config).Field("ignoreGravity").SetValue(defaultValue);
        }
    }
}
