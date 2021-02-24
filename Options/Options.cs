using UnityEngine;
using SMLHelper.V2.Options;
using SMLHelper.V2.Utility;
using VehicleOxygenUpgrade.Configuration;

namespace VehicleOxygenUpgrade.Options  // Name of the mod.
{
    public class Options : ModOptions
    {
        public Options() : base("Vehicle Oxygen Upgrade")
        {
            ToggleChanged += Options_EnergyToggleChanged;
            ToggleChanged += Options_EasyEnergyToggleChanged;
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

        public void Options_EasyEnergyToggleChanged(object sender, ToggleChangedEventArgs e)
        {
            if (e.Id != "useEasyEnergyToggle") return;
            Config.UseEasyEnergyToggleValue = e.Value;
            PlayerPrefsExtra.SetBool("UseEasyEnergyToggle", e.Value);
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
            AddToggleOption("useEasyEnergyToggle", "Use easy energy usage mode", Config.UseEasyEnergyToggleValue);
            AddToggleOption("airVentsAutoToggle", "Enable auto air vents", Config.AirVentsAutoToggleValue);

            AddKeybindOption("toggleAirVentsKeybindPress", "Toggle air vents key (manual)", GameInput.Device.Keyboard, Config.ToggleAirVentsKeybindValue);

            AddSliderOption("airVentsFontSizeSlider", "Font size air vents (defult is 25)", 10, 40, Config.AirVentsFontSizeSliderValue);
        }
    }
}
