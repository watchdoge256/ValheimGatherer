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
        public void Config_ToggleSpecificResource_MushroomVsCopper()
        {
            string tempConfigPath = Path.Combine(Path.GetTempPath(), $"Gatherer_Test_{Path.GetRandomFileName()}.cfg");
            try
            {
                var configFile = new ConfigFile(tempConfigPath, true);
                var config = new GathererConfig(configFile);

                // Default: both mushrooms and copper are true
                var mushroomDef = ResourceRegistry.Match("Pickable_Mushroom", "", "");
                var copperDef = ResourceRegistry.Match("rock4_copper", "", "");

                Assert.NotNull(mushroomDef);
                Assert.NotNull(copperDef);

                Assert.True(ResourceRegistry.IsEnabled(mushroomDef, config));
                Assert.True(ResourceRegistry.IsEnabled(copperDef, config));

                // User preference: "I would like to mark mushrooms but not copper"
                config.MarkCopper.Value = false;

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

                // Initially enabled
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

                Assert.True(ResourceRegistry.IsEnabled(copperDef!, config));
                Assert.True(ResourceRegistry.IsEnabled(tinDef!, config));
                Assert.True(ResourceRegistry.IsEnabled(silverDef!, config));

                // Disable entire ores category
                config.TrackOresAndVeins.Value = false;

                Assert.False(ResourceRegistry.IsEnabled(copperDef!, config));
                Assert.False(ResourceRegistry.IsEnabled(tinDef!, config));
                Assert.False(ResourceRegistry.IsEnabled(silverDef!, config));

                // Mushrooms should still be enabled
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
    }
}
