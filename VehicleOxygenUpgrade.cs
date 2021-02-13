using Harmony;
using UnityEngine;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using QModManager.API;
using SMLHelper.V2.Options;
using SMLHelper.V2.Utility;


// Main mod.
namespace VehicleOxygenUpgrade  // Name of the mod.
{
    // Mod config to show in Q-Mod options.
    public static class Config
    {
        public static bool UseEnergyToggleValue;

        public static void Load()
        {
            UseEnergyToggleValue = PlayerPrefsExtra.GetBool("UseEnergyToggle", true);
        }
    }

    public class Options : ModOptions
    {
        public Options() : base("Vehicle Oxygen Upgrade")
        {
            ToggleChanged += Options_EnergyToggleChanged;
        }


        public void Options_EnergyToggleChanged(object sender, ToggleChangedEventArgs e)
        {
            if (e.Id != "useEnergyToggle") return;
            Config.UseEnergyToggleValue = e.Value;
            PlayerPrefsExtra.SetBool("UseEnergyToggle", e.Value);
        }


        // Default values of the mod
        public override void BuildModOptions()
        {
            AddToggleOption("useEnergyToggle", "Enable energy usage", Config.UseEnergyToggleValue);
        }
    }


    internal static class OtherModsInfo
    {
        internal static bool RefillableOxygenTankPresent = false;
        internal static bool OxStationPresent = false;
    }


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
            } // end if (modOxygenTank != null && mod.IsLoaded)

        } // end public static void Postfix()

    } // end internal class Vehicle_Start_Patch


    [HarmonyPatch(typeof(Vehicle))]  // Patch for the Vehicle class.
    [HarmonyPatch("ReplenishOxygen")]        // The Vehicle class's Update method.
    internal class Vehicle_ReplenishOxygen_Patch
    {
        [HarmonyPrefix]      // Harmony Prefix
        public static bool Prefix(ref Vehicle __instance)
        {
            if (Player.main.currentMountedVehicle.modules.GetCount(Modules.VehicleOxygenUpgradeModule.TechTypeID) == 0)            
                __instance.replenishesOxygen = false;            
            else            
                __instance.replenishesOxygen = true;
            
            return true;

        } // end public static bool Prefix(ref Vehicle __instance, ref bool __result)

    } // end internal class Vehicle_ReplenishOxygen_Patch


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

            if (Player.main.currentMountedVehicle == __instance && Config.UseEnergyToggleValue)
            {
                var efficiencyLoaded = __instance.modules.GetCount(TechType.VehiclePowerUpgradeModule);
                //float energyCost = 0.1f; // vanilla 0.1f per sec 
                float energyCost = Player.main.currentMountedVehicle.oxygenEnergyCost;
                
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

                // Consume energy for continuously replenishing oxygen
                OxygenManager oxygenMgr = Player.main.oxygenMgr;
                float oxygenAvailable;
                float oxygenCapacity;
                oxygenMgr.GetTotal(out oxygenAvailable, out oxygenCapacity);

                if (!OtherModsInfo.RefillableOxygenTankPresent)
                {
                    if (oxygenAvailable == oxygenCapacity)
                        ConsumeOxygenEnergy(__instance, energyCost);
                }
                else
                {
                    //if (!Player.main.oxygenMgr.HasOxygenTank())
                    if (Player.main.currentMountedVehicle.modules.GetCount(Modules.VehicleOxygenUpgradeModule.TechTypeID) > 0)
                        ConsumeOxygenEnergy(__instance, energyCost);
                }

            } // end if (main.currentMountedVehicle != null && Config.UseEnergyToggleValue)

        } // end public static void Postfix(Vehicle __instance)

    } // end internal class Vehicle_Update_Patch


    [HarmonyPatch(typeof(Player))]  // Patch for the Player class.
    [HarmonyPatch("CanBreathe")]        // The Player class's Update method.
    internal class Player_CanBreathe_Patch
    {

        [HarmonyPrefix]      // Harmony Prefix
        public static bool Prefix(ref Player __instance, ref bool __result)
        {
            if (__instance.currentMountedVehicle != null && __instance.currentMountedVehicle.modules.GetCount(Modules.VehicleOxygenUpgradeModule.TechTypeID) == 0)
            {
                __result = false;
                return false;
            }
                return true;

        } // end public static void Postfix(Player __instance)

    } // end internal class Player_Update_Patch

} // namespace BetterVehicleInfo 


// Code to handle module upgrade
namespace VehicleOxygenUpgrade.Modules
{
    public class VehicleOxygenUpgradeModule : Equipable
    {
        public static TechType TechTypeID { get; protected set; }
        public VehicleOxygenUpgradeModule() : base("VehicleOxygenUpgradeModule", "Vehicle Oxygen Upgrade Module", "Creates breathable air in the vehicle")
        {
            OnFinishedPatching += () =>
            {
                TechTypeID = this.TechType;
            };
        }
        public override EquipmentType EquipmentType => EquipmentType.VehicleModule;
        public override TechType RequiredForUnlock => TechType.BaseUpgradeConsole;
        public override TechGroup GroupForPDA => TechGroup.VehicleUpgrades;
        public override CraftTree.Type FabricatorType => CraftTree.Type.SeamothUpgrades;
        public override string[] StepsToFabricatorTab => new string[] { "CommonModules" };
        public override QuickSlotType QuickSlotType => QuickSlotType.Passive;

        public override GameObject GetGameObject()
        {
            var prefab = CraftData.GetPrefabForTechType(TechType.SeamothElectricalDefense);
            var obj = GameObject.Instantiate(prefab);
            return obj;
        }

        protected override TechData GetBlueprintRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients =
                {
                    new Ingredient(TechType.ComputerChip, 1),
                    new Ingredient(TechType.AdvancedWiringKit, 1),
                    new Ingredient(TechType.AirBladder, 1),
                },
            };
        }
        public override string AssetsFolder { get; } = MainPatcher.AssetsFolder;

    } // end public class BetterVehicleInfoModule : Equipable

} // end namespace BetterVehicleInfo.Modules