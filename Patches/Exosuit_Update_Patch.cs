using Harmony;
using UnityEngine;
using VehicleOxygenUpgrade.Configuration;

namespace VehicleOxygenUpgrade.Patches
{
    [HarmonyPatch(typeof(Exosuit))]  // Patch for the Exosuit class.
    [HarmonyPatch("Update")]        // The Exosuit class's Update method.
    internal class Exosuit_Update_Patch
    {
        // Change vanilla Exosuit operation.
        [HarmonyPostfix]      // Harmony postfix
        public static void Postfix(Exosuit __instance)
        {
            if (Config.ShowPlayerPromptsToggleValue)
            {
                Player main = Player.main;
                if (main != null && main.currentMountedVehicle == __instance && !main.GetPDA().isInUse)
                {
                    if (Objects.OtherModsInfo.PrawnSuitTorpedoDisplayPresent)
                        Objects.AirVentInfo.DisplayVehicleInfoForPSTD(__instance);
                    else
                        Objects.AirVentInfo.DisplayVehicleInfo();
                } // end if (Player.main != null && !Player.main.IsUnderwater() && !Player.main.GetPDA().isInUse)
                else
                {
                    GameObject gameObject2 = GameObject.Find("AirVentsDisplayUI");
                    if (gameObject2 != null)
                        gameObject2.SetActive(false);
                }

            }
            else
            {
                GameObject gameObject2 = GameObject.Find("AirVentsDisplayUI");
                if (gameObject2 != null)
                    gameObject2.SetActive(false);
            }

        } // end public static void Postfix(Exosuit __instance)

    } // end internal class Exosuit_Update_Patch

}
