using System;
using System.Collections.Generic;
using UnityEngine;
using ValheimGatherer.Configuration;
using ValheimGatherer.Core;

namespace ValheimGatherer.UI
{
    public class GathererInGameUI : MonoBehaviour
    {
        private static GathererInGameUI? _instance;
        public static GathererInGameUI? Instance => _instance;

        private bool _isOpen = false;
        private Rect _windowRect = new Rect(80, 80, 720, 520);
        private Vector2 _scrollPos = Vector2.zero;
        private int _activeTab = 0; // 0 = Categories & Resources, 1 = Settings, 2 = Filters

        private GUIStyle? _winStyle;
        private GUIStyle? _headerStyle;
        private GUIStyle? _tabStyle;
        private GUIStyle? _tabActiveStyle;
        private GUIStyle? _sectionStyle;
        private GUIStyle? _toggleStyle;
        private GUIStyle? _labelStyle;
        private GUIStyle? _btnStyle;

        private Texture2D? _bgTexture;
        private Texture2D? _tabActiveTexture;

        public static void Attach(GameObject root)
        {
            if (_instance == null)
            {
                _instance = root.AddComponent<GathererInGameUI>();
            }
        }

        private void Awake()
        {
            _instance = this;
            // Center window initially
            _windowRect = new Rect(
                Mathf.Max(20, (Screen.width - 720) / 2),
                Mathf.Max(20, (Screen.height - 520) / 2),
                720, 520);
        }

        private void Update()
        {
            var config = GathererManager.Instance.Config;
            if (config == null) return;

            if (Input.GetKeyDown(config.ToggleUiKey.Value))
            {
                ToggleMenu();
            }

            // Close with Escape if open
            if (_isOpen && Input.GetKeyDown(KeyCode.Escape))
            {
                _isOpen = false;
                TakeInputFocus(false);
            }
        }

        public void ToggleMenu()
        {
            _isOpen = !_isOpen;
            TakeInputFocus(_isOpen);
        }

        private void TakeInputFocus(bool focus)
        {
            if (focus)
            {
                // Unlock cursor so the player can interact with the menu
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                // Restore game camera control if no other game menu is open
                if (Player.m_localPlayer != null && !Menu.IsVisible() && !InventoryGui.IsVisible())
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
        }

        private void InitStyles()
        {
            if (_winStyle != null) return;

            _bgTexture = MakeSolidTexture(1, 1, new Color(0.12f, 0.14f, 0.17f, 0.96f));
            _tabActiveTexture = MakeSolidTexture(1, 1, new Color(0.24f, 0.38f, 0.52f, 1f));

            _winStyle = new GUIStyle(GUI.skin.window);
            _winStyle.normal.background = _bgTexture;
            _winStyle.normal.textColor = new Color(0.95f, 0.85f, 0.55f);
            _winStyle.fontSize = 15;
            _winStyle.fontStyle = FontStyle.Bold;
            _winStyle.alignment = TextAnchor.UpperCenter;
            _winStyle.padding = new RectOffset(14, 14, 28, 14);

            _headerStyle = new GUIStyle(GUI.skin.label);
            _headerStyle.fontSize = 13;
            _headerStyle.fontStyle = FontStyle.Bold;
            _headerStyle.normal.textColor = new Color(0.95f, 0.85f, 0.55f);
            _headerStyle.margin = new RectOffset(0, 0, 10, 4);

            _sectionStyle = new GUIStyle(GUI.skin.label);
            _sectionStyle.fontSize = 12;
            _sectionStyle.fontStyle = FontStyle.Bold;
            _sectionStyle.normal.textColor = new Color(0.55f, 0.85f, 0.95f);
            _sectionStyle.margin = new RectOffset(0, 0, 8, 2);

            _tabStyle = new GUIStyle(GUI.skin.button);
            _tabStyle.fontSize = 12;
            _tabStyle.normal.textColor = Color.white;
            _tabStyle.fixedHeight = 28;

            _tabActiveStyle = new GUIStyle(_tabStyle);
            _tabActiveStyle.normal.background = _tabActiveTexture;
            _tabActiveStyle.fontStyle = FontStyle.Bold;
            _tabActiveStyle.normal.textColor = Color.yellow;

            _toggleStyle = new GUIStyle(GUI.skin.toggle);
            _toggleStyle.fontSize = 12;
            _toggleStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f);
            _toggleStyle.onNormal.textColor = Color.white;

            _labelStyle = new GUIStyle(GUI.skin.label);
            _labelStyle.fontSize = 11;
            _labelStyle.normal.textColor = new Color(0.8f, 0.8f, 0.8f);

            _btnStyle = new GUIStyle(GUI.skin.button);
            _btnStyle.fontSize = 12;
            _btnStyle.fontStyle = FontStyle.Bold;
        }

        private Texture2D MakeSolidTexture(int width, int height, Color col)
        {
            var pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
                pix[i] = col;
            var result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        private void OnGUI()
        {
            if (!_isOpen) return;

            InitStyles();

            _windowRect = GUI.Window(
                9928374,
                _windowRect,
                DrawWindowContents,
                "Gatherer - Resource Tracking Menu",
                _winStyle);
        }

        private void DrawWindowContents(int windowId)
        {
            var config = GathererManager.Instance.Config;
            if (config == null) return;

            GUILayout.BeginVertical();

            // Tabs bar
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Resources & Categories", _activeTab == 0 ? _tabActiveStyle : _tabStyle)) _activeTab = 0;
            if (GUILayout.Button("General & Pin Settings", _activeTab == 1 ? _tabActiveStyle : _tabStyle)) _activeTab = 1;
            if (GUILayout.Button("Custom Filters & Radar", _activeTab == 2 ? _tabActiveStyle : _tabStyle)) _activeTab = 2;
            if (GUILayout.Button("Close (Esc)", _btnStyle, GUILayout.Width(90)))
            {
                _isOpen = false;
                TakeInputFocus(false);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(8);

            _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUILayout.ExpandHeight(true));

            if (_activeTab == 0)
            {
                DrawResourcesTab(config);
            }
            else if (_activeTab == 1)
            {
                DrawGeneralSettingsTab(config);
            }
            else
            {
                DrawFiltersTab(config);
            }

            GUILayout.EndScrollView();

            GUILayout.Space(6);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Press " + config.ToggleUiKey.Value + " or Esc to toggle this menu anytime in-game.", _labelStyle);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Select All", GUILayout.Width(90))) SetAllResources(config, true);
            if (GUILayout.Button("Deselect All", GUILayout.Width(90))) SetAllResources(config, false);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0, 0, 10000, 24));
        }

        private void DrawResourcesTab(GathererConfig config)
        {
            GUILayout.Label("Categories & Specific Resource Toggles", _headerStyle);
            GUILayout.Label("Toggle entire categories or individual resources below. Changes apply instantly in real-time.", _labelStyle);

            GUILayout.Space(6);

            // Mushrooms Section
            DrawCategoryHeader("Mushrooms", config.TrackMushrooms);
            if (config.TrackMushrooms.Value)
            {
                GUILayout.BeginHorizontal();
                DrawToggle(config.MarkRedMushroom, "Red Mushroom");
                DrawToggle(config.MarkYellowMushroom, "Yellow Mushroom");
                DrawToggle(config.MarkBlueMushroom, "Blue Mushroom");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                DrawToggle(config.MarkMagecap, "Magecap (Mistlands)");
                DrawToggle(config.MarkJotunPuffs, "Jotun Puffs (Mistlands)");
                DrawToggle(config.MarkSmokePuff, "Smoke Puff (Ashlands)");
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(6);

            // Berries Section
            DrawCategoryHeader("Berries", config.TrackBerries);
            if (config.TrackBerries.Value)
            {
                GUILayout.BeginHorizontal();
                DrawToggle(config.MarkRaspberry, "Raspberries");
                DrawToggle(config.MarkBlueberry, "Blueberries");
                DrawToggle(config.MarkCloudberry, "Cloudberries");
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(6);

            // Herbs & Plants Section
            DrawCategoryHeader("Herbs & Wild Plants", config.TrackHerbsAndPlants);
            if (config.TrackHerbsAndPlants.Value)
            {
                GUILayout.BeginHorizontal();
                DrawToggle(config.MarkThistle, "Thistle");
                DrawToggle(config.MarkDandelion, "Dandelion");
                DrawToggle(config.MarkCarrotSeeds, "Carrot Seeds");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                DrawToggle(config.MarkTurnipSeeds, "Turnip Seeds");
                DrawToggle(config.MarkOnionSeeds, "Onion Seeds");
                DrawToggle(config.MarkFiddlehead, "Fiddlehead (Ashlands)");
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(6);

            // Farming Crops Section
            DrawCategoryHeader("Farming Crops (Wild)", config.TrackFarmingCrops);
            if (config.TrackFarmingCrops.Value)
            {
                GUILayout.BeginHorizontal();
                DrawToggle(config.MarkBarley, "Barley");
                DrawToggle(config.MarkFlax, "Flax");
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(6);

            // Ores & Veins Section
            DrawCategoryHeader("Ores & Mineral Veins", config.TrackOresAndVeins);
            if (config.TrackOresAndVeins.Value)
            {
                GUILayout.BeginHorizontal();
                DrawToggle(config.MarkCopper, "Copper Deposit");
                DrawToggle(config.MarkTin, "Tin Deposit");
                DrawToggle(config.MarkSilver, "Silver Vein");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                DrawToggle(config.MarkIronMudpile, "Muddy Scrap Pile (Iron)");
                DrawToggle(config.MarkObsidian, "Obsidian");
                DrawToggle(config.MarkFlametal, "Flametal (Ashlands)");
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(6);

            // Special POIs Section
            DrawCategoryHeader("Special POIs & Beehives", config.TrackSpecial);
            if (config.TrackSpecial.Value)
            {
                GUILayout.BeginHorizontal();
                DrawToggle(config.MarkBeehive, "Wild Beehives");
                DrawToggle(config.MarkVegvisir, "Vegvisir Runestones");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                DrawToggle(config.MarkSurtlingSpawner, "Surtling Spawners");
                DrawToggle(config.MarkAncientRoot, "Ancient Roots (Sap)");
                GUILayout.EndHorizontal();
            }
        }

        private void DrawGeneralSettingsTab(GathererConfig config)
        {
            GUILayout.Label("General & Cartography Table Settings", _headerStyle);

            config.Enabled.Value = GUILayout.Toggle(config.Enabled.Value, " Mod Enabled (Master Toggle)", _toggleStyle);
            config.ShareOnCartographyTable.Value = GUILayout.Toggle(
                config.ShareOnCartographyTable.Value,
                " Share On Cartography Table (Synchronize pins with other players via Cartography Table)",
                _toggleStyle);
            config.ShowHudNotification.Value = GUILayout.Toggle(
                config.ShowHudNotification.Value,
                " Show HUD Notifications (Top-left on-screen message when a node is marked)",
                _toggleStyle);
            config.CleanPinNames.Value = GUILayout.Toggle(
                config.CleanPinNames.Value,
                " Clean Pin Names (e.g. 'Copper' instead of 'Copper deposit')",
                _toggleStyle);

            GUILayout.Space(10);
            GUILayout.Label("Discovery Mode", _sectionStyle);
            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(config.Mode.Value == DiscoveryMode.InteractAndProximity, " Interact & Proximity Radar (Default)", _toggleStyle))
                config.Mode.Value = DiscoveryMode.InteractAndProximity;
            if (GUILayout.Toggle(config.Mode.Value == DiscoveryMode.InteractOnly, " Interact Only (Classic WoW)", _toggleStyle))
                config.Mode.Value = DiscoveryMode.InteractOnly;
            if (GUILayout.Toggle(config.Mode.Value == DiscoveryMode.InteractAndHover, " Interact & Hover", _toggleStyle))
                config.Mode.Value = DiscoveryMode.InteractAndHover;
            if (GUILayout.Toggle(config.Mode.Value == DiscoveryMode.All, " All Triggers", _toggleStyle))
                config.Mode.Value = DiscoveryMode.All;
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.Label("Pin Label Prefix", _sectionStyle);
            config.PinNamePrefix.Value = GUILayout.TextField(config.PinNamePrefix.Value, GUILayout.Width(200));
            GUILayout.Label("Optional prefix added before names on the map (e.g. '[G] ' or empty).", _labelStyle);

            GUILayout.Space(10);
            GUILayout.Label("Duplicate Merging Distances (Radius in Meters)", _sectionStyle);
            GUILayout.BeginHorizontal();
            GUILayout.Label($"Herb/Mushroom Radius: {config.HerbMergeRadius.Value:F0}m", GUILayout.Width(170));
            config.HerbMergeRadius.Value = GUILayout.HorizontalSlider(config.HerbMergeRadius.Value, 2f, 30f, GUILayout.Width(150));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"Ore Deposit Radius: {config.OreMergeRadius.Value:F0}m", GUILayout.Width(170));
            config.OreMergeRadius.Value = GUILayout.HorizontalSlider(config.OreMergeRadius.Value, 5f, 50f, GUILayout.Width(150));
            GUILayout.EndHorizontal();
        }

        private void DrawFiltersTab(GathererConfig config)
        {
            GUILayout.Label("Advanced Filters & Radar Tuning", _headerStyle);

            GUILayout.Label("Proximity Radar Sweep Radius", _sectionStyle);
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{config.ProximityRadius.Value:F0} meters", GUILayout.Width(100));
            config.ProximityRadius.Value = GUILayout.HorizontalSlider(config.ProximityRadius.Value, 5f, 40f, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.Label("Custom Exclude Filter (Blacklist)", _sectionStyle);
            config.CustomExcludeFilter.Value = GUILayout.TextField(config.CustomExcludeFilter.Value);
            GUILayout.Label("Comma-separated prefab or display names to NEVER mark (e.g. 'Copper, Flint, Stone').", _labelStyle);

            GUILayout.Space(10);
            GUILayout.Label("Custom Include Filter (Whitelist / Modded items)", _sectionStyle);
            config.CustomIncludeFilter.Value = GUILayout.TextField(config.CustomIncludeFilter.Value);
            GUILayout.Label("Comma-separated prefab names to ALWAYS mark even from other mods (e.g. 'DragonEgg, GoblinTotem').", _labelStyle);
        }

        private void DrawCategoryHeader(string title, BepInEx.Configuration.ConfigEntry<bool> entry)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title, _sectionStyle);
            GUILayout.FlexibleSpace();
            entry.Value = GUILayout.Toggle(entry.Value, entry.Value ? "Category Enabled" : "Category Disabled", _toggleStyle);
            GUILayout.EndHorizontal();
        }

        private void DrawToggle(BepInEx.Configuration.ConfigEntry<bool> entry, string label)
        {
            entry.Value = GUILayout.Toggle(entry.Value, " " + label, _toggleStyle, GUILayout.Width(210));
        }

        private void SetAllResources(GathererConfig config, bool val)
        {
            config.MarkRedMushroom.Value = val;
            config.MarkYellowMushroom.Value = val;
            config.MarkBlueMushroom.Value = val;
            config.MarkMagecap.Value = val;
            config.MarkJotunPuffs.Value = val;
            config.MarkSmokePuff.Value = val;

            config.MarkRaspberry.Value = val;
            config.MarkBlueberry.Value = val;
            config.MarkCloudberry.Value = val;

            config.MarkThistle.Value = val;
            config.MarkDandelion.Value = val;
            config.MarkCarrotSeeds.Value = val;
            config.MarkTurnipSeeds.Value = val;
            config.MarkOnionSeeds.Value = val;
            config.MarkFiddlehead.Value = val;

            config.MarkBarley.Value = val;
            config.MarkFlax.Value = val;

            config.MarkCopper.Value = val;
            config.MarkTin.Value = val;
            config.MarkSilver.Value = val;
            config.MarkIronMudpile.Value = val;
            config.MarkObsidian.Value = val;
            config.MarkFlametal.Value = val;

            config.MarkBeehive.Value = val;
            config.MarkVegvisir.Value = val;
            config.MarkSurtlingSpawner.Value = val;
            config.MarkAncientRoot.Value = val;
        }

        private void OnDestroy()
        {
            if (_bgTexture != null) Destroy(_bgTexture);
            if (_tabActiveTexture != null) Destroy(_tabActiveTexture);
        }
    }
}
