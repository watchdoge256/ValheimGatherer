namespace ValheimGatherer.Configuration
{
    public enum ResourceCategory
    {
        Mushrooms,
        Berries,
        HerbsAndPlants,
        FarmingCrops,
        OresAndVeins,
        Beehives,
        Special,
        Custom
    }

    public enum DiscoveryMode
    {
        InteractOnly,
        InteractAndProximity,
        InteractAndHover,
        All
    }

    public enum PinIconType
    {
        Dot = 3,       // Minimap.PinType.Icon3
        Fireplace = 0, // Minimap.PinType.Icon0
        House = 1,     // Minimap.PinType.Icon1
        Hammer = 2,    // Minimap.PinType.Icon2
        Portal = 6     // Minimap.PinType.Icon4
    }
}
