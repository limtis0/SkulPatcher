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
        private static readonly LevelManager level = Singleton<Service>.Instance.levelManager;

        public static void SpawnGear<T>(GearReference gearRef) where T : Gear
        {
            GearRequest request = gearRef.LoadAsync();
            request.WaitForCompletion();

            T gear = (T)request.asset;
            gear.name = gearRef.name;

            level.DropGear(gear, level.player.transform.position);
        }
    }
}
