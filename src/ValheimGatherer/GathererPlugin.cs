using System.Reflection;
using BepInEx;
using HarmonyLib;
using ValheimGatherer.Configuration;
using ValheimGatherer.Core;
using ValheimGatherer.UI;

namespace ValheimGatherer
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class GathererPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = "com.watchdoge.valheim.gatherer";
        public const string PluginName = "Gatherer";
        public const string PluginVersion = "1.0.0";

        private Harmony? _harmony;
        public static GathererConfig? ModConfig { get; private set; }

        private void Awake()
        {
            ModConfig = new GathererConfig(Config);
            GathererManager.Instance.Initialize(ModConfig, Logger);

            gameObject.AddComponent<ProximityScanner>();
            GathererInGameUI.Attach(gameObject);

            _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGUID);

            Logger.LogInfo($"{PluginName} v{PluginVersion} loaded successfully.");
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }
    }
}
