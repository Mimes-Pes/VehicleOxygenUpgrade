using UnityEngine;
using SMLHelper.V2.Utility;

namespace VehicleOxygenUpgrade.Configuration  // Name of the mod.
{
    // Mod config to show in Q-Mod options.
    public static class Config
    {
        public static bool UseEnergyToggleValue;
        public static bool UseEasyEnergyToggleValue;
        public static bool AirVentsAutoToggleValue;
        public static bool ShowPlayerPromptsToggleValue;        

        public static KeyCode ToggleAirVentsKeybindValue;

        public static float AirVentsFontSizeSliderValue;

        public static void Load()
        {
            UseEnergyToggleValue = PlayerPrefsExtra.GetBool("UseEnergyToggle", true);
            UseEasyEnergyToggleValue = PlayerPrefsExtra.GetBool("UseEasyEnergyToggle", false);
            AirVentsAutoToggleValue = PlayerPrefsExtra.GetBool("AirVentsAutoToggle", true);
            ShowPlayerPromptsToggleValue = PlayerPrefsExtra.GetBool("ShowPlayerPromptsToggle", true);

            ToggleAirVentsKeybindValue = PlayerPrefsExtra.GetKeyCode("ToggleAirVentsKeybindPress", KeyCode.B);

            AirVentsFontSizeSliderValue = PlayerPrefs.GetFloat("AirVentsFontSizeSlider", 25f);
        }
    }
}
