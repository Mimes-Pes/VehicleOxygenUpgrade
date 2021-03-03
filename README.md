# VehicleOxygenUpgrade

### Summary:
This mod removes oxygen supply from Seamoth and Exosuit until the upgrade is made and inserted into a vehicle upgrade slot. This upgrade is unlocked with Base Upgrade Console.

### Requirements:
- Should work on all versions of Subnautica
- QModManager is required
- SMLHelper is required

### Features:
- No oxygen is supplied to player piloting Seamoth or Exosuit until upgrade is made
- All vehicles have now air vents which provide oxygen to player even without the Upgrade
- There are 3 energy consumption modes:
 1. No energy consumption
 2. Easy energy mode
 3. Default energy mode
- Mod provides comprehensive player key prompts
- Mod is fully customisable

Making Vehicle Oxygen Upgrade module requires:

| Ingredient          | Amount |
| ------------------- |:------:|
| Computer Chip       | 1      |
| Advanced Wiring Kit | 1      |
| AirBladder          | 1      |

Oxygen usage also consumes energy and equals to a 50% of default game Oxygen energy cost. In easy energy mode, it equals 40% of default game Oxygen energy cost. Using VehiclePowerUpgradeModule decreases energy usage further. This is stackable of up to 4 modules and up to 2 modules only in easy energy mode. Energy usage can be completely turned off in mod options.

Vehicles now also have "Air Vents", which opens manually or automatically when vehicle is partially or fully surfaced. With opened Air Vents, vehicle without Vehicle Oxygen Upgrade Module are able to provide oxygen to player. When Oxygen Upgrade Module is inserted and Air Vents are open, energy consumption is eliminated completely. Air Vent opening can be fully automatic, triggered by vehicle surfacing or submerging. In manual mode, Air Vents can be opened or closed manually when vehicle is surfaced, and Air Vents close themselves when vehicle submerges.

### Installation:
- Make sure you have both required mods installed first.
- QModManager
- SMLHelper
Extract the zip archive into your QMods folder.
Ensure in your QMods folder you have a folder named VehicleOxygenUpgrade.

### Usage:
Vehicle Oxygen Upgrade mod is fully customisable:

- In Subnautica main screen click on "Options".
- Click on "Mods" button.
- Scroll down to "Vehicle Oxygen Upgrade" section.

#### Modifiers (options):
- Enable energy usage: when ticked, vehicles provide player with oxygen at an energy cost.

- Easy energy mode: when ticked, vehicles provide player with oxygen at areduced energy cost as follows:

Easy Energy Mode:
|Number of Vehicle PowerUpgradeModules |% of base Energy Used|
| ------------------------ |:-----------:|
| 1                        |    25       |
| 2 or more                |    10       |

When not ticked, vehicles provide player with oxygen at an energy cost as follows:

Default Mod Energy Mode:
|Number of Vehicle PowerUpgradeModules |% of base Energy Used|
| ------------------------ |:-----------:|
| 1                        |    40       |
| 2                        |    30       |
| 3                        |    20       |
| 4 or more                |    10       |

- Auto air vents: When ticked, makes air vent management fully automatic. When the top of the vehicle emerges above the water, air vents automatically open. Without "Vehicle Oxygen Upgrade", player's oxygen will replenish with outside air. If player uses "Refillable Oxygen Tanks" mod, player will be able to breathe as if outside and O2 will not be consumed from refillable oxygen tanks. With "Vehicle Oxygen Upgrade" in use, vehicle will supply player at no energy cost if energy usage mode is enabled. Conversely, when the vehicle submerges, below the surface of water, air vents will automatically close.
When not ticked, it makes air vent opening manual with air vents self closing upon submersion. If a player chooses, he/she may open air vents manually, but only if the top of the vehicle is above the water. Without "Vehicle Oxygen Upgrade", player's oxygen will replenish with outside air. If player uses "Refillable Oxygen Tanks" mod, player will be able to breathe as if outside and O2 will not be consumed from refillable oxygen tanks.

- Toggle air vents key: player customisable key assignment for manual opening and closing of air vents (only works in manual mode with vehicle surfaced)

- Show player prompts: When ticked, player is presented with information and content +/- situation specific key prompts for various functions.

- Font size air vents: sets the font size for player prompts in Exosuit when using Rm_VehicleLightsImproved mod.

### Compatibility:
The mod should be fully compatible with the following mods:
1, "OxygenTank"
2, "TorpedoImprovements",
3, "PrawnSuitTorpedoDisplay"
4, "Rm_VehicleLightsImproved"
5, "SeamothEngineUpgrades"

### Languages:
- English
- German

### Known Issues:
- May not work correctly if used with mods which modify oxygen consumption behaviour.
- Tested on various Subnautica versions.

### Credits:
- Sir CCGould for concept, initial coding and icon artwork
- Pofessor Mimes for add-on features, options changes and re-coding
- Sir Desperationfighter for add-on features, options changes, German translation and beta testing
- Madam Kelsarhu for unrelenting beta testing
