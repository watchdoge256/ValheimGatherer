using HarmonyLib;
using ValheimGatherer.Core;

namespace ValheimGatherer.Patches
{
    [HarmonyPatch(typeof(Pickable), nameof(Pickable.Interact))]
    public static class PickablePatch
    {
        [HarmonyPrefix]
        public static void Prefix(Pickable __instance, Humanoid character)
        {
            if (__instance == null || character == null)
                return;

            if (character != Player.m_localPlayer)
                return;

            GathererManager.Instance.TryProcessGameObject(__instance.gameObject);
        }
    }
}
