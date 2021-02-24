using System.IO;
using System.Reflection;
using Harmony;
using SMLHelper.V2.Handlers;
using VehicleOxygenUpgrade.Configuration;

// You can use this file almost as-is. Just change the marked lines below. This will be the main file of each mod that tells Harmony to load your changes.
namespace VehicleOxygenUpgrade     // Change this line to match your mod.
{
    public class MainPatcher
    {
        private static Assembly thisAssembly = Assembly.GetExecutingAssembly();
        private static string ModPath = Path.GetDirectoryName(thisAssembly.Location);
        internal static string AssetsFolder = Path.Combine(ModPath, "Assets");

        public static void Patch()
        {
            var harmony = HarmonyInstance.Create("com.mimes.subnautica.bettervehicleinfo");   // Change this line to match your mod.
            var betterVehicleInfo = new Objects.VehicleOxygenUpgradeModule();
            betterVehicleInfo.Patch();
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Config.Load();
            OptionsPanelHandler.RegisterModOptions(new Options.Options());
        }
    }
}