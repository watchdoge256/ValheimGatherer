# Gatherer - Valheim Resource Tracker

**Version:** 1.2.0 | **Author:** Imbrus_Chrapmajster

**Gatherer** is a full-featured Valheim mod inspired by the legendary World of Warcraft Gatherer addon (v3.3.5a). It automatically logs, tracks, and pins gathered and discovered resource nodes on both your minimap and world map, prevents duplicate pin spam, supports granular per-resource mapping (e.g., mark mushrooms but ignore copper), and seamlessly shares all markings with clanmates through the in-game **Cartography Table**.

---

## Local-import manual

START HERE: Install BepInExPack_Valheim in the same r2modman profile, import the Gatherer ZIP using Settings > Import local mod, and launch with Start modded. Load a character and world, then press F8 to open Gatherer. Import the ZIP, not the DLL: the ZIP carries this manual.

CHOOSE WHAT TO MARK: On a fresh configuration, every resource is OFF. In Resources & Categories, tick an individual resource such as Red Mushroom. Its category is enabled automatically, without selecting the other resources. A Category: ON switch selects every resource in that group; switching it OFF clears that group's selections. Select All and Deselect All affect all resource groups. Choices take effect immediately and are saved for this r2modman profile. Existing configurations keep their previous selections when upgrading; use Deselect All for a clean start.

FIRST MARKER: Select Red Mushroom, close the window with F8, Esc, or Close, and walk near a wild mushroom. The default discovery mode scans within 15 metres every 2 seconds and also records gathering/mining interactions. Open the world map with M to see the marker. Nearby nodes of the same resource are merged to prevent clutter. Turning tracking off stops new markers; it does not delete existing map pins.

DISCOVERY AND SETTINGS: General & Pin Settings contains the master Mod Enabled switch, cartography sharing, notifications, discovery mode, pin prefix, and merge distances. Interact Only records picking/mining; Interact & Proximity Radar also scans nearby nodes; Interact & Hover also records nodes you look at; All combines these triggers. Custom Filters & Radar adjusts scan radius and include/exclude lists. Exclude names block matching resources; include names explicitly opt matching resources into tracking even if their normal toggles are off. Leave both lists empty for checkbox-only tracking.

SHARE WITH FRIENDS: Leave Share On Cartography Table enabled. At a Cartography Table, use Record discoveries to upload your saved map pins. Friends then use Read map at that table to receive them. Friends do not need Gatherer just to receive the shared pins. Sharing is through the table, not automatic live synchronisation.

CONFIGURATION FILE: After the first modded launch, r2modman's Configuration editor lists com.watchdoge.valheim.gatherer.cfg. When editing that file, enable BOTH the relevant Track category and the individual resource entry. For red mushrooms only, set TrackMushrooms = true and RedMushroom = true; leave other resource entries false. ToggleUiKey changes the F8 shortcut. Edit files with the game closed.

TROUBLESHOOTING: If F8 does nothing, confirm Start modded was used and Gatherer and BepInEx are enabled in the selected profile. If no pins appear, check Mod Enabled, select a resource, clear unwanted exclusions, and visit a matching wild node; an existing nearby pin may suppress duplicates. If the cursor is still missing, confirm the updated Gatherer DLL is installed and inspect BepInEx's LogOutput.log for patch errors. The menu blocks character input; it does not pause the world.

FULL REFERENCE: The ZIP includes README.md with the complete settings and resource tables. Use r2modman's Settings > Browse profile folder, then open BepInEx\plugins and the Gatherer installation folder to find README.md. Locally imported mods do not get a rendered README page in the Installed list. This local-import package puts the guide above into the description shown when you expand Gatherer.

---

## Table of Contents
1. [Core Features & Overview](#core-features--overview)
2. [Prerequisites & Installation](#prerequisites--installation)
   - [Installing via r2modman (Recommended)](#installing-via-r2modman-recommended)
   - [Manual Installation](#manual-installation)
3. [User Manual & How It Works](#user-manual--how-it-works)
   - [1. Discovery Modes (How Nodes are Detected)](#1-discovery-modes-how-nodes-are-detected)
   - [2. Cartography Table Sharing Guide](#2-cartography-table-sharing-guide)
   - [3. Duplicate Prevention & Node Clustering](#3-duplicate-prevention--node-clustering)
   - [4. Anti-Base Clutter (Base Crop Protection)](#4-anti-base-clutter-base-crop-protection)
4. [Configuration Guide](#configuration-guide)
   - [How to Access the Config](#how-to-access-the-config)
   - [Common Configuration Scenarios](#common-configuration-scenarios)
   - [Comprehensive Settings Reference](#comprehensive-settings-reference)
5. [Supported Resource Database](#supported-resource-database)
6. [Advanced Filtering (Custom & Modded Prefabs)](#advanced-filtering-custom--modded-prefabs)
7. [Troubleshooting & FAQ](#troubleshooting--faq)

---

## Core Features & Overview

- **WoW 3.3.5a Style Discovery:** Automatically spots and maps resource spawns upon gathering, mining, walking near them (radar scan), or hovering over them.
- **Native Cartography Table Integration:** Markings are created using Valheim's persistent map pin system (`m_save = true`), meaning **Record discoveries** uploads your pins to the Cartography Table and **Read map** distributes them to everyone else on the server.
- **Zero Client Lock-In (Vanilla Friendly):** Other players on your server **do not need the mod installed** to view or download your pins from the Cartography Table!
- **Granular Per-Resource Toggles:** Every single resource has its own independent toggle. Want to track mushrooms and thistle, but never copper or tin? A single toggle in the config accomplishes that.
- **Smart Duplicate Prevention:** Uses distance clustering (default 10m for herbs/mushrooms, 20m for large mineral deposits) so you don't get 50 pins for one patch of berries.
- **Customizable Pin Styles:** Select icon shapes (Dot, Hammer, Fireplace, House, Portal) and optional prefixes (e.g., `[G] `) for clear visual distinction.

---

## Prerequisites & Installation

### Installing via r2modman (Recommended)

> **Important Requirement:** Any r2modman profile requires **BepInExPack_Valheim** as the core modloader before other mods can run.

1. Open **r2modman** and select **Valheim**.
2. Select or create your profile (e.g. `Default` or your custom profile).
3. **If BepInEx is not yet installed in this profile:**
   - Go to the **Online** tab in the left menu.
   - Click **denikson-BepInExPack_Valheim** (pinned at top).
   - Click **Download** → **Download with dependencies**.
4. Import Gatherer:
   - Go to **Settings** in the left menu.
   - Click **Import local mod** → **From file**.
   - Browse to `Gatherer-1.2.0.zip` and select it. Expand Gatherer in the Installed list to read the local-import manual.
   - Confirm the author is `Imbrus_Chrapmajster`. If replacing an older `Unknown-Gatherer` import, uninstall that entry first so two copies of the plugin are not loaded. Keep your configuration file.
5. Launch the game using **Start modded** in the upper-left corner of r2modman.

### Manual Installation

1. Install **BepInEx for Valheim** (v5.4.2200 or newer) into your game root folder.
2. Extract `ValheimGatherer.dll` from `Gatherer-1.2.0.zip`.
3. Place `ValheimGatherer.dll` in `<ValheimDirectory>\BepInEx\plugins\`.
4. Launch Valheim via Steam.

---

## User Manual & How It Works

### 1. Discovery Modes (How Nodes are Detected)

Gatherer provides 4 distinct discovery behaviors selectable via `DiscoveryMode`:

1. **InteractOnly:**
   - Classic WoW Gatherer behavior.
   - Pins are placed only when you interact with the node: pick a mushroom, pluck raspberries, or hit an ore vein with a pickaxe.
2. **InteractAndProximity (Default):**
   - Combines interaction tracking with a passive radar sweep.
   - Every `ProximityScanInterval` (default 2.0s), the mod sweeps a sphere around your character up to `ProximityRadius` (default 15m) and pins any eligible unmapped resources you walk past.
3. **InteractAndHover:**
   - Pins nodes whenever you gather them or look directly at them (crosshair hover text appears).
4. **All:**
   - Enables interaction, proximity radar, and crosshair hover checks simultaneously.

### 2. Cartography Table Sharing Guide

The in-game **Cartography Table** lets vikings exchange discovered map fog and pins:

```
                  ┌──────────────────────┐
                  │    Local Player      │
                  │  Discovers Resource  │
                  └──────────┬───────────┘
                             │
                      [Auto-Pin Added]
                             │
                             ▼
                  ┌──────────────────────┐
                  │   Cartography Table  │
                  │ "Record discoveries" │
                  └──────────┬───────────┘
                             │
                     Synchronized to World
                             │
                             ▼
                  ┌──────────────────────┐
                  │     Other Players    │
                  │      "Read map"      │
                  └──────────────────────┘
```

1. As you explore and collect items, Gatherer tags your pins with persistent author and save flags.
2. Return to your base and interact with the **Cartography Table** using **"Record discoveries"** (default `E`).
3. Your Gatherer pins are now saved into the shared table data.
4. When any other player walks up and presses **"Read map"**, all your resource pins are instantly transferred to their map!
5. **HUD Feedback:** Gatherer notifies you with an on-screen prompt confirming pins were recorded to or synchronized from the table.

### 3. Duplicate Prevention & Node Clustering

In Valheim, resources like raspberries or mushrooms spawn in tight clusters of 3 to 6 instances, and copper deposits consist of dozens of hit voxels. Without duplicate control, this would clutter your map with overlapping markers.

Gatherer solves this with **intelligent radius merging**:
- **Herbs, Berries & Mushrooms:** Evaluated against `HerbMergeRadius` (default 10m). If a pin with the same resource name already exists within 10 meters, additional pins are skipped.
- **Ores & Minerals:** Evaluated against `OreMergeRadius` (default 20m) to encompass the full footprint of copper, silver, or obsidian deposits.
- **Special POIs:** Evaluated against `SpecialMergeRadius` (default 15m).

### 4. Anti-Base Clutter (Base Crop Protection)

Gatherer checks whether an object was cultivated or constructed by a player (`Piece.IsPlacedByPlayer()`).
- Farmed carrots, turnips, barley, onions, or player-built beehives inside your base will **never** trigger map pins.
- Only wild, naturally spawned nodes in the world will be mapped.

---

## Configuration Guide

### How to Access the Config

- **In-Game Interactive Menu (Built-in!):** Press **`F8`** anytime in-game to open Gatherer's custom interactive GUI. The mouse cursor appears automatically and game inputs are paused so you can freely click checkboxes, sliders, and buttons. You can toggle individual resources, categories, "Select All", radar ranges, and cartography sharing with real-time effect! (The keybind is rebindable via `ToggleUiKey`).
- **Default State (Clean Slate):** All resource tracking is **disabled by default**. You choose exactly what you want to track by opening F8 and checking what you need (e.g. check "Red Mushroom" or click "Category: ON" or "Select All").
- **Inside r2modman:** Click **Configuration editor** in the left sidebar, find `com.watchdoge.valheim.gatherer.cfg`, and click **Edit Config**.
- **In-Game (via BepInEx ConfigurationManager):** Press **`F1`** while in-game (if installed) to modify settings live.
- **Manual File:** Found at `<ValheimRoot>\BepInEx\config\com.watchdoge.valheim.gatherer.cfg`.

### Common Configuration Scenarios

#### Scenario A: Mark Mushrooms and Berries, but NEVER Copper or Ores
```ini
[3 - Categories]
TrackMushrooms = true
TrackBerries = true
TrackOresAndVeins = false

[4 - Specific Resources - Mushrooms]
RedMushroom = true
YellowMushroom = true
BlueMushroom = true
Magecap = true
JotunPuffs = true
SmokePuff = true

[5 - Specific Resources - Berries]
Raspberry = true
Blueberry = true
Cloudberry = true

[7 - Specific Resources - Ores]
CopperDeposit = false
```

#### Scenario B: Mark ONLY High-Tier Resources (Silver, Magecap, Jotun Puffs, Flametal)
```ini
[3 - Categories]
TrackMushrooms = true
TrackOresAndVeins = true

[4 - Specific Resources - Mushrooms]
RedMushroom = false
YellowMushroom = false
BlueMushroom = false
Magecap = true
JotunPuffs = true

[7 - Specific Resources - Ores]
CopperDeposit = false
TinDeposit = false
SilverVein = true
FlametalOre = true
```

#### Scenario C: Stealth / Exploration Radar (Quiet Mode)
```ini
[0 - General]
DiscoveryMode = InteractAndProximity
ShowHudNotification = false

[2 - Proximity Radar]
ProximityRadius = 25.0
ProximityScanInterval = 1.5
```

---

### Comprehensive Settings Reference

#### Section: `0 - General`
| Key | Type | Default | Description |
|---|---|---|---|
| `Enabled` | Boolean | `true` | Master switch for the mod. |
| `ToggleUiKey` | KeyCode | `F8` | Hotkey to open/close the interactive in-game resource configuration window. |
| `DiscoveryMode` | Enum | `InteractAndProximity` | Detection trigger (`InteractOnly`, `InteractAndProximity`, `InteractAndHover`, `All`). |
| `ShareOnCartographyTable` | Boolean | `true` | Saves pins with sync flag enabled for Cartography Table distribution. |
| `ShowHudNotification` | Boolean | `true` | Displays `[Gatherer] Marked <Resource>` in top-left HUD. |
| `PinNamePrefix` | String | `""` | Optional string prepended to pin labels (e.g. `[G] `). |
| `CleanPinNames` | Boolean | `true` | Strips redundant suffixes (e.g. "Copper" instead of "Copper Deposit"). |

#### Section: `1 - Pin Settings`
| Key | Type | Default | Description |
|---|---|---|---|
| `DefaultPinIcon` | Enum | `Dot` | Pin icon for general plants, berries, mushrooms (`Dot`, `Fireplace`, `House`, `Hammer`, `Portal`). |
| `OrePinIcon` | Enum | `Hammer` | Pin icon for ore veins and deposits. |
| `SpecialPinIcon` | Enum | `Fireplace` | Pin icon for wild beehives and special POIs. |
| `HerbMergeRadius` | Float | `10.0` | Minimum meters between duplicate herb/shroom/berry pins. |
| `OreMergeRadius` | Float | `20.0` | Minimum meters between duplicate ore vein pins. |
| `SpecialMergeRadius` | Float | `15.0` | Minimum meters between duplicate special POI pins. |

#### Section: `2 - Proximity Radar`
| Key | Type | Default | Description |
|---|---|---|---|
| `ProximityRadius` | Float | `15.0` | Radar scan radius in meters around the player. |
| `ProximityScanInterval` | Float | `2.0` | Frequency in seconds between radar sweeps. |

#### Section: `3 - Categories`
| Key | Type | Default | Description |
|---|---|---|---|
| `TrackMushrooms` | Boolean | `false` | Master category toggle for all mushrooms. |
| `TrackBerries` | Boolean | `false` | Master category toggle for all berry bushes. |
| `TrackHerbsAndPlants` | Boolean | `false` | Master category toggle for wild plants and seed crops. |
| `TrackFarmingCrops` | Boolean | `false` | Master category toggle for wild barley and flax. |
| `TrackOresAndVeins` | Boolean | `false` | Master category toggle for all ores and deposits. |
| `TrackBeehives` | Boolean | `false` | Master category toggle for wild beehives. |
| `TrackSpecial` | Boolean | `false` | Master category toggle for vegvisirs, roots, spawners. |

#### Section: `4 - Specific Resources - Mushrooms`
| Key | Type | Default | Biome / Notes |
|---|---|---|---|
| `RedMushroom` | Boolean | `false` | Meadows / Black Forest |
| `YellowMushroom` | Boolean | `false` | Burial Chambers / Sunken Crypts / Caves |
| `BlueMushroom` | Boolean | `false` | Deep forest & caves |
| `Magecap` | Boolean | `false` | Mistlands magic food |
| `JotunPuffs` | Boolean | `false` | Mistlands stamina food |
| `SmokePuff` | Boolean | `false` | Ashlands food |

#### Section: `5 - Specific Resources - Berries`
| Key | Type | Default | Biome / Notes |
|---|---|---|---|
| `Raspberry` | Boolean | `false` | Meadows bushes |
| `Blueberry` | Boolean | `false` | Black Forest bushes |
| `Cloudberry` | Boolean | `false` | Plains plains |

#### Section: `6 - Specific Resources - Herbs and Plants`
| Key | Type | Default | Biome / Notes |
|---|---|---|---|
| `Thistle` | Boolean | `false` | Black Forest / Swamp |
| `Dandelion` | Boolean | `false` | Meadows |
| `CarrotSeeds` | Boolean | `false` | Black Forest wild flowers |
| `TurnipSeeds` | Boolean | `false` | Swamp wild flowers |
| `OnionSeeds` | Boolean | `false` | Mountain chests / wild |
| `Fiddlehead` | Boolean | `false` | Ashlands ferns |
| `Barley` | Boolean | `false` | Plains Fuling villages |
| `Flax` | Boolean | `false` | Plains Fuling villages |

#### Section: `7 - Specific Resources - Ores`
| Key | Type | Default | Biome / Notes |
|---|---|---|---|
| `CopperDeposit` | Boolean | `false` | Black Forest mounds |
| `TinDeposit` | Boolean | `false` | Black Forest shorelines |
| `SilverVein` | Boolean | `false` | Mountain underground veins |
| `IronMuddyScrapPile`| Boolean | `false` | Swamp Sunken Crypts / muddy veins |
| `Obsidian` | Boolean | `false` | Mountain surface boulders |
| `FlametalOre` | Boolean | `false` | Ashlands meteorites |

#### Section: `8 - Specific Resources - Special`
| Key | Type | Default | Biome / Notes |
|---|---|---|---|
| `Beehive` | Boolean | `false` | Abandoned Meadow / Black Forest houses |
| `Vegvisir` | Boolean | `false` | Boss locator runestones |
| `SurtlingSpawner` | Boolean | `false` | Swamp fire geysers & Ashlands spawners |
| `AncientRoot` | Boolean | `false` | Mistlands sap-extractable roots |

#### Section: `9 - Advanced Filters`
| Key | Type | Default | Description |
|---|---|---|---|
| `CustomIncludeFilter` | String | `""` | Comma-separated prefab or display names to forcibly pin. |
| `CustomExcludeFilter` | String | `""` | Comma-separated prefab or display names to forcibly blacklist. |

---

## Supported Resource Database

| Internal ID | Display Name | Category | Default Icon | Merge Radius |
|---|---|---|---|---|
| `Mushroom` | Mushroom | Mushrooms | Dot | 10m |
| `MushroomYellow` | Yellow Mushroom | Mushrooms | Dot | 10m |
| `MushroomBlue` | Blue Mushroom | Mushrooms | Dot | 10m |
| `Magecap` | Magecap | Mushrooms | Dot | 10m |
| `JotunPuffs` | Jotun Puffs | Mushrooms | Dot | 10m |
| `SmokePuff` | Smoke Puff | Mushrooms | Dot | 10m |
| `Raspberry` | Raspberries | Berries | Dot | 10m |
| `Blueberry` | Blueberries | Berries | Dot | 10m |
| `Cloudberry` | Cloudberries | Berries | Dot | 10m |
| `Thistle` | Thistle | HerbsAndPlants | Dot | 10m |
| `Dandelion` | Dandelion | HerbsAndPlants | Dot | 10m |
| `CarrotSeeds` | Carrot Seeds | HerbsAndPlants | Dot | 10m |
| `TurnipSeeds` | Turnip Seeds | HerbsAndPlants | Dot | 10m |
| `OnionSeeds` | Onion Seeds | HerbsAndPlants | Dot | 10m |
| `Fiddlehead` | Fiddlehead | HerbsAndPlants | Dot | 10m |
| `Barley` | Barley | FarmingCrops | Dot | 10m |
| `Flax` | Flax | FarmingCrops | Dot | 10m |
| `Copper` | Copper | OresAndVeins | Hammer | 20m |
| `Tin` | Tin | OresAndVeins | Hammer | 15m |
| `Silver` | Silver | OresAndVeins | Hammer | 20m |
| `IronMudpile` | Muddy Scrap Pile | OresAndVeins | Hammer | 15m |
| `Obsidian` | Obsidian | OresAndVeins | Hammer | 15m |
| `Flametal` | Flametal | OresAndVeins | Hammer | 20m |
| `Beehive` | Beehive | Beehives | Fireplace | 15m |
| `Vegvisir` | Vegvisir | Special | Dot | 15m |
| `SurtlingSpawner`| Surtling Spawner | Special | Fireplace | 15m |
| `AncientRoot` | Ancient Root | Special | Dot | 20m |

---

## Advanced Filtering (Custom & Modded Prefabs)

Want to track custom items from mods like *Epic Loot*, *Therzie's Warfare*, or *Plant Everything*?

- **To include extra prefabs:**
  Set `CustomIncludeFilter` in Section 9:
  ```ini
  CustomIncludeFilter = DragonEgg, GoblinTotem, TarLiquid, CustomModdedHerb
  ```
- **To blacklist unwanted items:**
  Set `CustomExcludeFilter` in Section 9:
  ```ini
  CustomExcludeFilter = Flint, Stone, Branch
  ```
Custom include filters take precedence over category toggles, allowing surgical fine-tuning.

---

## Troubleshooting & FAQ

#### Q: r2modman gives an error: `Failed to find preloader dll ... scandir ... BepInEx\core`?
**A:** This happens when a new profile is created in r2modman without the base mod loader installed. Go to **Online** in r2modman, download **denikson-BepInExPack_Valheim**, and then launch via **Start modded**.

#### Q: Do other players on my multiplayer server need this mod installed?
**A:** **No!** Other players do not need the mod installed. When you upload your pins to the in-game Cartography Table, Valheim saves them into standard vanilla world map packets. Any player can walk up to the table and download your pins onto their vanilla game map.

#### Q: How do I remove or delete a pin?
**A:** Use Valheim's standard map controls: open your map (`M`), hover over the pin, and **Right-Click** it to delete it.

#### Q: Will this mod pin crops in my garden?
**A:** No. Gatherer checks `Piece.IsPlacedByPlayer()` and ignores crops, beehives, and pieces planted or constructed by players.

---

## License

Released under the **MIT License**. Created by **watchdoge256**.
