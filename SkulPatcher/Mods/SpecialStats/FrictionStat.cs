using Characters;
using HarmonyLib;

namespace SkulPatcher
{
    public class FrictionStat : SpecialStat
    {
        private const float defaultValue = 0.95f;

        public static readonly Stat.Kind kind = CreateKind("Friction");
        public static readonly Stat.Category category = CreateCategory("Friction");

        public FrictionStat(Character owner, double value) : base(owner, value)
        {
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override void Attach()
        {
            Traverse friction = new Traverse(Owner.movement.config).Field("friction");

            float? frictionValue = friction.GetValue() as float?;

            friction.SetValue((float)(frictionValue! * Value));
        }

        public override void Detach()
        {
            if (Owner is null)
                return;

            new Traverse(Owner.movement.config).Field("friction").SetValue(defaultValue);
        }
    }
}
