using Harmony;
using VehicleOxygenUpgrade.Objects;
using VehicleOxygenUpgrade.Configuration;

namespace VehicleOxygenUpgrade.Patches
{
    [HarmonyPatch(typeof(SeaMoth))]  // Patch for the SeaMoth class.
    [HarmonyPatch("Update")]        // The SeaMoth class's Update method.
    internal class SeaMoth_Update_Patch
    {
        // Change vanilla Seamoth operation.
        [HarmonyPostfix]      // Harmony postfix
        public static void Postfix(SeaMoth __instance)
        {
            if (Config.ShowPlayerPromptsToggleValue)
            {
                Player main = Player.main;
                if (main != null && main.currentMountedVehicle == __instance && !main.GetPDA().isInUse)
                    AirVentInfo.DisplayVehicleInfo();
            }

        } // end public static void Postfix(SeaMoth __instance)

    } // end internal class SeaMoth_Update_Patch

}
