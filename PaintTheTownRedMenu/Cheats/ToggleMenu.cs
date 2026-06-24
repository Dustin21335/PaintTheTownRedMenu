using HarmonyLib;
using Il2Cpp;
using Il2CppSteamworks;
using PaintTheTownRedMenu.Cheats.Core;
using UnityEngine;
using GUI = SharpGUI.GUI;
// ReSharper disable InconsistentNaming

namespace PaintTheTownRedMenu.Cheats
{
    [HarmonyPatch(typeof(GameManager))]
    public class ToggleMenu : ToggleCheat
    {
        public ToggleMenu() : base("Toggle Menu", KeyCode.Insert)
        {
            Enabled = true;
        }

        public class Settings
        {
            public Color MenuWatermarkColor = new(0.30f, 0.62f, 1.00f, 1.00f);
        }

        public Settings ToggleMenuSettings = new();

        private static bool _suppressHook;

        public override void OnStateChanged(bool state)
        {
            GUI.HandleInput = state;
            GUI.BlockInput = state;
        }

        public override void Update()
        {
            if (GameManager.Instance == null) return;
            _suppressHook = true;
            GameManager.Instance.OnGameOverlayActivated(new GameOverlayActivated_t
            {
                m_bActive = (byte)(Enabled ? 1 : 0),
            });
            _suppressHook = false;
        }


        [HarmonyPatch(nameof(GameManager.OnGameOverlayActivated)), HarmonyPrefix]
        public static bool OnGameOverlayActivated()
        {
            return _suppressHook || Instance<ToggleMenu>() is not { Enabled: true };
        }
    }
}