using Characters;
using Level;
using System.Collections;
using UnityEngine;

namespace SkulPatcher.Mods.SpecialStats
{
    public class TimeScaleEnemies : SpecialStat
    {
        public static readonly Stat.Kind kind = CreateKind("TimeScaleEnemies");
        public static readonly Stat.Category category = CreateCategory("TimeScaleEnemies");

        private readonly IEnumerator coroutine;

        public TimeScaleEnemies(double value) : base(value)
        {
            Value /= 100;
            coroutine = Coroutine();
        }

        public override Stat.Kind Kind => kind;

        public override Stat.Category Category => category;

        public override string Title => "[General] TimeScaleEnemies";

        public override double MinValue => 0;

        public override double MaxValue => 300;

        public override double DefaultValue => 100;

        public override string Abbreviation => "%";

        public override void Attach()
        {
            ModConfig.menu.StartCoroutine(coroutine);
        }

        public override void Detach()
        {
            ModConfig.menu.StopCoroutine(coroutine);

            foreach (Character enemy in Map.Instance.waveContainer.GetAllEnemies())
            {
                enemy.chronometer.master.DetachTimeScale(this);
            }
        }

        private IEnumerator Coroutine()
        {
            while (true)
            {
                if (ModConfig.IsInGame)
                {
                    foreach (Character enemy in Map.Instance.waveContainer.GetAllEnemies())
                    {
                        enemy.chronometer.master.AttachTimeScale(this, (float)Value);
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
