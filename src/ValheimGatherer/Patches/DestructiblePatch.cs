using HarmonyLib;
using ValheimGatherer.Core;

namespace ValheimGatherer.Patches
{
    [HarmonyPatch(typeof(Destructible), nameof(Destructible.Damage))]
    public static class DestructiblePatch
    {
        [HarmonyPrefix]
        public static void Prefix(Destructible __instance, HitData hit)
        {
            if (__instance == null || hit == null)
                return;

            if (hit.GetAttacker() == Player.m_localPlayer)
            {
                GathererManager.Instance.TryProcessGameObject(__instance.gameObject);
            }
        }
    }
}
