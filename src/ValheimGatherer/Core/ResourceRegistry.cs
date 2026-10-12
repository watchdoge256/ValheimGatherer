using System;
using System.Collections.Generic;
using ValheimGatherer.Configuration;

namespace ValheimGatherer.Core
{
    public class ResourceRegistry
    {
        private static readonly List<ResourceDefinition> Definitions = new List<ResourceDefinition>();

        static ResourceRegistry()
        {
            RegisterDefaults();
        }

        private static void RegisterDefaults()
        {
            // --- Mushrooms (Specific variants before base Mushroom) ---
            Register(new ResourceDefinition(
                "MushroomYellow", "Yellow Mushroom", "Yellow Mushroom",
                ResourceCategory.Mushrooms, PinIconType.Dot,
                new[] { "Pickable_Mushroom_yellow", "MushroomYellow" },
                new[] { "$item_mushroomyellow" },
                10.0f,
                cfg => cfg.TrackMushrooms.Value && cfg.MarkYellowMushroom.Value));

            Register(new ResourceDefinition(
                "MushroomBlue", "Blue Mushroom", "Blue Mushroom",
                ResourceCategory.Mushrooms, PinIconType.Dot,
                new[] { "Pickable_Mushroom_blue", "MushroomBlue" },
                new[] { "$item_mushroomblue" },
                10.0f,
                cfg => cfg.TrackMushrooms.Value && cfg.MarkBlueMushroom.Value));

            Register(new ResourceDefinition(
                "Magecap", "Magecap", "Magecap",
                ResourceCategory.Mushrooms, PinIconType.Dot,
                new[] { "Pickable_Magecap", "Magecap" },
                new[] { "$item_magecap" },
                10.0f,
                cfg => cfg.TrackMushrooms.Value && cfg.MarkMagecap.Value));

            Register(new ResourceDefinition(
                "JotunPuffs", "Jotun Puffs", "Jotun Puffs",
                ResourceCategory.Mushrooms, PinIconType.Dot,
                new[] { "Pickable_JotunPuffs", "JotunPuffs" },
                new[] { "$item_jotunpuffs" },
                10.0f,
                cfg => cfg.TrackMushrooms.Value && cfg.MarkJotunPuffs.Value));

            Register(new ResourceDefinition(
                "SmokePuff", "Smoke Puff", "Smoke Puff",
                ResourceCategory.Mushrooms, PinIconType.Dot,
                new[] { "Pickable_SmokePuff", "SmokePuff" },
                new[] { "$item_smokepuff" },
                10.0f,
                cfg => cfg.TrackMushrooms.Value && cfg.MarkSmokePuff.Value));

            Register(new ResourceDefinition(
                "Mushroom", "Mushroom", "Mushroom",
                ResourceCategory.Mushrooms, PinIconType.Dot,
                new[] { "Pickable_Mushroom", "Mushroom" },
                new[] { "$item_mushroom" },
                10.0f,
                cfg => cfg.TrackMushrooms.Value && cfg.MarkRedMushroom.Value));

            // --- Berries ---
            Register(new ResourceDefinition(
                "Raspberry", "Raspberries", "Raspberries",
                ResourceCategory.Berries, PinIconType.Dot,
                new[] { "Pickable_Raspberry", "Raspberry", "Raspberries" },
                new[] { "$item_raspberries" },
                10.0f,
                cfg => cfg.TrackBerries.Value && cfg.MarkRaspberry.Value));

            Register(new ResourceDefinition(
                "Blueberry", "Blueberries", "Blueberries",
                ResourceCategory.Berries, PinIconType.Dot,
                new[] { "Pickable_Blueberries", "Blueberries" },
                new[] { "$item_blueberries" },
                10.0f,
                cfg => cfg.TrackBerries.Value && cfg.MarkBlueberry.Value));

            Register(new ResourceDefinition(
                "Cloudberry", "Cloudberries", "Cloudberries",
                ResourceCategory.Berries, PinIconType.Dot,
                new[] { "Pickable_Cloudberry", "Cloudberry", "Cloudberries" },
                new[] { "$item_cloudberry" },
                10.0f,
                cfg => cfg.TrackBerries.Value && cfg.MarkCloudberry.Value));

            // --- Herbs & Plants ---
            Register(new ResourceDefinition(
                "Thistle", "Thistle", "Thistle",
                ResourceCategory.HerbsAndPlants, PinIconType.Dot,
                new[] { "Pickable_Thistle", "Thistle" },
                new[] { "$item_thistle" },
                10.0f,
                cfg => cfg.TrackHerbsAndPlants.Value && cfg.MarkThistle.Value));

            Register(new ResourceDefinition(
                "Dandelion", "Dandelion", "Dandelion",
                ResourceCategory.HerbsAndPlants, PinIconType.Dot,
                new[] { "Pickable_Dandelion", "Dandelion" },
                new[] { "$item_dandelion" },
                10.0f,
                cfg => cfg.TrackHerbsAndPlants.Value && cfg.MarkDandelion.Value));

            Register(new ResourceDefinition(
                "CarrotSeeds", "Carrot Seeds", "Carrot Seeds",
                ResourceCategory.HerbsAndPlants, PinIconType.Dot,
                new[] { "Pickable_SeedCarrot", "CarrotSeeds" },
                new[] { "$item_carrotseeds" },
                10.0f,
                cfg => cfg.TrackHerbsAndPlants.Value && cfg.MarkCarrotSeeds.Value));

            Register(new ResourceDefinition(
                "TurnipSeeds", "Turnip Seeds", "Turnip Seeds",
                ResourceCategory.HerbsAndPlants, PinIconType.Dot,
                new[] { "Pickable_SeedTurnip", "TurnipSeeds" },
                new[] { "$item_turnipseeds" },
                10.0f,
                cfg => cfg.TrackHerbsAndPlants.Value && cfg.MarkTurnipSeeds.Value));

            Register(new ResourceDefinition(
                "OnionSeeds", "Onion Seeds", "Onion Seeds",
                ResourceCategory.HerbsAndPlants, PinIconType.Dot,
                new[] { "Pickable_SeedOnion", "OnionSeeds" },
                new[] { "$item_onionseeds" },
                10.0f,
                cfg => cfg.TrackHerbsAndPlants.Value && cfg.MarkOnionSeeds.Value));

            Register(new ResourceDefinition(
                "Fiddlehead", "Fiddlehead", "Fiddlehead",
                ResourceCategory.HerbsAndPlants, PinIconType.Dot,
                new[] { "Pickable_Fiddlehead", "Fiddlehead" },
                new[] { "$item_fiddlehead" },
                10.0f,
                cfg => cfg.TrackHerbsAndPlants.Value && cfg.MarkFiddlehead.Value));

            // --- Farming Crops ---
            Register(new ResourceDefinition(
                "Barley", "Barley", "Barley",
                ResourceCategory.FarmingCrops, PinIconType.Dot,
                new[] { "Pickable_Barley", "Pickable_Barley_Wild", "Barley" },
                new[] { "$item_barley" },
                10.0f,
                cfg => cfg.TrackFarmingCrops.Value && cfg.MarkBarley.Value));

            Register(new ResourceDefinition(
                "Flax", "Flax", "Flax",
                ResourceCategory.FarmingCrops, PinIconType.Dot,
                new[] { "Pickable_Flax", "Pickable_Flax_Wild", "Flax" },
                new[] { "$item_flax" },
                10.0f,
                cfg => cfg.TrackFarmingCrops.Value && cfg.MarkFlax.Value));

            // --- Ores & Veins ---
            Register(new ResourceDefinition(
                "Copper", "Copper Deposit", "Copper",
                ResourceCategory.OresAndVeins, PinIconType.Hammer,
                new[] { "rock4_copper", "MineRock_Copper", "copper" },
                new[] { "$piece_deposit_copper" },
                20.0f,
                cfg => cfg.TrackOresAndVeins.Value && cfg.MarkCopper.Value));

            Register(new ResourceDefinition(
                "Tin", "Tin Deposit", "Tin",
                ResourceCategory.OresAndVeins, PinIconType.Hammer,
                new[] { "mine_rock_tin", "tin" },
                new[] { "$piece_deposit_tin" },
                15.0f,
                cfg => cfg.TrackOresAndVeins.Value && cfg.MarkTin.Value));

            Register(new ResourceDefinition(
                "Silver", "Silver Vein", "Silver",
                ResourceCategory.OresAndVeins, PinIconType.Hammer,
                new[] { "silvervein", "rock3_silver", "silver" },
                new[] { "$piece_deposit_silver" },
                20.0f,
                cfg => cfg.TrackOresAndVeins.Value && cfg.MarkSilver.Value));

            Register(new ResourceDefinition(
                "IronMudpile", "Muddy Scrap Pile", "Muddy Scrap Pile",
                ResourceCategory.OresAndVeins, PinIconType.Hammer,
                new[] { "mudpile", "mudpile2", "iron" },
                new[] { "$piece_mudpile" },
                15.0f,
                cfg => cfg.TrackOresAndVeins.Value && cfg.MarkIronMudpile.Value));

            Register(new ResourceDefinition(
                "Obsidian", "Obsidian Deposit", "Obsidian",
                ResourceCategory.OresAndVeins, PinIconType.Hammer,
                new[] { "MineRock_Obsidian", "rock4_forest_fracturing", "obsidian" },
                new[] { "$piece_deposit_obsidian" },
                15.0f,
                cfg => cfg.TrackOresAndVeins.Value && cfg.MarkObsidian.Value));

            Register(new ResourceDefinition(
                "Flametal", "Flametal Ore", "Flametal",
                ResourceCategory.OresAndVeins, PinIconType.Hammer,
                new[] { "MineRock_Meteorite", "flametal" },
                new[] { "$piece_deposit_flametal" },
                20.0f,
                cfg => cfg.TrackOresAndVeins.Value && cfg.MarkFlametal.Value));

            // --- Special POIs ---
            Register(new ResourceDefinition(
                "Beehive", "Wild Beehive", "Beehive",
                ResourceCategory.Beehives, PinIconType.Fireplace,
                new[] { "Beehive" },
                new[] { "$piece_beehive" },
                15.0f,
                cfg => cfg.TrackBeehives.Value && cfg.MarkBeehive.Value));

            Register(new ResourceDefinition(
                "Vegvisir", "Vegvisir", "Vegvisir",
                ResourceCategory.Special, PinIconType.Dot,
                new[] { "Vegvisir" },
                new[] { "$piece_vegvisir" },
                15.0f,
                cfg => cfg.TrackSpecial.Value && cfg.MarkVegvisir.Value));

            Register(new ResourceDefinition(
                "SurtlingSpawner", "Surtling Spawner", "Surtling Spawner",
                ResourceCategory.Special, PinIconType.Fireplace,
                new[] { "Spawner_FireHole", "FireFlies_FireHole" },
                new[] { "$piece_surtlingspawner", "$piece_firehole" },
                15.0f,
                cfg => cfg.TrackSpecial.Value && cfg.MarkSurtlingSpawner.Value));

            Register(new ResourceDefinition(
                "AncientRoot", "Ancient Root", "Ancient Root",
                ResourceCategory.Special, PinIconType.Dot,
                new[] { "ResourceRoot" },
                new[] { "$piece_resourceroot" },
                20.0f,
                cfg => cfg.TrackSpecial.Value && cfg.MarkAncientRoot.Value));
        }

        public static void Register(ResourceDefinition def)
        {
            Definitions.Add(def);
        }

        public static IReadOnlyList<ResourceDefinition> AllDefinitions => Definitions;

        public static ResourceDefinition? Match(string prefabName, string hoverName, string localizedName)
        {
            foreach (var def in Definitions)
            {
                if (def.Matches(prefabName, hoverName, localizedName))
                    return def;
            }
            return null;
        }

        public static bool IsEnabled(ResourceDefinition def, GathererConfig config, string? prefabName = null, string? localizedName = null)
        {
            if (!config.Enabled.Value)
                return false;

            // Check custom exclude filter first
            if (config.IsCustomExcluded(def.Id) ||
                config.IsCustomExcluded(def.DisplayName) ||
                config.IsCustomExcluded(def.CleanShortName) ||
                (!string.IsNullOrEmpty(prefabName) && config.IsCustomExcluded(prefabName)) ||
                (!string.IsNullOrEmpty(localizedName) && config.IsCustomExcluded(localizedName)))
            {
                return false;
            }

            // Check custom include filter (overrides category/resource toggles)
            if (config.IsCustomIncluded(def.Id) ||
                config.IsCustomIncluded(def.DisplayName) ||
                config.IsCustomIncluded(def.CleanShortName) ||
                (!string.IsNullOrEmpty(prefabName) && config.IsCustomIncluded(prefabName)) ||
                (!string.IsNullOrEmpty(localizedName) && config.IsCustomIncluded(localizedName)))
            {
                return true;
            }

            // Standard config check
            return def.IsEnabledFunc(config);
        }
    }
}
