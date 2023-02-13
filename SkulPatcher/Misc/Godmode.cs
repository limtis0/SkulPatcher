using Characters.Abilities;
using System.Collections;
using UnityEngine;

namespace SkulPatcher
{
    static class Godmode
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
            if (!Config.IsInGame)
                return;

            if (Config.godmodeOn)
            {
                invulnerableStatus.duration = float.MaxValue;
                Config.level.player.invulnerable.Attach(invulnerableStatus);
            }
            else
            {
                Config.level.player.invulnerable.Detach(invulnerableStatus);
            }
        }
    }
}
