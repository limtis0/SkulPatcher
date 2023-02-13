using Characters.Gear;
using GameResources;

namespace SkulPatcher
{
    public static class GearSpawn
    {
        public static void SpawnGear<T>(GearReference gearRef) where T : Gear
        {
            if (!Config.IsInGame)
                return;

            GearRequest request = gearRef.LoadAsync();
            request.WaitForCompletion();

            T gear = (T)request.asset;
            gear.name = gearRef.name;

            Config.level.DropGear(gear, Config.level.player.transform.position);
        }
    }
}
