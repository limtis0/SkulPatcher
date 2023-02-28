using Characters;
using Characters.Movements;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkulPatcher
{
    public class AccelerationStat : SpecialStat
    {
        public static readonly Stat.Kind kind = CreateKind("Acceleration");
        public static readonly Stat.Category category = CreateCategory("Acceleration");

        private const string fieldName = "acceleration";
        private readonly IEnumerator coroutine;
        private readonly Dictionary<Movement.Config, float> defaultValues = new();

        public AccelerationStat(double value) : base(value)
        {
            Value /= 100;
            coroutine = Coroutine();
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

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
