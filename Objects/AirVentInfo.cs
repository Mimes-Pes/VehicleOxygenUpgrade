using System.Text;
using Harmony;
using UnityEngine;
using UnityEngine.UI;
using SMLHelper.V2.Utility;
using VehicleOxygenUpgrade.Configuration;

namespace VehicleOxygenUpgrade.Objects
{
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

            if (Language.main.GetCurrentLanguage() == "German")
            {
                AirVentHUDTextPromptOpen = "Luftventile öffnen ";
                AirVentHUDTextPromptClose = "Luftventile schließen ";
                AirVentHUDTextAutoOpened = "Luftventile automatisch geöffnet";
                AirVentHUDTextAutoClosed = "Luftventile automatisch geschlossen";
                AirVentHUDTextClosed = "Luftventile geschlossen";
            }

            // prompt for Air vent vents toggle
            if (Mathf.RoundToInt(Player.main.GetDepth()) > 0)
            {
                stringBuilder.Append(colorYellow);
                if (Config.AirVentsAutoToggleValue)
                    stringBuilder.Append(AirVentHUDTextAutoClosed);
                else
                    stringBuilder.Append(AirVentHUDTextClosed);
                stringBuilder.Append(colorEnd);
            }
            else
            {
                if (Config.AirVentsAutoToggleValue)
                {
                    stringBuilder.Append(colorYellow);
                    stringBuilder.Append(AirVentHUDTextAutoOpened);
                    stringBuilder.Append(colorEnd);
                }
                else
                {
                    if (!AirVentsOn)
                    {
                        stringBuilder.Append(colorRed);
                        stringBuilder.Append(AirVentHUDTextPromptOpen);
                        stringBuilder.Append(colorEnd);
                    }
                    else
                        stringBuilder.Append(AirVentHUDTextPromptClose);
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
                component.localPosition = AirVentHUDPosition;
                component.sizeDelta = AirVentHUDSize;
                AirVentDisplayText = text;
                gameObject = gameObject2;
                StringBuilder stringBuilder = new StringBuilder();

                // Air vents display
                if (Mathf.RoundToInt(Player.main.GetDepth()) > 0)
                {
                    if (Config.AirVentsAutoToggleValue)
                        AirVentDisplayText.text = colorYellow + AirVentHUDTextAutoClosed + colorEnd;
                    else
                        AirVentDisplayText.text = colorYellow + AirVentHUDTextClosed + colorEnd;
                }
                else
                {
                    if (Config.AirVentsAutoToggleValue)
                    {
                        AirVentDisplayText.text = colorYellow + AirVentHUDTextAutoOpened + colorEnd;
                    }
                    else
                    {
                        if (!AirVentsOn)
                        {
                            stringBuilder.Append(colorRed);
                            stringBuilder.Append(AirVentHUDTextPromptOpen);
                            stringBuilder.Append(colorEnd);
                            stringBuilder.AppendFormat("(<color=#ADF8FFFF>{0}</color>)", Config.ToggleAirVentsKeybindValue.ToString());
                            //AirVentDisplayText.text = stringBuilder.ToString();
                        }
                        else
                        {
                            stringBuilder.Append(AirVentHUDTextPromptClose);
                            stringBuilder.AppendFormat("(<color=#ADF8FFFF>{0}</color>)", Config.ToggleAirVentsKeybindValue.ToString());
                            //AirVentDisplayText.text = stringBuilder.ToString();
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
                AirVentDisplayText.text = AirVentDisplayText.text + stringBuilder.ToString();

                gameObject2.SetActive(true);
            }
            else
            {
                if (gameObject2 != null)
                    gameObject2.SetActive(false);
            }

        }

    }

}
