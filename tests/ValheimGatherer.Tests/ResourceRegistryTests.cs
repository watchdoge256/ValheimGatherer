using ValheimGatherer.Configuration;
using ValheimGatherer.Core;
using Xunit;

namespace ValheimGatherer.Tests
{
    public class ResourceRegistryTests
    {
        [Theory]
        [InlineData("Pickable_Mushroom", "$item_mushroom", "Mushroom", "Mushroom")]
        [InlineData("Pickable_Mushroom_yellow", "$item_mushroomyellow", "Yellow Mushroom", "MushroomYellow")]
        [InlineData("Pickable_Mushroom_blue", "$item_mushroomblue", "Blue Mushroom", "MushroomBlue")]
        [InlineData("Pickable_Raspberry", "$item_raspberries", "Raspberries", "Raspberry")]
        [InlineData("Pickable_Blueberries", "$item_blueberries", "Blueberries", "Blueberry")]
        [InlineData("Pickable_Cloudberry", "$item_cloudberry", "Cloudberries", "Cloudberry")]
        [InlineData("Pickable_Thistle", "$item_thistle", "Thistle", "Thistle")]
        [InlineData("Pickable_Dandelion", "$item_dandelion", "Dandelion", "Dandelion")]
        [InlineData("rock4_copper", "$piece_deposit_copper", "Copper Deposit", "Copper")]
        [InlineData("mine_rock_tin", "$piece_deposit_tin", "Tin Deposit", "Tin")]
        [InlineData("silvervein", "$piece_deposit_silver", "Silver Vein", "Silver")]
        [InlineData("mudpile", "$piece_mudpile", "Muddy Scrap Pile", "IronMudpile")]
        [InlineData("Beehive", "$piece_beehive", "Wild Beehive", "Beehive")]
        public void Match_KnownResources_ReturnsCorrectDefinition(
            string prefab, string hover, string localized, string expectedId)
        {
            var def = ResourceRegistry.Match(prefab, hover, localized);
            Assert.NotNull(def);
            Assert.Equal(expectedId, def.Id);
        }

        [Fact]
        public void Match_CaseInsensitive_MatchesCorrectly()
        {
            var def = ResourceRegistry.Match("pickable_mushroom", "", "");
            Assert.NotNull(def);
            Assert.Equal("Mushroom", def.Id);

            var copper = ResourceRegistry.Match("ROCK4_COPPER", "", "");
            Assert.NotNull(copper);
            Assert.Equal("Copper", copper.Id);
        }

        [Fact]
        public void Match_UnknownResource_ReturnsNull()
        {
            var def = ResourceRegistry.Match("RandomStonePiece", "A rock", "Stone");
            Assert.Null(def);
        }
    }
}
