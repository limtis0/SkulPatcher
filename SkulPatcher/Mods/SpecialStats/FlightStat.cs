using Characters;
using Characters.Movements;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SkulPatcher.Mods.SpecialStats
{
    public class FlightStat : SpecialStat
    {
        public static readonly Stat.Kind kind = CreateKind("Flight");
        public static readonly Stat.Category category = CreateCategory("Flight");

        private const string typeFieldName = "type";
        private const string gravityFieldName = "ignoreGravity";
        private readonly IEnumerator coroutine;
        private readonly Dictionary<Movement.Config, (Movement.Config.Type type, bool ignoreGravity)> defaultValues = new();

        public FlightStat(double value) : base(value)
        {
            coroutine = Coroutine();
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override string Title => "[Movement] Flight";

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

            foreach (KeyValuePair<Movement.Config, (Movement.Config.Type type, bool ignoreGravity) > config in defaultValues)
            {
                new Traverse(config.Key).Field(typeFieldName).SetValue(config.Value.type);
                new Traverse(config.Key).Field(gravityFieldName).SetValue(config.Value.ignoreGravity);
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
                        Traverse traverse = new(config);

                        Movement.Config.Type typeFieldValue = (Movement.Config.Type)traverse.Field(typeFieldName).GetValue();
                        bool gravityFieldValue = (bool)traverse.Field(gravityFieldName).GetValue();

                        defaultValues.Add(config, (typeFieldValue, gravityFieldValue));

                        traverse.Field(typeFieldName).SetValue(Movement.Config.Type.Bidimensional);
                        traverse.Field(gravityFieldName).SetValue(true);
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
