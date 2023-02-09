using Characters.Abilities;
using Level;
using Services;
using Singletons;

namespace SkulPatcher
{
    static class Godmode
    {
        private static readonly Ability invulnerableStatus = new GetInvulnerable();
        private static readonly LevelManager level = Singleton<Service>.Instance.levelManager;

        public static void Set()
        {
            if (Config.godmodeOn)
            {
                invulnerableStatus.duration = float.MaxValue;
                level.player.invulnerable.Attach(invulnerableStatus);
            }
            else
            {
                level.player.invulnerable.Detach(invulnerableStatus);
            }
        }
    }
}
