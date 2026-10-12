using System.Collections.Generic;
using UnityEngine;
using ValheimGatherer.Core;
using Xunit;

namespace ValheimGatherer.Tests
{
    public class DuplicatePreventionTests
    {
        [Fact]
        public void CheckDuplicate_WithinRadius_ReturnsTrue()
        {
            var existingPins = new List<PinCheckData>
            {
                new PinCheckData(new Vector3(100f, 10f, 100f), "Mushroom")
            };

            // Query at 5 meters distance (merge radius = 10m)
            Vector3 closePos = new Vector3(103f, 10f, 104f);
            bool isDup = GathererManager.CheckDuplicate(closePos, "Mushroom", existingPins, 10.0f);

            Assert.True(isDup);
        }

        [Fact]
        public void CheckDuplicate_OutsideRadius_ReturnsFalse()
        {
            var existingPins = new List<PinCheckData>
            {
                new PinCheckData(new Vector3(100f, 10f, 100f), "Mushroom")
            };

            // Query at 25 meters distance (merge radius = 10m)
            Vector3 farPos = new Vector3(125f, 10f, 100f);
            bool isDup = GathererManager.CheckDuplicate(farPos, "Mushroom", existingPins, 10.0f);

            Assert.False(isDup);
        }

        [Fact]
        public void CheckDuplicate_DifferentResourceWithinRadius_ReturnsFalse()
        {
            var existingPins = new List<PinCheckData>
            {
                new PinCheckData(new Vector3(100f, 10f, 100f), "Copper")
            };

            // Query at same location for Tin
            Vector3 pos = new Vector3(100f, 10f, 100f);
            bool isDup = GathererManager.CheckDuplicate(pos, "Tin", existingPins, 20.0f);

            Assert.False(isDup);
        }

        [Fact]
        public void CheckDuplicate_PrefixedPin_ReturnsTrue()
        {
            var existingPins = new List<PinCheckData>
            {
                new PinCheckData(new Vector3(50f, 0f, 50f), "[G] Mushroom")
            };

            // Query without prefix
            Vector3 pos = new Vector3(52f, 0f, 51f);
            bool isDup = GathererManager.CheckDuplicate(pos, "Mushroom", existingPins, 10.0f);

            Assert.True(isDup);
        }
    }
}
