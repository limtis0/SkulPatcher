using Characters;
using Characters.Movements;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkulPatcher.Mods.SpecialStats
{
    public class FlightStat : SpecialStat
    {
        public static readonly Stat.Kind kind = CreateKind("Flight");
        public static readonly Stat.Category category = CreateCategory("Flight");

        private const string fieldName = "type";
        private readonly IEnumerator coroutine;
        private readonly Dictionary<Movement.Config, Movement.Config.Type> defaultValues = new();

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
            StatMenuFuncs.SetSpecialStat(IgnoreGravityStat.kind, true, 1);

            ModConfig.menu.StartCoroutine(coroutine);
        }

        public override void Detach()
        {
            StatMenuFuncs.SetSpecialStat(IgnoreGravityStat.kind, false, 0);

            ModConfig.menu.StopCoroutine(coroutine);

            foreach (KeyValuePair<Movement.Config, Movement.Config.Type> config in defaultValues)
            {
                new Traverse(config.Key).Field(fieldName).SetValue(config.Value);
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
                        Traverse field = new Traverse(config).Field(fieldName);

                        Movement.Config.Type fieldValue = (Movement.Config.Type)field.GetValue();

                        defaultValues.Add(config, fieldValue);

                        field.SetValue(Movement.Config.Type.Bidimensional);
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
