using Characters;
using Characters.Movements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkulPatcher.Mods.SpecialStats
{
    public class AccelerationStat : SpecialStat
    {
        public static readonly Stat.Kind kind = CreateKind("Acceleration");
        public static readonly Stat.Category category = CreateCategory("Acceleration");

        private readonly IEnumerator coroutine;
        private readonly Dictionary<Movement.Config, float> defaultValues = new();

        public override double Value { get => base.Value; set => base.Value = value / 100; }

        public AccelerationStat(double value) : base(value)
        {
            coroutine = Coroutine();
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override string Title => "[Movement] Acceleration";

        public override double MinValue => 0;

        public override double MaxValue => 1000;

        public override double DefaultValue => 100;

        public override string Abbreviation => "%";

        public override void Attach()
        {
            ModConfig.menu.StartCoroutine(coroutine);
        }

        public override void Detach()
        {
            ModConfig.menu.StopCoroutine(coroutine);

            foreach (KeyValuePair<Movement.Config, float> config in defaultValues)
            {
                config.Key.acceleration = config.Value;
            }
            defaultValues.Clear();
        }

        private IEnumerator Coroutine()
        {
            while (true)
            {
                if (ModConfig.IsInGame)
                {
                    Movement.Config config = ModConfig.Level.player.movement.config;

                    if (!defaultValues.ContainsKey(config))
                    {
                        defaultValues.Add(config, config.acceleration);

                        config.acceleration *= (float)Value;
                    }
                }
                else
                {
                    Detach();
                }

                yield return new WaitForSeconds(1);
            }
        }
    }
}
