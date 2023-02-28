using Characters;
using Characters.Movements;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkulPatcher
{
    public class KeepMovingStat : SpecialStat
    {
        public static readonly Stat.Kind kind = CreateKind("KeepMoving");
        public static readonly Stat.Category category = CreateCategory("KeepMoving");

        private const string fieldName = "keepMove";
        private readonly IEnumerator coroutine;
        private readonly Dictionary<Movement.Config, bool> defaultValues = new();

        public KeepMovingStat(double value) : base(value)
        {
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
            foreach (KeyValuePair<Movement.Config, bool> config in defaultValues)
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

                        defaultValues.Add(config, (bool)field.GetValue());

                        field.SetValue(Value != 0);
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
