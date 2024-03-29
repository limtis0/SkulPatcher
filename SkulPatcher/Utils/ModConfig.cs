﻿using Characters.Player;
using GameResources;
using Level;
using Scenes;
using Services;
using Singletons;
using SkulPatcher.UI;
using UI;
using UnityEngine;

namespace SkulPatcher
{
    public static class ModConfig
    {
        // LuckyPatch
        public static bool luckBoostOn = PlayerPrefs.GetInt("luckBoostOn", 0) != 0;
        public static int luckBoostPercent = PlayerPrefs.GetInt("luckBoostPercent", 33);
        public static bool luckBoostContinuous = PlayerPrefs.GetInt("luckBoostContinuous", 1) != 0;

        // Auto-equip
        public static bool autoEquipSpawnedGear = PlayerPrefs.GetInt("autoEquipSpawnedGear", 0) != 0;

        // DuplicateGear
        public static bool allowDuplicateItems = PlayerPrefs.GetInt("allowDuplicateItems", 0) != 0;
        public static bool allowDuplicateSkulls = PlayerPrefs.GetInt("allowDuplicateSkulls", 0) != 0;

        // HiddenGear
        public static bool allowHiddenGear = PlayerPrefs.GetInt("allowHiddenGear", 0) != 0;
        public static bool allowHiddenGearPreviousState = false;

        // CurrencyPatch
        public static bool goldMultOn = PlayerPrefs.GetInt("goldMultOn", 0) != 0;
        public static bool dQuartzMultOn = PlayerPrefs.GetInt("dQuartzMultOn", 0) != 0;
        public static bool bonesMultOn = PlayerPrefs.GetInt("bonesMultOn", 0) != 0;
        public static bool hQuartzMultOn = PlayerPrefs.GetInt("hQuartzMultOn", 0) != 0;

        public static float goldMultValue = PlayerPrefs.GetFloat("goldMultValue", 1);
        public static float dQuartzMultValue = PlayerPrefs.GetFloat("dQuartzMultValue", 1);
        public static float bonesMultValue = PlayerPrefs.GetFloat("bonesMultValue", 1);
        public static float hQuartzMultValue = PlayerPrefs.GetFloat("hQuartzMultValue", 1);

        // EasyPatch
        public static bool forceEasyModeOn = PlayerPrefs.GetInt("forceEasyModeOn", 0) != 0;

        // God Mode
        public static bool godmodeOn = PlayerPrefs.GetInt("godmodeOn", 0) != 0;

        // Turbo Actions
        public static bool turboAttackOn = PlayerPrefs.GetInt("turboAttackOn", 1) != 0;
        public static bool turboDashOn = PlayerPrefs.GetInt("turboDashOn", 0) != 0;

        // Boss rush
        public static bool bossRushOn = PlayerPrefs.GetInt("bossRushOn", 0) != 0;
        public static bool bossRushOnPreviousState = bossRushOn;

        public static bool bossRushSkipRewards = PlayerPrefs.GetInt("bossRushSkipRewards", 1) != 0;

        public static bool bossRushIncludeShops = PlayerPrefs.GetInt("bossRushAllowShops", 1) != 0;
        public static bool bossRushShopsPreviousState = bossRushIncludeShops;

        public static bool bossRushIncludeArachne = PlayerPrefs.GetInt("bossRushAllowArachne", 1) != 0;
        public static bool bossRushArachnePreviousState = bossRushIncludeArachne;

        // Item/Skull limits
        public static int inventoryPagesCount = 1;
        public static int skullLimit = 2;

        // Utils
        public static Menu menu;

        public static LevelManager Level => Singleton<Service>.Instance.levelManager;
        public static GearResource Gear => GearResource.instance;
        public static Inventory Inventory => Level.player.playerComponents.inventory;
        public static UIManager UI => Scene<GameBase>.instance.uiManager;

        public static bool IsInGame => Level.player is not null;

        public static void Save()
        {
            PlayerPrefs.SetInt("luckBoostOn", luckBoostOn ? 1 : 0);
            PlayerPrefs.SetInt("luckBoostPercent", luckBoostPercent);
            PlayerPrefs.SetInt("luckBoostContinuous", luckBoostContinuous ? 1 : 0);

            PlayerPrefs.SetInt("autoEquipSpawnedGear", autoEquipSpawnedGear ? 1 : 0);

            PlayerPrefs.SetInt("allowDuplicateItems", allowDuplicateItems ? 1 : 0);
            PlayerPrefs.SetInt("allowDuplicateSkulls", allowDuplicateSkulls ? 1 : 0);

            PlayerPrefs.SetInt("allowHiddenGear", allowHiddenGear ? 1 : 0);

            PlayerPrefs.SetInt("goldMultOn", goldMultOn ? 1 : 0);
            PlayerPrefs.SetInt("dQuartzMultOn", dQuartzMultOn ? 1 : 0);
            PlayerPrefs.SetInt("bonesMultOn", bonesMultOn ? 1 : 0);
            PlayerPrefs.SetInt("hQuartzMultOn", hQuartzMultOn ? 1 : 0);

            PlayerPrefs.SetFloat("goldMultValue", goldMultValue);
            PlayerPrefs.SetFloat("dQuartzMultValue", dQuartzMultValue);
            PlayerPrefs.SetFloat("bonesMultValue", bonesMultValue);
            PlayerPrefs.SetFloat("hQuartzMultValue", hQuartzMultValue);

            PlayerPrefs.SetInt("forceEasyModeOn", forceEasyModeOn ? 1 : 0);

            PlayerPrefs.SetInt("godmodeOn", godmodeOn ? 1 : 0);

            PlayerPrefs.SetInt("turboAttackOn", turboAttackOn ? 1 : 0);
            PlayerPrefs.SetInt("turboDashOn", turboDashOn ? 1 : 0);

            PlayerPrefs.SetInt("bossRushOn", bossRushOn ? 1 : 0);
            PlayerPrefs.SetInt("bossRushSkipRewards", bossRushSkipRewards ? 1 : 0);
            PlayerPrefs.SetInt("bossRushAllowShops", bossRushIncludeShops ? 1 : 0);
            PlayerPrefs.SetInt("bossRushAllowArachne", bossRushIncludeArachne ? 1 : 0);
        }
    }
}
