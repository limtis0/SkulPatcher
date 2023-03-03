using Characters;
namespace SkulPatcher
{
    public class AirJumpCountStat : SpecialStat
    {
        public static readonly Stat.Kind kind = CreateKind("AirJumpCount");
        public static readonly Stat.Category category = CreateCategory("AirJumpCount");

        public AirJumpCountStat(double value) : base(value)
        {
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override string Title => "[Movement] AirJumpCount";

        public override double MinValue => 0;

        public override double MaxValue => 100;

        public override double DefaultValue => 0;

        public override string Abbreviation => "≡";

        public override void Attach()
        {
            int totalJumps = ModConfig.Level.player.movement.airJumpCount.total;
            ModConfig.Level.player.movement.airJumpCount.Add(this, (int)Value - totalJumps);
        }

        public override void Detach()
        {
            ModConfig.Level.player.movement.airJumpCount.Remove(this);
        }
    }
}
