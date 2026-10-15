using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using ValheimGatherer.Core;

namespace ValheimGatherer.UI
{
    public static class ResourcePinPresentation
    {
        private static readonly Dictionary<string, string> ItemNames = new Dictionary<string, string>
        {
            ["Mushroom"] = "Mushroom", ["MushroomYellow"] = "MushroomYellow",
            ["MushroomBlue"] = "MushroomBlue", ["Magecap"] = "MushroomMagecap",
            ["JotunPuffs"] = "MushroomJotunPuffs", ["SmokePuff"] = "MushroomSmokePuff",
            ["Raspberry"] = "Raspberry", ["Blueberry"] = "Blueberries", ["Cloudberry"] = "Cloudberry",
            ["Thistle"] = "Thistle", ["Dandelion"] = "Dandelion",
            ["CarrotSeeds"] = "CarrotSeeds", ["TurnipSeeds"] = "TurnipSeeds", ["OnionSeeds"] = "OnionSeeds",
            ["Fiddlehead"] = "Fiddleheadfern", ["Barley"] = "Barley", ["Flax"] = "Flax",
            ["Copper"] = "CopperOre", ["Tin"] = "TinOre", ["Silver"] = "SilverOre",
            ["IronMudpile"] = "IronScrap", ["Obsidian"] = "Obsidian", ["Flametal"] = "FlametalOreNew",
            ["Beehive"] = "QueenBee"
        };

        private static readonly AccessTools.FieldRef<Minimap, List<Minimap.PinData>> Pins =
            AccessTools.FieldRefAccess<Minimap, List<Minimap.PinData>>("m_pins");
        private static readonly Dictionary<string, ResourceDefinition?> Matches = new Dictionary<string, ResourceDefinition?>();
        private static readonly Dictionary<string, Sprite?> Icons = new Dictionary<string, Sprite?>();
        private static readonly List<VisualState> Changed = new List<VisualState>();
        private static ObjectDB? _database;
        private static string _prefix = "";

        private readonly struct VisualState
        {
            public readonly GameObject Marker;
            public readonly GameObject? Label;
            public readonly bool MarkerActive;
            public readonly bool LabelActive;
            public readonly Image Image;
            public readonly Sprite Sprite;

            public VisualState(Minimap.PinData pin)
            {
                Marker = pin.m_uiElement.gameObject;
                Label = pin.m_NamePinData?.PinNameGameObject;
                MarkerActive = Marker.activeSelf;
                LabelActive = Label != null && Label.activeSelf;
                Image = pin.m_iconElement;
                Sprite = Image.sprite;
            }

            public void Restore()
            {
                if (Marker != null) Marker.SetActive(MarkerActive);
                if (Label != null) Label.SetActive(LabelActive);
                if (Image != null) Image.sprite = Sprite;
            }
        }

        // Restore only presentation we changed; Valheim then applies its own zoom/type filters.
        public static void Restore()
        {
            foreach (var state in Changed) state.Restore();
            Changed.Clear();
        }

        public static void Reset()
        {
            Restore();
            Matches.Clear();
            Icons.Clear();
            _database = null;
            _prefix = "";
        }

        public static void Apply(Minimap map)
        {
            var config = GathererManager.Instance.Config;
            if (config == null || !config.Enabled.Value) return;
            if (_prefix != config.PinNamePrefix.Value)
            {
                _prefix = config.PinNamePrefix.Value;
                Matches.Clear();
            }
            if (_database != ObjectDB.instance)
            {
                _database = ObjectDB.instance;
                Icons.Clear();
            }

            foreach (var pin in Pins(map))
            {
                // Leave player, boss, death, event and other system markers alone.
                if ((pin.m_type != Minimap.PinType.Icon0 && pin.m_type != Minimap.PinType.Icon1 &&
                     pin.m_type != Minimap.PinType.Icon2 && pin.m_type != Minimap.PinType.Icon3 &&
                     pin.m_type != Minimap.PinType.Icon4) ||
                    pin.m_uiElement == null || pin.m_iconElement == null) continue;
                if (!Matches.TryGetValue(pin.m_name, out var resource))
                {
                    resource = ResourcePinMatcher.Match(pin.m_name,
                        key => Localization.instance != null ? Localization.instance.Localize(key) : key, _prefix);
                    // Bound the cache even when players continually rename pins.
                    if (Matches.Count >= 4096) Matches.Clear();
                    Matches[pin.m_name] = resource;
                }
                if (resource == null) continue;

                bool visible = !config.ResourceVisibility.TryGetValue(resource.Id, out var entry) || entry.Value;
                var icon = config.UseResourceIcons.Value ? GetIcon(resource) : null;
                if (visible && icon == null) continue;
                Changed.Add(new VisualState(pin));
                if (!visible)
                {
                    pin.m_uiElement.gameObject.SetActive(false);
                    if (pin.m_NamePinData?.PinNameGameObject != null)
                        pin.m_NamePinData.PinNameGameObject.SetActive(false);
                }
                else
                {
                    pin.m_iconElement.sprite = icon;
                }
            }
        }

        private static Sprite? GetIcon(ResourceDefinition resource)
        {
            if (_database == null) return null;
            if (Icons.TryGetValue(resource.Id, out var icon)) return icon;
            if (ItemNames.TryGetValue(resource.Id, out var itemName))
            {
                var prefab = _database.GetItemPrefab(itemName);
                var item = prefab != null ? prefab.GetComponent<ItemDrop>() : null;
                if (item != null && item.m_itemData.m_shared.m_icons.Length > 0)
                    icon = item.m_itemData.GetIcon();
            }
            Icons[resource.Id] = icon;
            return icon;
        }
    }
}
