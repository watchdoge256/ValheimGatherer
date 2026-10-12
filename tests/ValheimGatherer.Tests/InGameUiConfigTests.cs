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
    }
}
