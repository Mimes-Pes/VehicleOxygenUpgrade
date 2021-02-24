using Harmony;
using VehicleOxygenUpgrade.Objects;

namespace VehicleOxygenUpgrade.Patches
{
    [HarmonyPatch(typeof(OxygenManager))]  // Patch for the OxygenManager class.
    [HarmonyPatch("AddOxygenAtSurface")]   // The Player class's AddOxygenAtSurface method.
    internal class OxygenManager_AddOxygenAtSurface_Patch
    {
        [HarmonyPrefix]      // Harmony Prefix
        public static bool Prefix(ref OxygenManager __instance)
        {
            // if (Player.main.currentMountedVehicle != null && !Player.main.currentMountedVehicle.IsPowered() && Player.main.currentMountedVehicle.modules.GetCount(Modules.VehicleOxygenUpgradeModule.TechTypeID) == 0)
            if (Player.main.currentMountedVehicle != null && !AirVentInfo.AirVentsOn)
                return false;
            return true;
        } // end public static bool Prefix(ref OxygenManager __instance)

    } // end internal class OxygenManager_AddOxygenAtSurface_Patch

} // end namespace VehicleOxygenUpgrade
