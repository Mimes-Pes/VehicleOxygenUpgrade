using Harmony;
using QModManager.API;
using VehicleOxygenUpgrade.Objects;

namespace VehicleOxygenUpgrade.Patches
{
    [HarmonyPatch(typeof(Vehicle))]  // Patch for the Vehicle class.
    [HarmonyPatch("Start")]        // The Vehicle class's Start method.
    internal class Vehicle_Start_Patch
    {
        [HarmonyPrefix]      // Harmony Postfix
        public static void Postfix()
        {
            IQMod modOxygenTank = QModServices.Main.FindModById("OxygenTank");
            if (modOxygenTank != null && modOxygenTank.IsLoaded)
            {
                OtherModsInfo.RefillableOxygenTankPresent = true;
            }

            IQMod modPrawnSuitTorpedoDisplay = QModServices.Main.FindModById("PrawnSuitTorpedoDisplay");
            if (modPrawnSuitTorpedoDisplay != null && modPrawnSuitTorpedoDisplay.IsLoaded)
            {
                OtherModsInfo.PrawnSuitTorpedoDisplayPresent = true;
            }

            IQMod modRm_VehicleLightsImproved = QModServices.Main.FindModById("Rm_VehicleLightsImproved");
            if (modRm_VehicleLightsImproved != null && modRm_VehicleLightsImproved.IsLoaded)
            {
                OtherModsInfo.Rm_VehicleLightsImprovedPresent = true;
            }

        } // end public static void Postfix()

    } // end internal class Vehicle_Start_Patch
}
