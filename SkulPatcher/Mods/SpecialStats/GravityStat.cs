using Characters;
using HarmonyLib;

namespace SkulPatcher
{
    public class GravityStat : SpecialStat
    {
        private const float defaultValue = -40f;

        public static readonly Stat.Kind kind = CreateKind("Gravity");
        public static readonly Stat.Category category = CreateCategory("Gravity");

        public GravityStat(double value) : base(value)
        {
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override void Attach()
        {
            Traverse gravity = new Traverse(ModConfig.Level.player.movement.config).Field("gravity");

            float? gravityValue = gravity.GetValue() as float?;

            gravity.SetValue((float)(gravityValue! * Value));
        }

        public override void Detach()
        {
            if (ModConfig.Level.player is null)
                return;

            new Traverse(ModConfig.Level.player.movement.config).Field("gravity").SetValue(defaultValue);
        }
    }
}
