using Harmony;
using UnityEngine;
using SMLHelper.V2.Utility;
using VehicleOxygenUpgrade.Objects;
using VehicleOxygenUpgrade.Configuration;

namespace VehicleOxygenUpgrade.Patches
{
    [HarmonyPatch(typeof(Vehicle))]  // Patch for the Vehicle class.
    [HarmonyPatch("Update")]        // The Vehicle class's Update method.
    internal class Vehicle_Update_Patch
    {
        public static void ConsumeOxygenEnergy(Vehicle thisVehicle, float thisEnergyCost)
        {
            EnergyInterface thisEnergyInterface = thisVehicle.GetComponent<EnergyInterface>();
            //float amount = __instance.oxygenPerSecond * energyCost;
            float amount = DayNightCycle.main.deltaTime * thisEnergyCost;
            thisEnergyInterface.ConsumeEnergy(amount);
        }

        [HarmonyPostfix]      // Harmony postfix
        public static void Postfix(Vehicle __instance)
        {
            if (Player.main.currentMountedVehicle != null)
            {
                if (Player.main.currentMountedVehicle == __instance && Config.UseEnergyToggleValue && !AirVentInfo.AirVentsOn)
                {
                    var efficiencyLoaded = __instance.modules.GetCount(TechType.VehiclePowerUpgradeModule);
                    //float energyCost = 0.1f; // vanilla 0.1f per sec 
                    float energyCost = Player.main.currentMountedVehicle.oxygenEnergyCost;

                    if (Config.UseEasyEnergyToggleValue)
                    {
                        switch (efficiencyLoaded)
                        {
                            case 0:
                                energyCost *= 0.4f;
                                break;
                            case 1:
                                energyCost *= 0.25f;
                                break;
                            default:
                                energyCost *= 0.1f;
                                break;
                        }
                    }
                    else
                    {
                        switch (efficiencyLoaded)
                        {
                            case 0:
                                energyCost *= 0.5f;
                                break;
                            case 1:
                                energyCost *= 0.4f;
                                break;
                            case 2:
                                energyCost *= 0.3f;
                                break;
                            case 3:
                                energyCost *= 0.2f;
                                break;
                            default:
                                energyCost *= 0.1f;
                                break;
                        }
                    }

                    // Consume energy for continuously replenishing oxygen
                    OxygenManager oxygenMgr = Player.main.oxygenMgr;
                    oxygenMgr.GetTotal(out float oxygenAvailable, out float oxygenCapacity);

                    if (!OtherModsInfo.RefillableOxygenTankPresent)
                    {
                        if (oxygenAvailable == oxygenCapacity)
                            ConsumeOxygenEnergy(__instance, energyCost);
                    }
                    else
                    {
                        //if (!Player.main.oxygenMgr.HasOxygenTank())
                        if (Player.main.currentMountedVehicle.modules.GetCount(VehicleOxygenUpgradeModule.TechTypeID) > 0)
                            ConsumeOxygenEnergy(__instance, energyCost);
                    }

                } // end if (main.currentMountedVehicle != null && Config.UseEnergyToggleValue)

                if (KeyCodeUtils.GetKeyDown(Config.ToggleAirVentsKeybindValue))
                {
                    if (Mathf.RoundToInt(Player.main.GetDepth()) == 0)
                    {
                        if (AirVentInfo.AirVentsOn == false)
                            AirVentInfo.AirVentsOn = true;
                        else
                            AirVentInfo.AirVentsOn = false;
                    }
                }

                if (Mathf.RoundToInt(Player.main.GetDepth()) > 0)
                {
                    if (AirVentInfo.AirVentsOn == true)
                        AirVentInfo.AirVentsOn = false;
                }
                else
                {
                    if (Config.AirVentsAutoToggleValue)
                    {
                        if (AirVentInfo.AirVentsOn == false)
                            AirVentInfo.AirVentsOn = true;
                    }
                }

            } // end if (Player.main.currentMountedVehicle != null)

        } // end public static void Postfix(Vehicle __instance)

    } // end internal class Vehicle_Update_Patch

} // end namespace VehicleOxygenUpgrade.Patches
