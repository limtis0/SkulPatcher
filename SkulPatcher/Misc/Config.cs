using GameResources;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace SkulPatcher
{
    public static class Config
    {
        // LuckyPatch
        public static bool luckBoostOn = PlayerPrefs.GetInt("luckBoostOn", 1) != 0;
        public static int luckBoostPercent = PlayerPrefs.GetInt("luckBoostPercent", 33);
        public static bool luckBoostContinuous = PlayerPrefs.GetInt("luckBoostContinuous", 1) != 0;

        // DuplicateGear
        public static bool allowDuplicateItems = PlayerPrefs.GetInt("allowDuplicateItems", 0) != 0;
        public static bool allowDuplicateSkulls = PlayerPrefs.GetInt("allowDuplicateSkulls", 0) != 0;

        // CurrencyPatch
        public static bool goldMultOn = PlayerPrefs.GetInt("goldMultOn", 0) != 0;
        public static bool dQuartzMultOn = PlayerPrefs.GetInt("dQuartzMultOn", 0) != 0;
        public static bool bonesMultOn = PlayerPrefs.GetInt("bonesMultOn", 0) != 0;
        public static bool hQuartzMultOn = PlayerPrefs.GetInt("hQuartzMultOn", 0) != 0;

        public static int goldMultValue = PlayerPrefs.GetInt("goldMultValue", 1);
        public static int dQuartzMultValue = PlayerPrefs.GetInt("dQuartzMultValue", 1);
        public static int bonesMultValue = PlayerPrefs.GetInt("bonesMultValue", 1);
        public static int hQuartzMultValue = PlayerPrefs.GetInt("hQuartzMultValue", 1);

        // Boss rush
        public static bool bossRushOn = PlayerPrefs.GetInt("bossRushOn", 0) != 0;

        // EasyPatch
        public static bool forceEasyModeOn = PlayerPrefs.GetInt("forceEasyModeOn", 0) != 0;

        // God Mode
        public static bool godmodeOn = PlayerPrefs.GetInt("godmodeOn", 0) != 0;

        // Turbo Actions
        public static bool turboAttackOn = PlayerPrefs.GetInt("turboAttackOn", 1) != 0;
        public static bool turboDashOn = PlayerPrefs.GetInt("turboDashOn", 0) != 0;

        // Misc
        public static readonly LevelManager level = Singleton<Service>.Instance.levelManager;
        public static readonly GearResource gear = GearResource.instance;

        public static bool IsInGame => !(level.player is null);

        public static void Save()
        {
            PlayerPrefs.SetInt("luckBoostOn", luckBoostOn ? 1 : 0);
            PlayerPrefs.SetInt("luckBoostPercent", luckBoostPercent);
            PlayerPrefs.SetInt("luckBoostContinuous", luckBoostContinuous ? 1 : 0);

            PlayerPrefs.SetInt("allowDuplicateItems", allowDuplicateItems ? 1 : 0);
            PlayerPrefs.SetInt("allowDuplicateSkulls", allowDuplicateSkulls ? 1 : 0);

            PlayerPrefs.SetInt("goldMultOn", goldMultOn ? 1 : 0);
            PlayerPrefs.SetInt("dQuartzMultOn", dQuartzMultOn ? 1 : 0);
            PlayerPrefs.SetInt("bonesMultOn", bonesMultOn ? 1 : 0);
            PlayerPrefs.SetInt("hQuartzMultOn", hQuartzMultOn ? 1 : 0);

            PlayerPrefs.SetInt("goldMultValue", goldMultValue);
            PlayerPrefs.SetInt("dQuartzMultValue", dQuartzMultValue);
            PlayerPrefs.SetInt("bonesMultValue", bonesMultValue);
            PlayerPrefs.SetInt("hQuartzMultValue", hQuartzMultValue);

            PlayerPrefs.SetInt("bossRushOn", bossRushOn ? 1 : 0);

            PlayerPrefs.SetInt("forceEasyModeOn", forceEasyModeOn ? 1 : 0);

            PlayerPrefs.SetInt("godmodeOn", godmodeOn ? 1 : 0);

            PlayerPrefs.SetInt("turboAttackOn", turboAttackOn ? 1 : 0);
            PlayerPrefs.SetInt("turboDashOn", turboDashOn ? 1 : 0);
        }
    }
}
