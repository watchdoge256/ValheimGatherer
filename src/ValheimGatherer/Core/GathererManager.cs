using System;
using System.Collections.Generic;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using ValheimGatherer.Configuration;
using ValheimGatherer.UI;

namespace ValheimGatherer.Core
{
    public struct PinCheckData
    {
        public Vector3 Position;
        public string Name;

        public PinCheckData(Vector3 position, string name)
        {
            Position = position;
            Name = name;
        }
    }

    public class GathererManager
    {
        private static GathererManager? _instance;
        public static GathererManager Instance => _instance ??= new GathererManager();

        private static readonly AccessTools.FieldRef<Minimap, List<Minimap.PinData>> PinsFieldRef =
            AccessTools.FieldRefAccess<Minimap, List<Minimap.PinData>>("m_pins");

        public GathererConfig? Config { get; private set; }
        private ManualLogSource? _logger;

        public void Initialize(GathererConfig config, ManualLogSource logger)
        {
            Config = config;
            _logger = logger;
        }

        public bool TryProcessGameObject(GameObject? go, Vector3? explicitPos = null)
        {
            if (go == null || Config == null || !Config.Enabled.Value)
                return false;

            if (Minimap.instance == null)
                return false;

            // Check if player-built structure/crop to prevent marking player bases
            var piece = go.GetComponentInParent<Piece>() ?? go.GetComponent<Piece>();
            if (piece != null && piece.IsPlacedByPlayer())
            {
                return false;
            }

            // Extract prefab name (strip (Clone) suffix)
            string rawName = go.name;
            string prefabName = rawName.EndsWith("(Clone)", StringComparison.OrdinalIgnoreCase)
                ? rawName.Substring(0, rawName.Length - 7)
                : rawName;

            string hoverName = "";
            string itemPrefabName = "";

            // Check Pickable
            var pickable = go.GetComponentInParent<Pickable>() ?? go.GetComponent<Pickable>();
            if (pickable != null)
            {
                if (pickable.m_itemPrefab != null)
                {
                    itemPrefabName = pickable.m_itemPrefab.name;
                }
                hoverName = pickable.GetHoverName();
            }

            // Check MineRock / MineRock5
            if (string.IsNullOrEmpty(hoverName))
            {
                var mineRock = go.GetComponentInParent<MineRock>() ?? go.GetComponent<MineRock>();
                if (mineRock != null)
                {
                    hoverName = mineRock.m_name;
                }
            }

            if (string.IsNullOrEmpty(hoverName))
            {
                var mineRock5 = go.GetComponentInParent<MineRock5>() ?? go.GetComponent<MineRock5>();
                if (mineRock5 != null)
                {
                    hoverName = mineRock5.m_name;
                }
            }

            // Check Beehive
            if (string.IsNullOrEmpty(hoverName))
            {
                var beehive = go.GetComponentInParent<Beehive>() ?? go.GetComponent<Beehive>();
                if (beehive != null)
                {
                    hoverName = beehive.m_name;
                }
            }

            // Check general Hoverable
            if (string.IsNullOrEmpty(hoverName))
            {
                var hoverable = go.GetComponentInParent<Hoverable>() ?? go.GetComponent<Hoverable>();
                if (hoverable != null)
                {
                    hoverName = hoverable.GetHoverText();
                }
            }

            // Localize hover name
            string localizedName = hoverName;
            if (!string.IsNullOrEmpty(hoverName) && Localization.instance != null)
            {
                localizedName = Localization.instance.Localize(hoverName);
            }

            // Match with registry
            ResourceDefinition? def = null;
            if (!string.IsNullOrEmpty(itemPrefabName))
            {
                def = ResourceRegistry.Match(itemPrefabName, hoverName, localizedName);
            }
            if (def == null)
            {
                def = ResourceRegistry.Match(prefabName, hoverName, localizedName);
            }

            // Check custom includes if not in registry
            if (def == null)
            {
                if (Config.IsCustomIncluded(prefabName) ||
                    (!string.IsNullOrEmpty(itemPrefabName) && Config.IsCustomIncluded(itemPrefabName)) ||
                    (!string.IsNullOrEmpty(localizedName) && Config.IsCustomIncluded(localizedName)))
                {
                    string customName = !string.IsNullOrEmpty(localizedName) ? localizedName : prefabName;
                    def = new ResourceDefinition(
                        prefabName, customName, customName,
                        ResourceCategory.Custom, Config.DefaultPinIcon.Value,
                        new[] { prefabName }, new string[0],
                        Config.HerbMergeRadius.Value,
                        cfg => true);
                }
                else
                {
                    return false;
                }
            }

            // Check if resource is enabled
            if (!ResourceRegistry.IsEnabled(def, Config, prefabName, localizedName))
            {
                return false;
            }

            Vector3 pos = explicitPos ?? go.transform.position;

            // Determine radius & icon based on category
            float mergeRadius;
            PinIconType pinIcon;

            if (def.Category == ResourceCategory.OresAndVeins)
            {
                mergeRadius = Config.OreMergeRadius.Value;
                pinIcon = Config.OrePinIcon.Value;
            }
            else if (def.Category == ResourceCategory.Special || def.Category == ResourceCategory.Beehives)
            {
                mergeRadius = Config.SpecialMergeRadius.Value;
                pinIcon = Config.SpecialPinIcon.Value;
            }
            else
            {
                mergeRadius = Config.HerbMergeRadius.Value;
                pinIcon = Config.DefaultPinIcon.Value;
            }

            // Determine pin display name
            string baseName = Config.CleanPinNames.Value ? def.CleanShortName : def.DisplayName;
            if (!string.IsNullOrEmpty(localizedName) && !Config.CleanPinNames.Value)
            {
                baseName = localizedName;
            }

            string pinName = string.IsNullOrEmpty(Config.PinNamePrefix.Value)
                ? baseName
                : Config.PinNamePrefix.Value + baseName;

            // Check duplicate pins
            if (IsDuplicateExistingPin(pos, baseName, mergeRadius))
            {
                return false;
            }

            // Place pin
            bool save = Config.ShareOnCartographyTable.Value;
            long ownerId = Player.m_localPlayer != null ? Player.m_localPlayer.GetPlayerID() : 0L;
            var author = Splatform.PlatformUserID.None;

            var pin = Minimap.instance.AddPin(
                pos,
                (Minimap.PinType)(int)pinIcon,
                pinName,
                save,
                false,
                ownerId,
                author);

            if (pin != null)
            {
                _logger?.LogInfo($"[Gatherer] Added pin '{pinName}' at {pos} (shared via Cartography: {save})");

                if (Config.ShowHudNotification.Value)
                {
                    GathererNotifications.ShowMarked(baseName);
                }
                return true;
            }

            return false;
        }

        public bool IsDuplicateExistingPin(Vector3 pos, string resourceName, float radius)
        {
            if (Minimap.instance == null)
                return false;

            var pins = PinsFieldRef(Minimap.instance);
            if (pins == null)
                return false;

            float radiusSq = radius * radius;

            for (int i = 0; i < pins.Count; i++)
            {
                var pin = pins[i];
                if (pin == null) continue;

                float dx = pos.x - pin.m_pos.x;
                float dz = pos.z - pin.m_pos.z;

                if (dx * dx + dz * dz <= radiusSq)
                {
                    if (string.Equals(pin.m_name, resourceName, StringComparison.OrdinalIgnoreCase) ||
                        (!string.IsNullOrEmpty(pin.m_name) && pin.m_name.IndexOf(resourceName, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrEmpty(resourceName) && resourceName.IndexOf(pin.m_name, StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool CheckDuplicate(Vector3 pos, string resourceName, IEnumerable<PinCheckData> existingPins, float radius)
        {
            float radiusSq = radius * radius;
            foreach (var pin in existingPins)
            {
                float dx = pos.x - pin.Position.x;
                float dz = pos.z - pin.Position.z;

                if (dx * dx + dz * dz <= radiusSq)
                {
                    if (string.Equals(pin.Name, resourceName, StringComparison.OrdinalIgnoreCase) ||
                        (!string.IsNullOrEmpty(pin.Name) && pin.Name.IndexOf(resourceName, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrEmpty(resourceName) && resourceName.IndexOf(pin.Name, StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
