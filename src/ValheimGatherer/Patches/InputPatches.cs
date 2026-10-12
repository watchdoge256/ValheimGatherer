using HarmonyLib;
using UnityEngine;
using ValheimGatherer.UI;

namespace ValheimGatherer.Patches
{
    [HarmonyPatch(typeof(GameCamera), "UpdateMouseCapture")]
    public static class GameCameraMouseCapturePatch
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            if (GathererInGameUI.IsMenuOpen)
            {
                ZCursor.LockState = CursorLockMode.None;
                ZCursor.Show();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(PlayerController), "TakeInput", new[] { typeof(bool) })]
    public static class PlayerControllerTakeInputPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(ref bool __result)
        {
            if (GathererInGameUI.IsMenuOpen)
            {
                __result = false;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Player), "TakeInput")]
    public static class PlayerTakeInputPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(ref bool __result)
        {
            if (GathererInGameUI.IsMenuOpen)
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
