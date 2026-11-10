using System.IO;
using BepInEx.Configuration;
using ValheimGatherer.Configuration;
using ValheimGatherer.Core;
using Xunit;

namespace ValheimGatherer.Tests
{
    public class ConfigurationTests
    {
        [Fact]
        public void ParseFilterString_HandlesDelimitersAndWhitespace()
        {
            string filter = " Copper,Tin; Silver ,  IronMudpile ";
            var parsed = GathererConfig.ParseFilterString(filter);

            Assert.Equal(4, parsed.Count);
            Assert.Contains("Copper", parsed);
            Assert.Contains("Tin", parsed);
            Assert.Contains("Silver", parsed);
            Assert.Contains("IronMudpile", parsed);
        }

        [Fact]
        public void ParseFilterString_NullOrEmpty_ReturnsEmptySet()
        {
            var empty = GathererConfig.ParseFilterString("");
            var nullFilter = GathererConfig.ParseFilterString(null);

            Assert.Empty(empty);
            Assert.Empty(nullFilter);
        }

        [Fact]
        public void Config_DefaultsAreDisabled_PlayerEnablesSelectively()
        {
            string tempConfigPath = Path.Combine(Path.GetTempPath(), $"Gatherer_Test_{Path.GetRandomFileName()}.cfg");
            try
            {
                var configFile = new ConfigFile(tempConfigPath, true);
                var config = new GathererConfig(configFile);

                // Default: all categories and resources are disabled by default
                var mushroomDef = ResourceRegistry.Match("Pickable_Mushroom", "", "");
                var copperDef = ResourceRegistry.Match("rock4_copper", "", "");

                Assert.NotNull(mushroomDef);
                Assert.NotNull(copperDef);

                Assert.False(ResourceRegistry.IsEnabled(mushroomDef, config));
                Assert.False(ResourceRegistry.IsEnabled(copperDef, config));

                // Player preference: "I would like to mark mushrooms but not copper"
                config.TrackMushrooms.Value = true;
                config.MarkRedMushroom.Value = true;

                Assert.True(ResourceRegistry.IsEnabled(mushroomDef, config));
                Assert.False(ResourceRegistry.IsEnabled(copperDef, config));
            }
            finally
            {
                if (File.Exists(tempConfigPath))
                    File.Delete(tempConfigPath);
            }
        }

        [Fact]
        public void Config_CustomExcludeFilter_OverridesResourceEnable()
        {
            string tempConfigPath = Path.Combine(Path.GetTempPath(), $"Gatherer_Test_{Path.GetRandomFileName()}.cfg");
            try
            {
                var configFile = new ConfigFile(tempConfigPath, true);
                var config = new GathererConfig(configFile);

                var copperDef = ResourceRegistry.Match("rock4_copper", "", "");
                Assert.NotNull(copperDef);

                // Enable ores category & copper
                config.TrackOresAndVeins.Value = true;
                config.MarkCopper.Value = true;
                Assert.True(ResourceRegistry.IsEnabled(copperDef, config));

                // Add Copper to custom exclude filter
                config.CustomExcludeFilter.Value = "Copper";

                Assert.False(ResourceRegistry.IsEnabled(copperDef, config));
            }
            finally
            {
                if (File.Exists(tempConfigPath))
                    File.Delete(tempConfigPath);
            }
        }

        [Fact]
        public void Config_CategoryToggle_DisablesAllInCategory()
        {
            string tempConfigPath = Path.Combine(Path.GetTempPath(), $"Gatherer_Test_{Path.GetRandomFileName()}.cfg");
            try
            {
                var configFile = new ConfigFile(tempConfigPath, true);
                var config = new GathererConfig(configFile);

                var copperDef = ResourceRegistry.Match("rock4_copper", "", "");
                var tinDef = ResourceRegistry.Match("mine_rock_tin", "", "");
                var silverDef = ResourceRegistry.Match("silvervein", "", "");

                Assert.NotNull(copperDef);
                Assert.NotNull(tinDef);
                Assert.NotNull(silverDef);

                // Enable all ores
                config.TrackOresAndVeins.Value = true;
                config.MarkCopper.Value = true;
                config.MarkTin.Value = true;
                config.MarkSilver.Value = true;

                Assert.True(ResourceRegistry.IsEnabled(copperDef!, config));
                Assert.True(ResourceRegistry.IsEnabled(tinDef!, config));
                Assert.True(ResourceRegistry.IsEnabled(silverDef!, config));

                // Disable entire ores category
                config.TrackOresAndVeins.Value = false;

                Assert.False(ResourceRegistry.IsEnabled(copperDef!, config));
                Assert.False(ResourceRegistry.IsEnabled(tinDef!, config));
                Assert.False(ResourceRegistry.IsEnabled(silverDef!, config));

                // Enable mushrooms
                config.TrackMushrooms.Value = true;
                config.MarkRedMushroom.Value = true;
                var mushroomDef = ResourceRegistry.Match("Pickable_Mushroom", "", "");
                Assert.NotNull(mushroomDef);
                Assert.True(ResourceRegistry.IsEnabled(mushroomDef!, config));
            }
            finally
            {
                if (File.Exists(tempConfigPath))
                    File.Delete(tempConfigPath);
            }
        }

        [Fact]
        public void Config_LingonberryRequiresCategoryAndResourceSelection()
        {
            string tempConfigPath = Path.Combine(Path.GetTempPath(), $"Gatherer_Test_{Path.GetRandomFileName()}.cfg");
            try
            {
                var config = new GathererConfig(new ConfigFile(tempConfigPath, true));
                var lingonberry = ResourceRegistry.Match("Pickable_Lingonberry", "$item_lingonberries", "Lingonberries");

                Assert.NotNull(lingonberry);
                Assert.False(config.TrackBerries.Value);
                Assert.False(config.MarkLingonberry.Value);
                Assert.False(ResourceRegistry.IsEnabled(lingonberry, config));

                config.MarkLingonberry.Value = true;
                Assert.False(ResourceRegistry.IsEnabled(lingonberry, config));

                config.TrackBerries.Value = true;
                Assert.True(ResourceRegistry.IsEnabled(lingonberry, config));

                config.ResourceVisibility["Lingonberry"].Value = false;
                Assert.True(ResourceRegistry.IsEnabled(lingonberry, config));
            }
            finally
            {
                if (File.Exists(tempConfigPath))
                    File.Delete(tempConfigPath);
            }
        }

        [Fact]
        public void Config_DeepNorthPlantingResourcesRequireCropCategory()
        {
            string tempConfigPath = Path.Combine(Path.GetTempPath(), $"Gatherer_Test_{Path.GetRandomFileName()}.cfg");
            try
            {
                var config = new GathererConfig(new ConfigFile(tempConfigPath, true));
                var kaleSeeds = ResourceRegistry.Match("Pickable_KaleSeeds", "$item_kaleseeds", "Kale Seeds");
                var oatSeeds = ResourceRegistry.Match("OatSeeds", "$item_oatseeds", "Oat Seeds");
                var timberwoodCone = ResourceRegistry.Match("FirConeFrost", "$item_firconefrost", "Timberwood Cone");

                Assert.NotNull(kaleSeeds);
                Assert.NotNull(oatSeeds);
                Assert.NotNull(timberwoodCone);
                Assert.False(ResourceRegistry.IsEnabled(kaleSeeds, config));
                Assert.False(ResourceRegistry.IsEnabled(oatSeeds, config));
                Assert.False(ResourceRegistry.IsEnabled(timberwoodCone, config));

                config.TrackFarmingCrops.Value = true;
                config.MarkKaleSeeds.Value = true;
                config.MarkOatSeeds.Value = true;
                config.MarkTimberwoodCone.Value = true;

                Assert.True(ResourceRegistry.IsEnabled(kaleSeeds, config));
                Assert.True(ResourceRegistry.IsEnabled(oatSeeds, config));
                Assert.True(ResourceRegistry.IsEnabled(timberwoodCone, config));
            }
            finally
            {
                if (File.Exists(tempConfigPath))
                    File.Delete(tempConfigPath);
            }
        }
    }
}
