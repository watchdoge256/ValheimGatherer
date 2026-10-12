using HarmonyLib;
using ValheimGatherer.Configuration;
using ValheimGatherer.Core;

namespace ValheimGatherer.Patches
{
    [HarmonyPatch(typeof(Player), "UpdateHover")]
    public static class HoverPatch
    {
        private static float _lastHoverProcessTime;

        [HarmonyPostfix]
        public static void Postfix(Player __instance)
        {
            if (__instance == null || __instance != Player.m_localPlayer)
                return;

            var config = GathererManager.Instance.Config;
            if (config == null || !config.Enabled.Value)
                return;

            var mode = config.Mode.Value;
            if (mode != DiscoveryMode.InteractAndHover && mode != DiscoveryMode.All)
                return;

            // Throttle hover checks to twice per second
            if (UnityEngine.Time.time - _lastHoverProcessTime < 0.5f)
                return;

            var hoverObj = __instance.GetHoverObject();
            if (hoverObj != null)
            {
                _lastHoverProcessTime = UnityEngine.Time.time;
                GathererManager.Instance.TryProcessGameObject(hoverObj);
            }
        }
    }
}
