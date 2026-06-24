using HarmonyLib;
using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;

namespace PaintTheTownRedMenu.Cheats
{
    [HarmonyPatch(typeof(GameManager))]
    public class CustomSteamRichPresence() : ToggleCheat("Custom Steam Rich Presence")
    {
        public class Settings
        {
            public string CustomPresence = "Paint The Town Red Menu";
        }

        public Settings CustomSteamRichPresenceSettings = new();

        private string? _currentPresence;
        private string? _lastScene;

        public override void OnEnable()
        {
            SetRichPresence(CustomSteamRichPresenceSettings.CustomPresence);
        }

        public override void OnDisable()
        {
            if (!string.IsNullOrEmpty(_lastScene)) GameManager.SetRichPresenceForNewLevel(_lastScene);
        }

        public override void OnSceneChanged(string sceneName)
        {
            _lastScene = sceneName;
        }

        public override void Update()
        {
            if (!Enabled) return;
            string customPresence = CustomSteamRichPresenceSettings.CustomPresence;
            if (_currentPresence == customPresence) return;
            SetRichPresence(customPresence);
        }

        private static void SetRichPresence(string value)
        {
            GameManager.SetRichPresenceKeyValue("levelName", value);
            GameManager.SetRichPresence("#StatusScenario");
        }

        [HarmonyPatch(nameof(GameManager.SetRichPresenceKeyValue)), HarmonyPrefix]
        public static void SetRichPresenceKeyValue(string key, string value)
        {
            if (key == "levelName") Instance<CustomSteamRichPresence>()?._currentPresence = value;
        }
    }
}
