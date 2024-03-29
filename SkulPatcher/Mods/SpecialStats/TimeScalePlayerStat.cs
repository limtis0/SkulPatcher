﻿using Characters;

namespace SkulPatcher.Mods.SpecialStats
{
    public class TimeScalePlayerStat : SpecialStat
    {
        public static readonly Stat.Kind kind = CreateKind("TimeScalePlayer");
        public static readonly Stat.Category category = CreateCategory("TimeScalePlayer");

        public override double Value { get => base.Value; set => base.Value = value / 100; }

        public TimeScalePlayerStat(double value) : base(value)
        {
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override string Title => "[General] TimeScalePlayer";

        public override double MinValue => 0;

        public override double MaxValue => 300;

        public override double DefaultValue => 100;

        public override string Abbreviation => "%";

        public override void Attach()
        {
            ModConfig.Level.player.chronometer.master.AttachTimeScale(this, (float)Value);
        }

        public override void Detach()
        {
            ModConfig.Level.player.chronometer.master.DetachTimeScale(this);
        }
    }
}
