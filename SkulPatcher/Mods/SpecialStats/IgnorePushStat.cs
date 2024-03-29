﻿using Characters;
using Characters.Movements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkulPatcher.Mods.SpecialStats
{
    public class IgnorePushStat : SpecialStat
    {
        public static readonly Stat.Kind kind = CreateKind("IgnorePush");
        public static readonly Stat.Category category = CreateCategory("IgnorePush");

        private readonly IEnumerator coroutine;
        private readonly Dictionary<Movement.Config, bool> defaultValues = new();

        public IgnorePushStat(double value) : base(value)
        {
            coroutine = Coroutine();
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override string Title => "[Movement] IgnorePush";

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
            foreach (KeyValuePair<Movement.Config, bool> config in defaultValues)
            {
                config.Key.ignorePush = config.Value;
            }
            defaultValues.Clear();

            ModConfig.menu.StopCoroutine(coroutine);
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
                        defaultValues.Add(config, config.ignorePush);

                        config.ignorePush = true;
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
