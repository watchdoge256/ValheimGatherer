using HarmonyLib;
using ValheimGatherer.Core;

namespace ValheimGatherer.Patches
{
    [HarmonyPatch(typeof(ItemDrop), nameof(ItemDrop.Pickup), new[] { typeof(Humanoid) })]
    public static class ItemDropPatch
    {
        [HarmonyPrefix]
        public static void Prefix(ItemDrop __instance, Humanoid character)
        {
            if (__instance == null || character != Player.m_localPlayer)
                return;

            GathererManager.Instance.TryProcessGameObject(__instance.gameObject);
        }
    }
}
