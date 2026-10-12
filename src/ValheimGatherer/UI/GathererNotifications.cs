using UnityEngine;

namespace ValheimGatherer.UI
{
    public static class GathererNotifications
    {
        public static void ShowMarked(string resourceName)
        {
            if (Player.m_localPlayer == null)
                return;

            string message = $"[Gatherer] Marked {resourceName}";
            Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, message);
        }
    }
}
