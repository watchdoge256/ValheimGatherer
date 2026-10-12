using System;
using ValheimGatherer.Configuration;

namespace ValheimGatherer.Core
{
    public class ResourceDefinition
    {
        public string Id { get; }
        public string DisplayName { get; }
        public string CleanShortName { get; }
        public ResourceCategory Category { get; }
        public PinIconType DefaultIcon { get; }
        public string[] PrefabNames { get; }
        public string[] LocalizationKeys { get; }
        public float DefaultMergeRadius { get; }
        public Func<GathererConfig, bool> IsEnabledFunc { get; }

        public ResourceDefinition(
            string id,
            string displayName,
            string cleanShortName,
            ResourceCategory category,
            PinIconType defaultIcon,
            string[] prefabNames,
            string[] localizationKeys,
            float defaultMergeRadius,
            Func<GathererConfig, bool> isEnabledFunc)
        {
            Id = id;
            DisplayName = displayName;
            CleanShortName = cleanShortName;
            Category = category;
            DefaultIcon = defaultIcon;
            PrefabNames = prefabNames;
            LocalizationKeys = localizationKeys;
            DefaultMergeRadius = defaultMergeRadius;
            IsEnabledFunc = isEnabledFunc;
        }

        public bool Matches(string prefabName, string hoverName, string localizedName)
        {
            // 1. Check exact localization key first (most precise in Valheim)
            if (!string.IsNullOrEmpty(hoverName))
            {
                foreach (var key in LocalizationKeys)
                {
                    if (string.Equals(hoverName, key, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }

            // 2. Check exact prefab name
            if (!string.IsNullOrEmpty(prefabName))
            {
                foreach (var p in PrefabNames)
                {
                    if (string.Equals(prefabName, p, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }

            // 3. Check exact display or short name
            if (!string.IsNullOrEmpty(localizedName))
            {
                if (string.Equals(localizedName, DisplayName, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(localizedName, CleanShortName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            // 4. Check prefix with separator (e.g. "Vegvisir" matches "Vegvisir_Eikthyr")
            if (!string.IsNullOrEmpty(prefabName))
            {
                foreach (var p in PrefabNames)
                {
                    if (prefabName.StartsWith(p + "_", StringComparison.OrdinalIgnoreCase) ||
                        prefabName.StartsWith(p + "(", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
