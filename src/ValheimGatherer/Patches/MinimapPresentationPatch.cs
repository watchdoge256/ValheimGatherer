using HarmonyLib;
using ValheimGatherer.UI;

namespace ValheimGatherer.Patches
{
    [HarmonyPatch(typeof(Minimap), "UpdatePins")]
    public static class MinimapPresentationPatch
    {
        [HarmonyPrefix]
        public static void Prefix() => ResourcePinPresentation.Restore();

        [HarmonyPostfix]
        public static void Postfix(Minimap __instance) => ResourcePinPresentation.Apply(__instance);
    }

    [HarmonyPatch(typeof(Minimap), "OnDestroy")]
    public static class MinimapPresentationCleanupPatch
    {
        [HarmonyPrefix]
        public static void Prefix() => ResourcePinPresentation.Reset();
    }

    [HarmonyPatch(typeof(Localization), "SetupLanguage")]
    public static class ResourcePinLanguagePatch
    {
        [HarmonyPostfix]
        public static void Postfix() => ResourcePinPresentation.Reset();
    }
}
