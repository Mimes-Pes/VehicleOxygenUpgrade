using Harmony;

namespace VehicleOxygenUpgrade.Patches
{
    [HarmonyPatch(typeof(Vehicle))]  // Patch for the Vehicle class.
    [HarmonyPatch("ReplenishOxygen")]        // The Vehicle class's Update method.
    internal class Vehicle_ReplenishOxygen_Patch
    {
        [HarmonyPrefix]      // Harmony Prefix
        public static bool Prefix(ref Vehicle __instance)
        {
            if (Player.main.currentMountedVehicle != null)
            {
                if (Player.main.currentMountedVehicle.modules.GetCount(Objects.VehicleOxygenUpgradeModule.TechTypeID) == 0)
                    //__instance.replenishesOxygen = false;
                    Player.main.currentMountedVehicle.replenishesOxygen = false;
                else
                    //__instance.replenishesOxygen = true;
                    Player.main.currentMountedVehicle.replenishesOxygen = true;
            }

            return true;

        } // end public static bool Prefix(ref Vehicle __instance, ref bool __result)

    } // end internal class Vehicle_ReplenishOxygen_Patch
}
