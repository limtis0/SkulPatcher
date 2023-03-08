using Characters;
using Characters.Movements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkulPatcher.Mods.SpecialStats
{
    public class IgnoreGravityStat : SpecialStat
    {
        public static readonly Stat.Kind kind = CreateKind("IgnoreGravity");
        public static readonly Stat.Category category = CreateCategory("IgnoreGravity");

        private readonly IEnumerator coroutine;
        private readonly Dictionary<Movement.Config, bool> defaultValues = new();

        public IgnoreGravityStat(double value) : base(value)
        {
            coroutine = Coroutine();
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override string Title => "[Movement] IgnoreGravity";

        public override double MinValue => 0;

        public override double MaxValue => 1;

        public override double DefaultValue => 0;

        public override string Abbreviation => "≡";

        public override void Attach()
        {
            ModConfig.menu.StartCoroutine(coroutine);
        }

        public override void Detach()
        {
            ModConfig.menu.StopCoroutine(coroutine);

            foreach (KeyValuePair<Movement.Config, bool> config in defaultValues)
            {
                config.Key.ignoreGravity = config.Value;
            }
            defaultValues.Clear();
        }

        private IEnumerator Coroutine()
        {
            while (true)
            {
                if (ModConfig.IsInGame && Value != 0)
                {
                    Movement.Config config = ModConfig.Level.player.movement.config;

                    if (!defaultValues.ContainsKey(config))
                    {
                        defaultValues.Add(config, config.ignoreGravity);

                        config.ignoreGravity = true;
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
