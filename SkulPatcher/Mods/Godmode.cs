using Characters.Abilities;
using System.Collections;
using UnityEngine;

namespace SkulPatcher
{
    public static class Godmode
    {
        public static IEnumerator Coroutine()
        {
            while (true)
            {
                SetGodmode();

                yield return new WaitForSeconds(1f);
            }
        }

        private static readonly Ability invulnerableStatus = new GetInvulnerable();

        private static void SetGodmode()
        {
            if (!ModConfig.IsInGame)
                return;

            if (ModConfig.godmodeOn)
            {
                invulnerableStatus.duration = float.MaxValue;
                ModConfig.Level.player.invulnerable.Attach(invulnerableStatus);
            }
            else
            {
                ModConfig.Level.player.invulnerable.Detach(invulnerableStatus);
            }
        }
    }
}
