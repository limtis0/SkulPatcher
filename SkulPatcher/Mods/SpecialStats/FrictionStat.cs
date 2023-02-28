﻿using Characters;
using Characters.Movements;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkulPatcher
{
    public class FrictionStat : SpecialStat
    {
        public static readonly Stat.Kind kind = CreateKind("Friction");
        public static readonly Stat.Category category = CreateCategory("Friction");

        private const string fieldName = "friction";
        private readonly IEnumerator coroutine;
        private readonly Dictionary<Movement.Config, float> defaultValues = new();

        public FrictionStat(double value) : base(value)
        {
            Value /= 100;
            coroutine = Coroutine();
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override void Attach()
        {
            ModConfig.menu.StartCoroutine(coroutine);
        }

        public override void Detach()
        {
            foreach (KeyValuePair<Movement.Config, float> config in defaultValues)
            {
                new Traverse(config.Key).Field(fieldName).SetValue(config.Value);
            }
            defaultValues.Clear();

            ModConfig.menu.StopCoroutine(coroutine);
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
                        Traverse field = new Traverse(config).Field(fieldName);
                        float fieldValue = (float)field.GetValue();

                        defaultValues.Add(config, fieldValue);

                        field.SetValue(fieldValue * (float)Value);
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
