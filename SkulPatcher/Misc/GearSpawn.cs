using Characters.Gear;
using GameResources;
using Level;
using Services;
using Singletons;

namespace SkulPatcher
{
    public static class GearSpawn
    {
        public static readonly GearResource gear = GearResource.instance;

        public static void SpawnGear<T>(GearReference gearRef) where T : Gear
        {
            if (!Config.GameStarted)
                return;

            GearRequest request = gearRef.LoadAsync();
            request.WaitForCompletion();

            T gear = (T)request.asset;
            gear.name = gearRef.name;

            Config.level.DropGear(gear, Config.level.player.transform.position);
        }
    }
}
