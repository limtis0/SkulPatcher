using Characters;
using HarmonyLib;

namespace SkulPatcher
{
    public class MaxFallSpeedStat : SpecialStat
    {
        private const float defaultValue = 25f;

        public static readonly Stat.Kind kind = CreateKind("MaxFallSpeed");
        public static readonly Stat.Category category = CreateCategory("MaxFallSpeed");

        public MaxFallSpeedStat(Character owner, double value) : base(owner, value)
        {
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override void Attach()
        {
            new Traverse(Owner.movement.config).Field("maxFallSpeed").SetValue((float)Value);
        }

        public override void Detach()
        {
            if (Owner is null)
                return;

            new Traverse(Owner.movement.config).Field("maxFallSpeed").SetValue(defaultValue);
        }
    }
}
