using UnityEngine;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;

    // Code to handle module upgrade
namespace VehicleOxygenUpgrade.Objects
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
            public override TechCategory CategoryForPDA => TechCategory.VehicleUpgrades;
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

    } // end namespace VehicleOxygenUpgrade.Objects
