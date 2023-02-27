using Characters;
using HarmonyLib;

namespace SkulPatcher
{
    public class IgnorePushStat : SpecialStat
    {
        private const bool defaultValue = false;

        public static readonly Stat.Kind kind = CreateKind("IgnorePush");
        public static readonly Stat.Category category = CreateCategory("IgnorePush");

        public IgnorePushStat(double value) : base(value)
        {
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override void Attach()
        {
            new Traverse(ModConfig.Level.player.movement.config).Field("ignorePush").SetValue(Value != 0);
        }

        public override void Detach()
        {
            if (ModConfig.Level.player is null)
                return;

            new Traverse(ModConfig.Level.player.movement.config).Field("ignorePush").SetValue(defaultValue);
        }
    }
}
