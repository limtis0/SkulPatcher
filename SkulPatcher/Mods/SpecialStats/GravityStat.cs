using Characters;
using HarmonyLib;

namespace SkulPatcher
{
    public class GravityStat : SpecialStat
    {
        private const float defaultValue = -40f;

        public static readonly Stat.Kind kind = CreateKind("Gravity");
        public static readonly Stat.Category category = CreateCategory("Gravity");

        public GravityStat(Character owner, double value) : base(owner, value)
        {
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override void Attach()
        {
            Traverse gravity = new Traverse(Owner.movement.config).Field("gravity");

            float? gravityValue = gravity.GetValue() as float?;

            gravity.SetValue((float)(gravityValue! * Value));
        }

        public override void Detach()
        {
            if (Owner is null)
                return;

            new Traverse(Owner.movement.config).Field("gravity").SetValue(defaultValue);
        }
    }
}
