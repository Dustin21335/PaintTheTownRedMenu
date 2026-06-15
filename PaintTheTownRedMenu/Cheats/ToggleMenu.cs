using HarmonyLib;
using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;
using UnityEngine;
using GUI = SharpGUI.GUI;

namespace PaintTheTownRedMenu.Cheats
{
    [HarmonyPatch(typeof(GameManager))]
    public class ToggleMenu : ToggleCheat
    {
        public class Settings
        {
            public Color MenuTextColor = new Color(0.30f, 0.62f, 1.00f, 1.00f);
        }

        public Settings ToggleMenuSettings = new();

        public ToggleMenu() : base(KeyCode.Insert)
        {
            Enabled = true;
        }

        private bool _bypassPatch;

        public override string GetName()
        {
            return "Toggle Menu";
        }

        public override void OnStateChanged(bool state)
        {
            GUI.HandleInput = state;
            GUI.BlockInput = state;
            _bypassPatch = true;
            if (GameManager.Instance != null)
            {
                if (state) GameManager.Pause();
                else GameManager.UnPause();
            }
            _bypassPatch = false;
        }

        public override void Update()
        {
            if (!Enabled) return;
            if (GameManager.Instance == null) return;
            if (!GameManager.Paused) GameManager.Pause();
            if (!Cursor.visible) GameManager.Instance.SetCursorVisibility(true);
        }

        [HarmonyPatch(nameof(GameManager.Pause)), HarmonyPrefix]
        [HarmonyPatch(nameof(GameManager.UnPause))]
        public static bool PausePrefix()
        {
            ToggleMenu? toggleMenu = Instance<ToggleMenu>();
            return toggleMenu is not { Enabled: true } || toggleMenu._bypassPatch;
        }
    }
}