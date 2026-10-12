using System.IO;
using BepInEx.Configuration;
using UnityEngine;
using ValheimGatherer.Configuration;
using Xunit;

namespace ValheimGatherer.Tests
{
    public class InGameUiConfigTests
    {
        [Fact]
        public void ToggleUiKey_DefaultIsF8()
        {
            string tempConfigPath = Path.Combine(Path.GetTempPath(), $"Gatherer_Test_{Path.GetRandomFileName()}.cfg");
            try
            {
                var configFile = new ConfigFile(tempConfigPath, true);
                var config = new GathererConfig(configFile);

                Assert.Equal(KeyCode.F8, config.ToggleUiKey.Value);
            }
            finally
            {
                if (File.Exists(tempConfigPath))
                    File.Delete(tempConfigPath);
            }
        }

        [Fact]
        public void ToggleUiKey_CanBeRebound()
        {
            string tempConfigPath = Path.Combine(Path.GetTempPath(), $"Gatherer_Test_{Path.GetRandomFileName()}.cfg");
            try
            {
                var configFile = new ConfigFile(tempConfigPath, true);
                var config = new GathererConfig(configFile);

                config.ToggleUiKey.Value = KeyCode.F7;
                Assert.Equal(KeyCode.F7, config.ToggleUiKey.Value);
            }
            finally
            {
                if (File.Exists(tempConfigPath))
                    File.Delete(tempConfigPath);
            }
        }

        [Fact]
        public void AllCategoriesAndResources_DefaultToDisabled()
        {
            string tempConfigPath = Path.Combine(Path.GetTempPath(), $"Gatherer_Test_{Path.GetRandomFileName()}.cfg");
            try
            {
                var configFile = new ConfigFile(tempConfigPath, true);
                var config = new GathererConfig(configFile);

                // Categories must be disabled by default
                Assert.False(config.TrackMushrooms.Value);
                Assert.False(config.TrackBerries.Value);
                Assert.False(config.TrackHerbsAndPlants.Value);
                Assert.False(config.TrackFarmingCrops.Value);
                Assert.False(config.TrackOresAndVeins.Value);
                Assert.False(config.TrackBeehives.Value);
                Assert.False(config.TrackSpecial.Value);

                // Specific resources must be disabled by default
                Assert.False(config.MarkRedMushroom.Value);
                Assert.False(config.MarkYellowMushroom.Value);
                Assert.False(config.MarkBlueMushroom.Value);
                Assert.False(config.MarkRaspberry.Value);
                Assert.False(config.MarkBlueberry.Value);
                Assert.False(config.MarkCloudberry.Value);
                Assert.False(config.MarkThistle.Value);
                Assert.False(config.MarkCopper.Value);
                Assert.False(config.MarkTin.Value);
                Assert.False(config.MarkSilver.Value);
                Assert.False(config.MarkIronMudpile.Value);
                Assert.False(config.MarkBeehive.Value);
            }
            finally
            {
                if (File.Exists(tempConfigPath))
                    File.Delete(tempConfigPath);
            }
        }
    }
}
