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

        public static void On()
        {
            invulnerableStatus.duration = float.MaxValue;
            level.player.invulnerable.Attach(invulnerableStatus);
        }

        public static void Off()
        {
            level.player.invulnerable.Detach(invulnerableStatus);
        }
    }
}
