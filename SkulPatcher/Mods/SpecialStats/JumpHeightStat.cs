using Characters;
using Characters.Movements;
using HarmonyLib;
namespace SkulPatcher
{
    public class JumpHeightStat : SpecialStat
    {
        public static readonly Stat.Kind kind = CreateKind("JumpHeight");
        public static readonly Stat.Category category = CreateCategory("JumpHeight");

        public JumpHeightStat(double value) : base(value)
        {
            Value /= 100;
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

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
    }

    [HarmonyPatch(typeof(Movement), nameof(Movement.Jump))]
    static class JumpHeightPatch
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
