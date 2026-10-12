using System.IO;
using BepInEx.Configuration;
using ValheimGatherer.Configuration;
using ValheimGatherer.Core;
using Xunit;

namespace ValheimGatherer.Tests
{
    public class CartographySharingTests
    {
        [Fact]
        public void ShareOnCartographyTable_DefaultsToTrue()
        {
            string tempConfigPath = Path.Combine(Path.GetTempPath(), $"Gatherer_Test_{Path.GetRandomFileName()}.cfg");
            try
            {
                var configFile = new ConfigFile(tempConfigPath, true);
                var config = new GathererConfig(configFile);

                // Default must be true so Gatherer markings are shared via Cartography Table
                Assert.True(config.ShareOnCartographyTable.Value);
            }
            finally
            {
                if (File.Exists(tempConfigPath))
                    File.Delete(tempConfigPath);
            }
        }

        [Fact]
        public void ShareOnCartographyTable_CanBeDisabled()
        {
            string tempConfigPath = Path.Combine(Path.GetTempPath(), $"Gatherer_Test_{Path.GetRandomFileName()}.cfg");
            try
            {
                var configFile = new ConfigFile(tempConfigPath, true);
                var config = new GathererConfig(configFile);

                config.ShareOnCartographyTable.Value = false;
                Assert.False(config.ShareOnCartographyTable.Value);
            }
            finally
            {
                if (File.Exists(tempConfigPath))
                    File.Delete(tempConfigPath);
            }
        }

        [Fact]
        public void CustomInclude_OverridesDisabledCategory()
        {
            string tempConfigPath = Path.Combine(Path.GetTempPath(), $"Gatherer_Test_{Path.GetRandomFileName()}.cfg");
            try
            {
                var configFile = new ConfigFile(tempConfigPath, true);
                var config = new GathererConfig(configFile);

                // Disable all ores
                config.TrackOresAndVeins.Value = false;

                var copperDef = ResourceRegistry.Match("rock4_copper", "", "");
                Assert.NotNull(copperDef);
                Assert.False(ResourceRegistry.IsEnabled(copperDef!, config));

                // But explicitly include Copper in CustomIncludeFilter
                config.CustomIncludeFilter.Value = "Copper";
                Assert.True(ResourceRegistry.IsEnabled(copperDef!, config));
            }
            finally
            {
                if (File.Exists(tempConfigPath))
                    File.Delete(tempConfigPath);
            }
        }
    }
}
