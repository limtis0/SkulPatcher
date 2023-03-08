using Characters;
using Characters.Movements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkulPatcher.Mods.SpecialStats
{
    public class MaxFallSpeedStat : SpecialStat
    {
        public static readonly Stat.Kind kind = CreateKind("MaxFallSpeed");
        public static readonly Stat.Category category = CreateCategory("MaxFallSpeed");

        private readonly IEnumerator coroutine;
        private readonly Dictionary<Movement.Config, float> defaultValues = new();

        public MaxFallSpeedStat(double value) : base(value)
        {
            coroutine = Coroutine();
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override string Title => "[Movement] MaxFallSpeed";

        public override double MinValue => 0;

        public override double MaxValue => 250;

        public override double DefaultValue => 25;

        public override string Abbreviation => "≡";

        public override void Attach()
        {
            ModConfig.menu.StartCoroutine(coroutine);
        }

        public override void Detach()
        {
            ModConfig.menu.StopCoroutine(coroutine);

            foreach (KeyValuePair<Movement.Config, float> config in defaultValues)
            {
                config.Key.maxFallSpeed = config.Value;
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
                        defaultValues.Add(config, config.maxFallSpeed);

                        config.maxFallSpeed = (float)Value;
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
