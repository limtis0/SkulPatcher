using Characters;
using HarmonyLib;
using System;

namespace SkulPatcher
{
    public class KeepMovingStat : SpecialStat
    {
        private const bool defaultValue = false;

        public static readonly Stat.Kind kind = CreateKind("KeepMoving");
        public static readonly Stat.Category category = CreateCategory("KeepMoving");

        public KeepMovingStat(Character owner, double value) : base(owner, value)
        {
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override void Attach()
        {
            new Traverse(Owner.movement.config).Field("keepMove").SetValue(Value != 0);
        }

        public override void Detach()
        {
            if (Owner is null)
                return;

            new Traverse(Owner.movement.config).Field("keepMove").SetValue(defaultValue);
        }
    }
}
