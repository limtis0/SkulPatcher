using Level;
using Services;
using Singletons;

namespace SkulPatcher
{
    public static class Config
    {
        // LuckyPatch
        public static bool luckBoostOn = true;
        public static int luckBoostPercent = 50;
        public static bool luckBoostContinuous = true;

        // CurrencyPatch
        public static bool goldMultOn = true;
        public static bool dQuartzMultOn = true;
        public static bool bonesMultOn = true;
        public static bool hQuartzMultOn = true;

        public static int goldMult = 1;
        public static int dQuartzMult = 1;
        public static int bonesMult = 1;
        public static int hQuartzMult = 1;

        // EasyPatch
        public static bool forceEasyModeOn = false;

        // God Mode
        public static bool godmodeOn = false;
        public static bool godmodePreviousState = false;

        // Turbo Actions
        public static bool turboAttackOn = true;
        public static bool turboSkillsOn = true;
        public static bool turboDashOn = true;

        // Misc
        public static readonly LevelManager level = Singleton<Service>.Instance.levelManager;
        public static bool GameStarted => !(level.player is null);
    }
}
