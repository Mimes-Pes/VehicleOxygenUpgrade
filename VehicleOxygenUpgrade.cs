using System.Text;
using Harmony;
using UnityEngine;
using UnityEngine.UI;
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
        public static bool AirVentsAutoToggleValue;

        public static KeyCode ToggleAirVentsKeybindValue;

        public static float AirVentsFontSizeSliderValue;

        public static void Load()
        {
            UseEnergyToggleValue = PlayerPrefsExtra.GetBool("UseEnergyToggle", true);
            AirVentsAutoToggleValue = PlayerPrefsExtra.GetBool("AirVentsAutoToggle", false);

            ToggleAirVentsKeybindValue = PlayerPrefsExtra.GetKeyCode("ToggleAirVentsKeybindPress", KeyCode.B);

            AirVentsFontSizeSliderValue = PlayerPrefs.GetFloat("AirVentsFontSizeSlider", 25f);
        }
    }

    public class Options : ModOptions
    {
        public Options() : base("Vehicle Oxygen Upgrade")
        {
            ToggleChanged += Options_EnergyToggleChanged;
            ToggleChanged += Options_AirVentsAutoToggleChanged;            

            KeybindChanged += Options_ToggleAirVentsKeybindValueChanged;

            SliderChanged += Options_AirVentsFontSizeSliderChanged;
        }


        public void Options_EnergyToggleChanged(object sender, ToggleChangedEventArgs e)
        {
            if (e.Id != "useEnergyToggle") return;
            Config.UseEnergyToggleValue = e.Value;
            PlayerPrefsExtra.SetBool("UseEnergyToggle", e.Value);
        }

        public void Options_AirVentsAutoToggleChanged(object sender, ToggleChangedEventArgs e)
        {
            if (e.Id != "airVentsAutoToggle") return;
            Config.AirVentsAutoToggleValue = e.Value;
            PlayerPrefsExtra.SetBool("AirVentsAutoToggle", e.Value);
        }


        public void Options_ToggleAirVentsKeybindValueChanged(object sender, KeybindChangedEventArgs e)
        {
            if (e.Id != "toggleAirVentsKeybindPress") return;
            Config.ToggleAirVentsKeybindValue = e.Key;
            PlayerPrefsExtra.SetKeyCode("ToggleAirVentsKeybindPress", e.Key);
        }


        public void Options_AirVentsFontSizeSliderChanged(object sender, SliderChangedEventArgs e)
        {
            if (e.Id != "airVentsFontSizeSlider") return;
            Config.AirVentsFontSizeSliderValue = Mathf.Floor(e.Value);
            PlayerPrefs.SetFloat("AirVentsFontSizeSlider", Mathf.Floor(e.Value));
        }


        // Default values of the mod
        public override void BuildModOptions()
        {
            AddToggleOption("useEnergyToggle", "Enable energy usage", Config.UseEnergyToggleValue);
            AddToggleOption("airVentsAutoToggle", "Enable auto air vents", Config.AirVentsAutoToggleValue);

            AddKeybindOption("toggleAirVentsKeybindPress", "Toggle air vents key (manual)", GameInput.Device.Keyboard, Config.ToggleAirVentsKeybindValue);

            AddSliderOption("airVentsFontSizeSlider", "Font size air vents (defult is 25)", 10, 40, Config.AirVentsFontSizeSliderValue);
        }
    }


    internal static class OtherModsInfo
    {
        internal static bool RefillableOxygenTankPresent = false;
        internal static bool PrawnSuitTorpedoDisplayPresent = false;
        internal static bool OxStationPresent = false;
    }



    // ######################################################################
    // Add on airvent HUD variables
    //
    // ######################################################################

    internal static class AirVentInfo
    {
        internal static bool AirVentsOn = false;
        internal static string AirVentHUDTextPromptOpen = "Open air vents ";
        internal static string AirVentHUDTextPromptClose = "Close air vents ";
        internal static string AirVentHUDTextAutoOpened = "Air vents auto opened";
        internal static string AirVentHUDTextAutoClosed = "Air vents auto closed";
        internal static string AirVentHUDTextClosed = "Air vents closed";
        internal static string colorYellow = "<color=yellow>";
        internal static string colorRed = "<color=red>";
        internal static string colorEnd = "</color>";
        internal static Text AirVentDisplayText;
        internal static GameObject gameObject;
        internal static Vector3 AirVentHUDPosition = new Vector3(0f, -290f, 0f); // was 600, -185, 0
        internal static Vector2 AirVentHUDSize = new Vector2(500f, 200f);

        internal static void DisplayVehicleInfo()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string thisText1 = Traverse.Create(HandReticle.main).Field("useText1").GetValue<string>();
            string thisText2 = Traverse.Create(HandReticle.main).Field("useText2").GetValue<string>();

            // prompt for Air vent vents toggle
            if (Mathf.RoundToInt(Player.main.GetDepth()) > 0)
            {
                stringBuilder.Append("<color=yellow>");
                if (Config.AirVentsAutoToggleValue)
                    stringBuilder.Append("Air vents auto closed");
                else
                    stringBuilder.Append("Air vents closed");
                stringBuilder.Append("</color>");
            }
            else
            {
                if (Config.AirVentsAutoToggleValue)
                {
                    stringBuilder.Append("<color=yellow>");
                    stringBuilder.Append("Air vents auto opened");
                    stringBuilder.Append("</color>");
                }
                else
                {
                    if (!AirVentInfo.AirVentsOn)
                    {
                        stringBuilder.Append("<color=red>");
                        stringBuilder.Append("Open air vents ");
                        stringBuilder.Append("</color>");
                    }
                    else
                        stringBuilder.Append("Close air vents ");
                    stringBuilder.AppendFormat("(<color=#ADF8FFFF>{0}</color>)", Config.ToggleAirVentsKeybindValue.ToString());
                }
            }

            SeaMoth thisSeaMoth = Player.main.currentMountedVehicle.GetComponent<SeaMoth>();
            if (thisSeaMoth)
                stringBuilder.Append('\n');
            stringBuilder.Append(thisText1);
            string result = stringBuilder.ToString();
            HandReticle.main.SetUseTextRaw(result, thisText2);
        }

        internal static void DisplayVehicleInfoForPSTD(Exosuit thisExosuit)
        {
            GameObject gameObject = GameObject.Find("HUD");
            GameObject gameObject2 = GameObject.Find("AirVentsDisplayUI");

            if (Player.main.currentMountedVehicle == thisExosuit)
            {
                if (gameObject2 == null)
                    gameObject2 = new GameObject("AirVentsDisplayUI");

                gameObject2.transform.SetParent(gameObject.transform, false);
                Text text = gameObject2.GetComponent<Text>();

                if (text == null)
                    text = gameObject2.gameObject.AddComponent<Text>();

                text.font = Player.main.textStyle.font;
                text.fontSize = Mathf.RoundToInt(Config.AirVentsFontSizeSliderValue);
                text.alignment = TextAnchor.LowerCenter;
                text.color = Color.white;
                RectTransform component = text.GetComponent<RectTransform>();
                component.localPosition = AirVentInfo.AirVentHUDPosition;
                component.sizeDelta = AirVentInfo.AirVentHUDSize;
                AirVentInfo.AirVentDisplayText = text;
                AirVentInfo.gameObject = gameObject2;
                StringBuilder stringBuilder = new StringBuilder();

                // Air vents display
                if (Mathf.RoundToInt(Player.main.GetDepth()) > 0)
                {
                    if (Config.AirVentsAutoToggleValue)
                        AirVentInfo.AirVentDisplayText.text = AirVentInfo.colorYellow + AirVentInfo.AirVentHUDTextAutoClosed + AirVentInfo.colorEnd;
                    else
                        AirVentInfo.AirVentDisplayText.text = AirVentInfo.colorYellow + AirVentInfo.AirVentHUDTextClosed + AirVentInfo.colorEnd;
                }
                else
                {
                    if (Config.AirVentsAutoToggleValue)
                    {
                        AirVentInfo.AirVentDisplayText.text = AirVentInfo.colorYellow + AirVentInfo.AirVentHUDTextAutoOpened + AirVentInfo.colorEnd;
                    }
                    else
                    {
                        if (!AirVentInfo.AirVentsOn)
                        {
                            stringBuilder.Append(AirVentInfo.colorRed);
                            stringBuilder.Append(AirVentInfo.AirVentHUDTextPromptOpen);
                            stringBuilder.Append(AirVentInfo.colorEnd);
                            stringBuilder.AppendFormat("(<color=#ADF8FFFF>{0}</color>)", Config.ToggleAirVentsKeybindValue.ToString());
                            //AirVentInfo.AirVentDisplayText.text = stringBuilder.ToString();
                        }
                        else
                        {
                            stringBuilder.Append(AirVentInfo.AirVentHUDTextPromptClose);
                            stringBuilder.AppendFormat("(<color=#ADF8FFFF>{0}</color>)", Config.ToggleAirVentsKeybindValue.ToString());
                            //AirVentInfo.AirVentDisplayText.text = stringBuilder.ToString();
                        }
                    }
                }

                stringBuilder.Append('\n');
                stringBuilder.Append("Exit ");
                stringBuilder.AppendFormat("(<color=#ADF8FFFF>{0}</color>)", KeyCodeUtils.KeyCodeToString(KeyCode.E));
                stringBuilder.Append('\n');
                stringBuilder.Append("Toggle lights ");
                string displayMidMouseButton = uGUI.GetDisplayTextForBinding("MouseButtonMiddle");
                stringBuilder.AppendFormat("(<color=#ADF8FFFF>{0}</color>)", displayMidMouseButton);
                AirVentInfo.AirVentDisplayText.text = AirVentInfo.AirVentDisplayText.text + stringBuilder.ToString();

                gameObject2.SetActive(true);
            }
            else
            {
                if (gameObject2 != null)
                    gameObject2.SetActive(false);
            }

        }
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

            IQMod modPrawnSuitTorpedoDisplay = QModServices.Main.FindModById("PrawnSuitTorpedoDisplay");
            if (modPrawnSuitTorpedoDisplay != null && modPrawnSuitTorpedoDisplay.IsLoaded)
            {
                OtherModsInfo.PrawnSuitTorpedoDisplayPresent = true;
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
            if(Player.main.currentMountedVehicle != null)
            {
                if (Player.main.currentMountedVehicle.modules.GetCount(Modules.VehicleOxygenUpgradeModule.TechTypeID) == 0)
                    //__instance.replenishesOxygen = false;
                    Player.main.currentMountedVehicle.replenishesOxygen = false;
                else
                    //__instance.replenishesOxygen = true;
                    Player.main.currentMountedVehicle.replenishesOxygen = true;
            }
            
            return true;

        } // end public static bool Prefix(ref Vehicle __instance, ref bool __result)

    } // end internal class Vehicle_ReplenishOxygen_Patch


    // ######################################################################
    // Vehucle update
    //
    // ######################################################################

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

                //Player main = Player.main;
                //if (main != null && main.currentMountedVehicle == __instance && !main.GetPDA().isInUse)
                //{
                //    OtherModsInfo.DisplayVehicleInfo();
                //} // end if (main != null && main.currentMountedVehicle == __instance && !main.GetPDA().isInUse)

            } // end if (Player.main.currentMountedVehicle != null)

        } // end public static void Postfix(Vehicle __instance)

    } // end internal class Vehicle_Update_Patch

    
    
    [HarmonyPatch(typeof(SeaMoth))]  // Patch for the SeaMoth class.
    [HarmonyPatch("Update")]        // The SeaMoth class's Update method.
    internal class SeaMoth_Update_Patch
    {
        // Change vanilla Seamoth operation.
        [HarmonyPostfix]      // Harmony postfix
        public static void Postfix(SeaMoth __instance)
        {
            Player main = Player.main;
            if (main != null && main.currentMountedVehicle == __instance && !main.GetPDA().isInUse)
            {
                AirVentInfo.DisplayVehicleInfo();

            } // end if (main != null && main.currentMountedVehicle == __instance && !main.GetPDA().isInUse)

        } // end public static void Postfix(SeaMoth __instance)

    } // end internal class SeaMoth_Update_Patch
    


    
    [HarmonyPatch(typeof(Exosuit))]  // Patch for the Exosuit class.
    [HarmonyPatch("Update")]        // The Exosuit class's Update method.
    internal class Exosuit_Update_Patch
    {
        // Change vanilla Exosuit operation.
        [HarmonyPostfix]      // Harmony postfix
        public static void Postfix(Exosuit __instance)
        {
            Player main = Player.main;
            if (main != null && main.currentMountedVehicle == __instance && !main.GetPDA().isInUse)
            {
                if (OtherModsInfo.PrawnSuitTorpedoDisplayPresent)
                    AirVentInfo.DisplayVehicleInfoForPSTD(__instance);
                else
                    AirVentInfo.DisplayVehicleInfo();
            } // end if (Player.main != null && !Player.main.IsUnderwater() && !Player.main.GetPDA().isInUse)
            else if (OtherModsInfo.PrawnSuitTorpedoDisplayPresent)
            {
                GameObject gameObject = GameObject.Find("HUD");
                GameObject gameObject2 = GameObject.Find("AirVentsDisplayUI");

                if (gameObject2 != null)
                    gameObject2.SetActive(false);
            }

        } // end public static void Postfix(Exosuit __instance)

    } // end internal class Exosuit_Update_Patch
    

    /*
    [HarmonyPatch(typeof(Player))]  // Patch for the Player class.
    [HarmonyPatch("Update")]        // The Player class's Update method.

    internal class Player_Update_Patch
    {
        public static void Postfix(Player __instance)
        {
            if (__instance != null)
            {
                // bool playerPiloting = __instance.GetMode() == Player.Mode.LockedPiloting;
                if (__instance.GetMode() == Player.Mode.LockedPiloting)
                {
                    Exosuit thisExosuit = __instance.currentMountedVehicle.GetComponent<Exosuit>();

                    if (thisExosuit)
                    {
                        if (__instance.currentMountedVehicle == thisExosuit && !__instance.GetPDA().isInUse)
                        {
                            if (OtherModsInfo.PrawnSuitTorpedoDisplayPresent)
                                AirVentInfo.DisplayVehicleInfoForPSTD(thisExosuit);
                            else
                                AirVentInfo.DisplayVehicleInfo();
                        }
                    }
                    else
                    {
                        if (!__instance.GetPDA().isInUse)
                            AirVentInfo.DisplayVehicleInfo();
                    }
                }
                else
                {
                    GameObject gameObject = GameObject.Find("HUD");
                    GameObject gameObject2 = GameObject.Find("AirVentsDisplayUI");

                    if (gameObject2 != null)
                        gameObject2.SetActive(false);
                }
            }
        }        
    }
    */











    [HarmonyPatch(typeof(Player))]  // Patch for the Player class.
    [HarmonyPatch("CanBreathe")]        // The Player class's CanBreathe method.
    internal class Player_CanBreathe_Patch
    {

        [HarmonyPrefix]      // Harmony Prefix
        public static bool Prefix(ref Player __instance, ref bool __result)
        {
            if (__instance.currentMountedVehicle != null && __instance.currentMountedVehicle.modules.GetCount(Modules.VehicleOxygenUpgradeModule.TechTypeID) == 0 && !AirVentInfo.AirVentsOn)
            {
                __result = false;
                return false;
            }
            return true;

        } // end public static void Postfix(Player __instance)

    } // end internal class Player_Update_Patch


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
        }

    } // end internal class OxygenManager_AddOxygenAtSurface_Patch

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