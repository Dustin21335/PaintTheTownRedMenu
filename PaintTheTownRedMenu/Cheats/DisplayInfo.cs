using ImGuiNET;
using PaintTheTownRedMenu.Cheats.Core;
using PaintTheTownRedMenu.Utils;
using System.Linq;
using UnityEngine;

namespace PaintTheTownRedMenu.Cheats
{
    public class DisplayInfo : ToggleCheat
    {
        public class Settings
        {
            public bool DisplayEnemiesLeft = true;
            public bool DisplayWeaponsLeft = true;
            public bool AutoResizeWindow = true;
        }

        public Settings DisplayInfoSettings = new();

        public override string GetName()
        {
            return "Display Info";
        }

        public override void OnGUI()
        {
            if (!Enabled) return;
            UIUtil.Area("Display Info", () =>
            {
                if (DisplayInfoSettings.DisplayEnemiesLeft) UIUtil.Text($"Enemies Left: {GameObjectManager.Enemies.Count(e => e != null && !e.IsDead())}");
                if (DisplayInfoSettings.DisplayWeaponsLeft) UIUtil.Text($"Weapons Left: {GameObjectManager.Weapons.Count(w => w != null && w is { hasBroken: false, heldByThisPlayer: false })}");
            }, new Vector2(200, 150), Vector2.zero, ImGuiWindowFlags.NoCollapse | (DisplayInfoSettings.AutoResizeWindow ? ImGuiWindowFlags.AlwaysAutoResize : 0), ImGuiCond.FirstUseEver);
        }
    }
}
