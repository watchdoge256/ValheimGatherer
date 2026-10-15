using System;

namespace ValheimGatherer.Core
{
    public static class ResourcePinMatcher
    {
        public static ResourceDefinition? Match(string? name, Func<string, string>? localize = null, string prefix = "")
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            if (!string.IsNullOrEmpty(prefix) && name!.StartsWith(prefix, StringComparison.Ordinal))
                name = name.Substring(prefix.Length);

            ResourceDefinition? result = null;
            int longestMatch = 0;
            foreach (var resource in ResourceRegistry.AllDefinitions)
            {
                Check(resource, resource.Id);
                Check(resource, resource.DisplayName);
                Check(resource, resource.CleanShortName);
                foreach (var key in resource.LocalizationKeys)
                {
                    Check(resource, key);
                    if (localize != null) Check(resource, localize(key));
                }
            }
            return result;

            void Check(ResourceDefinition resource, string candidate)
            {
                if (string.IsNullOrEmpty(candidate) || candidate.Length <= longestMatch) return;
                if (!name!.EndsWith(candidate, StringComparison.OrdinalIgnoreCase)) return;
                int start = name.Length - candidate.Length;
                if (start != 0 && !char.IsWhiteSpace(name[start - 1]) && name[start - 1] != ']') return;
                longestMatch = candidate.Length;
                result = resource;
            }
        }
    }
}
