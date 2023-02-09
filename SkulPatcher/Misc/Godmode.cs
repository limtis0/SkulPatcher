using Characters.Abilities;
using Level;
using Services;
using Singletons;

namespace SkulPatcher
{
    static class Godmode
    {
        private static readonly Ability invulnerableStatus = new GetInvulnerable();

        public static void Set()
        {
            if (!Config.GameStarted)
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
