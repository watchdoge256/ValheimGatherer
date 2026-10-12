using HarmonyLib;
using ValheimGatherer.Core;

namespace ValheimGatherer.Patches
{
    [HarmonyPatch(typeof(MineRock), nameof(MineRock.Damage))]
    public static class MineRockPatch
    {
        [HarmonyPrefix]
        public static void Prefix(MineRock __instance, HitData hit)
        {
            if (__instance == null || hit == null)
                return;

            if (hit.GetAttacker() == Player.m_localPlayer)
            {
                GathererManager.Instance.TryProcessGameObject(__instance.gameObject);
            }
        }
    }

    [HarmonyPatch(typeof(MineRock5), nameof(MineRock5.Damage))]
    public static class MineRock5Patch
    {
        [HarmonyPrefix]
        public static void Prefix(MineRock5 __instance, HitData hit)
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
