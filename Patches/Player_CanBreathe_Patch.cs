using Harmony;
using VehicleOxygenUpgrade.Objects;

namespace VehicleOxygenUpgrade.Patches
{
    [HarmonyPatch(typeof(Player))]  // Patch for the Player class.
    [HarmonyPatch("CanBreathe")]        // The Player class's CanBreathe method.
    internal class Player_CanBreathe_Patch
    {

        [HarmonyPrefix]      // Harmony Prefix
        public static bool Prefix(ref Player __instance, ref bool __result)
        {
            if (__instance.currentMountedVehicle != null && __instance.currentMountedVehicle.modules.GetCount(Objects.VehicleOxygenUpgradeModule.TechTypeID) == 0 && !AirVentInfo.AirVentsOn)
            {
                __result = false;
                return false;
            }
            return true;

        } // end public static void Postfix(Player __instance)

    } // end internal class Player_Update_Patch

}
