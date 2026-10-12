using HarmonyLib;
using ValheimGatherer.Core;

namespace ValheimGatherer.Patches
{
    [HarmonyPatch(typeof(MapTable), "OnWrite")]
    public static class MapTableOnWritePatch
    {
        [HarmonyPostfix]
        public static void Postfix(MapTable __instance, Switch caller, Humanoid user, ItemDrop.ItemData item)
        {
            if (user != Player.m_localPlayer)
                return;

            var config = GathererManager.Instance.Config;
            if (config != null && config.ShareOnCartographyTable.Value && config.ShowHudNotification.Value)
            {
                Player.m_localPlayer.Message(
                    MessageHud.MessageType.TopLeft,
                    "[Gatherer] Map discoveries and resource pins recorded to Cartography Table!");
            }
        }
    }

    [HarmonyPatch(typeof(MapTable), "OnRead", new[] { typeof(Switch), typeof(Humanoid), typeof(ItemDrop.ItemData), typeof(bool) })]
    public static class MapTableOnReadPatch
    {
        [HarmonyPostfix]
        public static void Postfix(MapTable __instance, Switch caller, Humanoid user, ItemDrop.ItemData item, bool showMessage)
        {
            if (user != Player.m_localPlayer || !showMessage)
                return;

            var config = GathererManager.Instance.Config;
            if (config != null && config.ShareOnCartographyTable.Value && config.ShowHudNotification.Value)
            {
                Player.m_localPlayer.Message(
                    MessageHud.MessageType.TopLeft,
                    "[Gatherer] Synchronized map discoveries and resource pins from Cartography Table!");
            }
        }
    }
}
