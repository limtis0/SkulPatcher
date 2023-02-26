using Characters;
using HarmonyLib;

namespace SkulPatcher
{
    public class AccelerationStat : SpecialStat
    {
        private const float defaultValue = 2f;

        public static readonly Stat.Kind kind = CreateKind("Acceleration");
        public static readonly Stat.Category category = CreateCategory("Acceleration");

        public AccelerationStat(Character owner, double value) : base(owner, value)
        {
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override void Attach()
        {
            Traverse acceleration = new Traverse(Owner.movement.config).Field("acceleration");

            float? accelerationValue = acceleration.GetValue() as float?;

            acceleration.SetValue((float)(accelerationValue! * Value));
        }

        public override void Detach()
        {
            if (Owner is null)
                return;

            new Traverse(Owner.movement.config).Field("acceleration").SetValue(defaultValue);
        }
    }
}
