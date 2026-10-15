using BepInEx.Configuration;
using ValheimGatherer.Configuration;
using ValheimGatherer.Core;

namespace ValheimGatherer.Tests
{
    public class ResourcePinDisplayTests
    {
        [Theory]
        [InlineData("Mushroom", "Mushroom")]
        [InlineData("Yellow Mushroom", "MushroomYellow")]
        [InlineData("Blue Mushroom", "MushroomBlue")]
        [InlineData("[G] Yellow Mushroom", "MushroomYellow")]
        [InlineData("[Clan]Copper Deposit", "Copper")]
        [InlineData("Alice Silver Vein", "Silver")]
        [InlineData("cOpPeR", "Copper")]
        [InlineData("$item_mushroomyellow", "MushroomYellow")]
        public void RecognizesNamesWithoutConfusingVariants(string name, string id)
        {
            Assert.Equal(id, ResourcePinMatcher.Match(name)?.Id);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Home")]
        [InlineData("NotCopper")]
        [InlineData("Copper storage")]
        public void LeavesUnrecognizedPinsAlone(string? name)
        {
            Assert.Null(ResourcePinMatcher.Match(name));
        }

        [Fact]
        public void RecognizesEveryRegisteredNameAndSharedPrefix()
        {
            foreach (var resource in ResourceRegistry.AllDefinitions)
            {
                Assert.Equal(resource.Id, ResourcePinMatcher.Match(resource.DisplayName)?.Id);
                Assert.Equal(resource.Id, ResourcePinMatcher.Match(resource.CleanShortName)?.Id);
                Assert.Equal(resource.Id, ResourcePinMatcher.Match("[Friend] " + resource.DisplayName)?.Id);
            }
        }

        [Fact]
        public void RecognizesLocalizedAndConfiguredPrefixNames()
        {
            Assert.Equal("Mushroom", ResourcePinMatcher.Match("[G] Grzyb",
                key => key == "$item_mushroom" ? "Grzyb" : key)?.Id);
            Assert.Equal("Copper", ResourcePinMatcher.Match("Gatherer:Copper", prefix: "Gatherer:")?.Id);
        }

        [Fact]
        public void VisibilityIsIndependentPersistentAndShownByDefault()
        {
            var path = Path.Combine(Path.GetTempPath(), $"Gatherer_{Guid.NewGuid()}.cfg");
            try
            {
                var file = new ConfigFile(path, true);
                var config = new GathererConfig(file);
                Assert.True(config.UseResourceIcons.Value);
                Assert.Equal(ResourceRegistry.AllDefinitions.Count, config.ResourceVisibility.Count);
                Assert.All(config.ResourceVisibility.Values, entry => Assert.True(entry.Value));
                Assert.False(config.MarkCopper.Value);
                config.TrackOresAndVeins.Value = true;
                config.MarkCopper.Value = true;
                config.ResourceVisibility["Copper"].Value = false;
                Assert.True(ResourceRegistry.IsEnabled(ResourceRegistry.Match("copper", "", "")!, config));
                Assert.True(config.ShareOnCartographyTable.Value);
                Assert.True(config.ResourceVisibility["Mushroom"].Value);
                file.Save();

                var reloaded = new GathererConfig(new ConfigFile(path, false));
                Assert.False(reloaded.ResourceVisibility["Copper"].Value);
                Assert.True(reloaded.MarkCopper.Value);
                Assert.True(reloaded.ShareOnCartographyTable.Value);
                foreach (var entry in config.ResourceVisibility.Values) entry.Value = false;
                Assert.All(config.ResourceVisibility.Values, entry => Assert.False(entry.Value));
                foreach (var entry in config.ResourceVisibility.Values) entry.Value = true;
                Assert.All(config.ResourceVisibility.Values, entry => Assert.True(entry.Value));
            }
            finally
            {
                if (File.Exists(path)) File.Delete(path);
            }
        }
    }
}
