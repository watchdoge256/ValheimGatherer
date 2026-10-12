#pragma warning disable CS8618
using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;

namespace ValheimGatherer.Configuration
{
    public class GathererConfig
    {
        // 0 - General
        public ConfigEntry<bool> Enabled { get; private set; }
        public ConfigEntry<DiscoveryMode> Mode { get; private set; }
        public ConfigEntry<bool> ShareOnCartographyTable { get; private set; }
        public ConfigEntry<bool> ShowHudNotification { get; private set; }
        public ConfigEntry<string> PinNamePrefix { get; private set; }
        public ConfigEntry<bool> CleanPinNames { get; private set; }

        // 1 - Pin Settings
        public ConfigEntry<PinIconType> DefaultPinIcon { get; private set; }
        public ConfigEntry<PinIconType> OrePinIcon { get; private set; }
        public ConfigEntry<PinIconType> SpecialPinIcon { get; private set; }
        public ConfigEntry<float> HerbMergeRadius { get; private set; }
        public ConfigEntry<float> OreMergeRadius { get; private set; }
        public ConfigEntry<float> SpecialMergeRadius { get; private set; }

        // 2 - Proximity Radar
        public ConfigEntry<float> ProximityRadius { get; private set; }
        public ConfigEntry<float> ProximityScanInterval { get; private set; }

        // 3 - Categories
        public ConfigEntry<bool> TrackMushrooms { get; private set; }
        public ConfigEntry<bool> TrackBerries { get; private set; }
        public ConfigEntry<bool> TrackHerbsAndPlants { get; private set; }
        public ConfigEntry<bool> TrackFarmingCrops { get; private set; }
        public ConfigEntry<bool> TrackOresAndVeins { get; private set; }
        public ConfigEntry<bool> TrackBeehives { get; private set; }
        public ConfigEntry<bool> TrackSpecial { get; private set; }

        // 4 - Specific Resources - Mushrooms
        public ConfigEntry<bool> MarkRedMushroom { get; private set; }
        public ConfigEntry<bool> MarkYellowMushroom { get; private set; }
        public ConfigEntry<bool> MarkBlueMushroom { get; private set; }
        public ConfigEntry<bool> MarkMagecap { get; private set; }
        public ConfigEntry<bool> MarkJotunPuffs { get; private set; }
        public ConfigEntry<bool> MarkSmokePuff { get; private set; }

        // 5 - Specific Resources - Berries
        public ConfigEntry<bool> MarkRaspberry { get; private set; }
        public ConfigEntry<bool> MarkBlueberry { get; private set; }
        public ConfigEntry<bool> MarkCloudberry { get; private set; }

        // 6 - Specific Resources - Herbs & Plants
        public ConfigEntry<bool> MarkThistle { get; private set; }
        public ConfigEntry<bool> MarkDandelion { get; private set; }
        public ConfigEntry<bool> MarkCarrotSeeds { get; private set; }
        public ConfigEntry<bool> MarkTurnipSeeds { get; private set; }
        public ConfigEntry<bool> MarkOnionSeeds { get; private set; }
        public ConfigEntry<bool> MarkFiddlehead { get; private set; }
        public ConfigEntry<bool> MarkBarley { get; private set; }
        public ConfigEntry<bool> MarkFlax { get; private set; }

        // 7 - Specific Resources - Ores
        public ConfigEntry<bool> MarkCopper { get; private set; }
        public ConfigEntry<bool> MarkTin { get; private set; }
        public ConfigEntry<bool> MarkSilver { get; private set; }
        public ConfigEntry<bool> MarkIronMudpile { get; private set; }
        public ConfigEntry<bool> MarkObsidian { get; private set; }
        public ConfigEntry<bool> MarkFlametal { get; private set; }

        // 8 - Specific Resources - Special
        public ConfigEntry<bool> MarkBeehive { get; private set; }
        public ConfigEntry<bool> MarkVegvisir { get; private set; }
        public ConfigEntry<bool> MarkSurtlingSpawner { get; private set; }
        public ConfigEntry<bool> MarkAncientRoot { get; private set; }

        // 9 - Advanced Filters
        public ConfigEntry<string> CustomIncludeFilter { get; private set; }
        public ConfigEntry<string> CustomExcludeFilter { get; private set; }

        private HashSet<string> _parsedIncludes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> _parsedExcludes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public GathererConfig(ConfigFile config)
        {
            InitConfig(config);
            UpdateParsedFilters();
            config.ConfigReloaded += (s, e) => UpdateParsedFilters();
        }

        private void InitConfig(ConfigFile config)
        {
            // 0 - General
            Enabled = config.Bind("0 - General", "Enabled", true,
                "Master switch to enable or disable the Gatherer mod.");
            Mode = config.Bind("0 - General", "DiscoveryMode", DiscoveryMode.InteractAndProximity,
                "Discovery mode: InteractOnly (when picked/mined), InteractAndProximity (auto-marks nearby nodes), InteractAndHover (when looking at nodes), or All.");
            ShareOnCartographyTable = config.Bind("0 - General", "ShareOnCartographyTable", true,
                "When true, markings are saved and automatically shared to other players via the in-game Cartography Table.");
            ShowHudNotification = config.Bind("0 - General", "ShowHudNotification", true,
                "Display a notification in top-left HUD when a new resource is marked.");
            PinNamePrefix = config.Bind("0 - General", "PinNamePrefix", "",
                "Optional prefix added to pin names (e.g. '[G] ' or empty for clean name).");
            CleanPinNames = config.Bind("0 - General", "CleanPinNames", true,
                "Shortens redundant names (e.g. 'Copper' instead of 'Copper deposit', 'Tin' instead of 'Tin deposit').");

            // 1 - Pin Settings
            DefaultPinIcon = config.Bind("1 - Pin Settings", "DefaultPinIcon", PinIconType.Dot,
                "Pin icon used for general resources like mushrooms, berries, and herbs (Dot, Fireplace, House, Hammer, Portal).");
            OrePinIcon = config.Bind("1 - Pin Settings", "OrePinIcon", PinIconType.Hammer,
                "Pin icon used for ore deposits and mineral veins.");
            SpecialPinIcon = config.Bind("1 - Pin Settings", "SpecialPinIcon", PinIconType.Fireplace,
                "Pin icon used for special POIs like wild beehives and vegvisirs.");
            HerbMergeRadius = config.Bind("1 - Pin Settings", "HerbMergeRadius", 10.0f,
                "Minimum distance in meters between pins of the same herb/mushroom/berry. Prevents duplicate pins within clusters.");
            OreMergeRadius = config.Bind("1 - Pin Settings", "OreMergeRadius", 20.0f,
                "Minimum distance in meters between pins of the same ore deposit. Prevents duplicate pins for large veins.");
            SpecialMergeRadius = config.Bind("1 - Pin Settings", "SpecialMergeRadius", 15.0f,
                "Minimum distance in meters for special POIs like beehives.");

            // 2 - Proximity Radar
            ProximityRadius = config.Bind("2 - Proximity Radar", "ProximityRadius", 15.0f,
                "Radius around the player to scan for unpinned resources when using proximity discovery (WoW Gatherer radar style).");
            ProximityScanInterval = config.Bind("2 - Proximity Radar", "ProximityScanInterval", 2.0f,
                "Interval in seconds between proximity scans. Higher values improve performance, lower values spot nodes faster.");

            // 3 - Categories
            TrackMushrooms = config.Bind("3 - Categories", "TrackMushrooms", true,
                "Enable or disable tracking all mushrooms.");
            TrackBerries = config.Bind("3 - Categories", "TrackBerries", true,
                "Enable or disable tracking all berries.");
            TrackHerbsAndPlants = config.Bind("3 - Categories", "TrackHerbsAndPlants", true,
                "Enable or disable tracking wild plants and herbs.");
            TrackFarmingCrops = config.Bind("3 - Categories", "TrackFarmingCrops", true,
                "Enable or disable tracking wild and cultivated crops (e.g. barley, flax).");
            TrackOresAndVeins = config.Bind("3 - Categories", "TrackOresAndVeins", true,
                "Enable or disable tracking all ore veins and mineral deposits.");
            TrackBeehives = config.Bind("3 - Categories", "TrackBeehives", true,
                "Enable or disable tracking wild beehives.");
            TrackSpecial = config.Bind("3 - Categories", "TrackSpecial", true,
                "Enable or disable tracking special POIs (vegvisir, surtling spawner, ancient roots).");

            // 4 - Specific Resources - Mushrooms
            MarkRedMushroom = config.Bind("4 - Specific Resources - Mushrooms", "RedMushroom", true,
                "Mark standard red mushrooms.");
            MarkYellowMushroom = config.Bind("4 - Specific Resources - Mushrooms", "YellowMushroom", true,
                "Mark yellow mushrooms found in dungeons, caves, and crypts.");
            MarkBlueMushroom = config.Bind("4 - Specific Resources - Mushrooms", "BlueMushroom", true,
                "Mark blue mushrooms.");
            MarkMagecap = config.Bind("4 - Specific Resources - Mushrooms", "Magecap", true,
                "Mark Magecap mushrooms (Mistlands).");
            MarkJotunPuffs = config.Bind("4 - Specific Resources - Mushrooms", "JotunPuffs", true,
                "Mark Jotun Puffs mushrooms (Mistlands).");
            MarkSmokePuff = config.Bind("4 - Specific Resources - Mushrooms", "SmokePuff", true,
                "Mark Smoke Puff mushrooms (Ashlands).");

            // 5 - Specific Resources - Berries
            MarkRaspberry = config.Bind("5 - Specific Resources - Berries", "Raspberry", true,
                "Mark raspberry bushes (Meadows).");
            MarkBlueberry = config.Bind("5 - Specific Resources - Berries", "Blueberry", true,
                "Mark blueberry bushes (Black Forest).");
            MarkCloudberry = config.Bind("5 - Specific Resources - Berries", "Cloudberry", true,
                "Mark cloudberry bushes (Plains).");

            // 6 - Specific Resources - Herbs & Plants
            MarkThistle = config.Bind("6 - Specific Resources - Herbs and Plants", "Thistle", true,
                "Mark glowing thistle (Black Forest / Swamp).");
            MarkDandelion = config.Bind("6 - Specific Resources - Herbs and Plants", "Dandelion", true,
                "Mark dandelions (Meadows).");
            MarkCarrotSeeds = config.Bind("6 - Specific Resources - Herbs and Plants", "CarrotSeeds", true,
                "Mark wild seed carrots (Black Forest).");
            MarkTurnipSeeds = config.Bind("6 - Specific Resources - Herbs and Plants", "TurnipSeeds", true,
                "Mark wild seed turnips (Swamp).");
            MarkOnionSeeds = config.Bind("6 - Specific Resources - Herbs and Plants", "OnionSeeds", true,
                "Mark wild seed onions (Mountain chests/wild).");
            MarkFiddlehead = config.Bind("6 - Specific Resources - Herbs and Plants", "Fiddlehead", true,
                "Mark fiddleheads (Ashlands).");
            MarkBarley = config.Bind("6 - Specific Resources - Herbs and Plants", "Barley", true,
                "Mark barley (Plains).");
            MarkFlax = config.Bind("6 - Specific Resources - Herbs and Plants", "Flax", true,
                "Mark flax (Plains).");

            // 7 - Specific Resources - Ores
            MarkCopper = config.Bind("7 - Specific Resources - Ores", "CopperDeposit", true,
                "Mark copper deposits (Black Forest). Set to false if you want mushrooms but not copper!");
            MarkTin = config.Bind("7 - Specific Resources - Ores", "TinDeposit", true,
                "Mark tin deposits (Black Forest shorelines).");
            MarkSilver = config.Bind("7 - Specific Resources - Ores", "SilverVein", true,
                "Mark silver veins (Mountains).");
            MarkIronMudpile = config.Bind("7 - Specific Resources - Ores", "IronMuddyScrapPile", true,
                "Mark muddy scrap piles (Swamp Sunken Crypts / Swamp ground).");
            MarkObsidian = config.Bind("7 - Specific Resources - Ores", "Obsidian", true,
                "Mark obsidian deposits (Mountains).");
            MarkFlametal = config.Bind("7 - Specific Resources - Ores", "FlametalOre", true,
                "Mark flametal / meteorite ore deposits (Ashlands).");

            // 8 - Specific Resources - Special
            MarkBeehive = config.Bind("8 - Specific Resources - Special", "Beehive", true,
                "Mark wild beehives found in abandoned buildings.");
            MarkVegvisir = config.Bind("8 - Specific Resources - Special", "Vegvisir", true,
                "Mark Vegvisir runestones.");
            MarkSurtlingSpawner = config.Bind("8 - Specific Resources - Special", "SurtlingSpawner", true,
                "Mark fire geysers / surtling spawners (Swamp / Ashlands).");
            MarkAncientRoot = config.Bind("8 - Specific Resources - Special", "AncientRoot", true,
                "Mark ancient roots / sap roots (Mistlands).");

            // 9 - Advanced Filters
            CustomIncludeFilter = config.Bind("9 - Advanced Filters", "CustomIncludeFilter", "",
                "Comma-separated list of prefab names or localized names to always mark, e.g. 'DragonEgg, GoblinTotem'.");
            CustomExcludeFilter = config.Bind("9 - Advanced Filters", "CustomExcludeFilter", "",
                "Comma-separated list of prefab names or localized names to never mark, e.g. 'Copper, Stone, Flint'.");

            CustomIncludeFilter.SettingChanged += (s, e) => UpdateParsedFilters();
            CustomExcludeFilter.SettingChanged += (s, e) => UpdateParsedFilters();
        }

        private void UpdateParsedFilters()
        {
            _parsedIncludes = ParseFilterString(CustomIncludeFilter?.Value);
            _parsedExcludes = ParseFilterString(CustomExcludeFilter?.Value);
        }

        public static HashSet<string> ParseFilterString(string? filter)
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrWhiteSpace(filter))
                return set;

            string[] tokens = filter.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var token in tokens)
            {
                string trimmed = token.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                    set.Add(trimmed);
            }
            return set;
        }

        public bool IsCustomIncluded(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            return _parsedIncludes.Contains(name);
        }

        public bool IsCustomExcluded(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            return _parsedExcludes.Contains(name);
        }
    }
}
