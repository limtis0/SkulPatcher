using Characters;

namespace SkulPatcher.Mods.SpecialStats
{
    public class TimeScaleGlobal : SpecialStat
    {
        public static readonly Stat.Kind kind = CreateKind("TimeScaleGlobal");
        public static readonly Stat.Category category = CreateCategory("TimeScaleGlobal");

        public TimeScaleGlobal(double value) : base(value)
        {
            Value /= 100;
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override string Title => "[General] TimeScaleGlobal";

        public override double MinValue => 0;

        public override double MaxValue => 300;

        public override double DefaultValue => 100;

        public override string Abbreviation => "%";

        public override void Attach()
        {
            Chronometer.global.AttachTimeScale(this, (float)Value);
        }

        public override void Detach()
        {
            Chronometer.global.DetachTimeScale(this);
        }
    }
}
