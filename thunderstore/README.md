# Gatherer - Valheim Resource Tracker

**Gatherer** is a Valheim mod inspired by the legendary World of Warcraft Gatherer mod (v3.3.5a). It automatically records and pins gathered and discovered resource nodes on both your minimap and world map.

---

## Features

- **WoW 3.3.5a Style Resource Tracking:**
  - **On Gather/Mine:** Picking mushrooms, harvesting berries, collecting thistle, or mining copper/tin/silver immediately places a pin on your map.
  - **Proximity Radar:** Automatically detects and marks resource nodes as you walk near them (configurable radius and scan interval).
  - **Hover Mode:** Optionally mark resources simply by looking at them with your crosshair.
- **Cartography Table Integration:**
  - Markings are fully integrated with Valheim's built-in **Cartography Table**.
  - When you click **Record discoveries**, all your Gatherer pins are saved to the table.
  - When other players click **Read map**, they receive all your gathered markers on their map.
  - **Vanilla Friendly:** Other players on your server do *not* need the mod installed to see or receive your pins from the Cartography Table!
- **Granular Per-Resource Configuration:**
  - Want to mark **mushrooms** but **not copper**? Every resource has an individual toggle in the configuration file!
  - You can also enable/disable entire categories with a single click.
- **Smart Duplicate Prevention:**
  - Prevents map clutter by clustering nodes within a configurable radius (e.g. 10m for herbs/mushrooms, 20m for ore veins).
  - Automatically ignores player-built structures and cultivated base crops.
- **Customizable Pin Icons & Names:**
  - Choose between Dot, Hammer, Fireplace, House, or Portal icons for different resource classes.
  - Option to clean up verbose names (e.g. "Copper" instead of "Copper deposit").
  - Optional custom pin prefix (e.g. `[G] `).
- **Advanced Filters:**
  - `CustomIncludeFilter`: Add custom or modded prefab names to track.
  - `CustomExcludeFilter`: Blacklist specific names or prefabs.

---

## Supported Resources (Out of the Box)

### Mushrooms
- Standard Red Mushroom
- Yellow Mushroom (Caves & Crypts)
- Blue Mushroom
- Magecap (Mistlands)
- Jotun Puffs (Mistlands)
- Smoke Puff (Ashlands)

### Berries
- Raspberries (Meadows)
- Blueberries (Black Forest)
- Cloudberries (Plains)

### Herbs & Wild Plants
- Thistle (Black Forest / Swamp)
- Dandelion (Meadows)
- Carrot Seeds (Black Forest)
- Turnip Seeds (Swamp)
- Onion Seeds (Mountain)
- Fiddlehead (Ashlands)

### Farming Crops
- Barley (Plains)
- Flax (Plains)

### Ores & Veins
- Copper Deposit (Black Forest)
- Tin Deposit (Black Forest shorelines)
- Silver Vein (Mountain)
- Muddy Scrap Pile / Iron (Swamp Crypts)
- Obsidian Deposit (Mountain)
- Flametal Ore (Ashlands)

### Special POIs
- Wild Beehives (Abandoned buildings)
- Vegvisir Runestones (Boss locators)
- Surtling Spawners / Fire Geysers (Swamp / Ashlands)
- Ancient Roots / Sap Roots (Mistlands)

---

## Configuration

The configuration file is located at `BepInEx/config/com.watchdoge256.valheim.gatherer.cfg` and can be edited in-game, via text editor, or directly inside **r2modman** under **Configuration editor**.

### Example: Mark Mushrooms but NOT Copper
To mark mushrooms while ignoring copper deposits, open the config and set:
```ini
[4 - Specific Resources - Mushrooms]
RedMushroom = true
YellowMushroom = true
BlueMushroom = true

[7 - Specific Resources - Ores]
CopperDeposit = false
```

### Key Settings

| Category | Setting | Default | Description |
|---|---|---|---|
| **0 - General** | `Enabled` | `true` | Master switch for Gatherer. |
| **0 - General** | `DiscoveryMode` | `InteractAndProximity` | `InteractOnly`, `InteractAndProximity`, `InteractAndHover`, or `All`. |
| **0 - General** | `ShareOnCartographyTable` | `true` | Saves pins to sync with the in-game Cartography Table. |
| **0 - General** | `ShowHudNotification` | `true` | Top-left HUD notification when a resource is marked. |
| **1 - Pin Settings** | `DefaultPinIcon` | `Dot` | Pin icon for herbs, berries, mushrooms. |
| **1 - Pin Settings** | `OrePinIcon` | `Hammer` | Pin icon for ore deposits. |
| **1 - Pin Settings** | `SpecialPinIcon` | `Fireplace` | Pin icon for beehives and special POIs. |
| **1 - Pin Settings** | `HerbMergeRadius` | `10.0` | Minimum distance in meters between duplicate herb pins. |
| **1 - Pin Settings** | `OreMergeRadius` | `20.0` | Minimum distance in meters between duplicate ore pins. |
| **2 - Proximity Radar** | `ProximityRadius` | `15.0` | Range in meters for radar auto-discovery. |
| **2 - Proximity Radar** | `ProximityScanInterval` | `2.0` | Seconds between radar scans. |
| **9 - Advanced Filters** | `CustomIncludeFilter` | `""` | Comma-separated extra prefabs to mark (e.g. `DragonEgg, GoblinTotem`). |
| **9 - Advanced Filters** | `CustomExcludeFilter` | `""` | Comma-separated prefabs to blacklist. |

---

## Installation via r2modman

1. Open **r2modman** and select **Valheim**.
2. Select your profile.
3. Click **Settings** in the left sidebar.
4. Select **Import local mod** -> **From file**.
5. Select the `Gatherer-1.0.0.zip` package file.
6. The mod is now installed and active!

---

## Manual Installation

1. Ensure **BepInEx for Valheim** is installed.
2. Place `ValheimGatherer.dll` inside `<ValheimDirectory>/BepInEx/plugins/`.
3. Launch the game.
