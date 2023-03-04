using Characters;
using UnityEngine;

namespace SkulPatcher.Mods.SpecialStats
{
    public class NoclipStat : SpecialStat
    {
        private LayerMask savedTerrain;

        public static readonly Stat.Kind kind = CreateKind("Flight");
        public static readonly Stat.Category category = CreateCategory("Flight");

        public NoclipStat(double value) : base(value)
        {
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override string Title => "[Movement] Noclip";

        public override double MinValue => 0;

        public override double MaxValue => 1;

        public override double DefaultValue => 0;

        public override string Abbreviation => "≡";

        public override void Attach()
        {
            StatMenuFuncs.SetSpecialStat(FlightStat.kind, Value != 0, Value);

            savedTerrain = ModConfig.Level.player.movement.controller.terrainMask;

            if (Value != 0)
            {
                ModConfig.Level.player.movement.controller.terrainMask = new LayerMask();
                ModConfig.Level.player.movement.controller.ignorePlatform = true;
            }
        }

        public override void Detach()
        {
            StatMenuFuncs.SetSpecialStat(FlightStat.kind, false, 0);

            ModConfig.Level.player.movement.controller.terrainMask = savedTerrain;
            ModConfig.Level.player.movement.controller.ignorePlatform = false;
        }
    }
}
