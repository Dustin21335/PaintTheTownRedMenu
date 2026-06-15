using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;
using PaintTheTownRedMenu.Utils;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace PaintTheTownRedMenu.Cheats
{
    public class ESP : ToggleCheat
    {
        public class ESPDisplaySettings
        {
            public bool Enabled = true;
            public bool ShowName = true;
            public bool ShowDistance = true;
            public Color Color = Color.white;
        }

        public class EnemySettings : ESPDisplaySettings
        {
            public bool ShowHealth = true;
            public bool ShowType = true;

            public EnemySettings()
            {
                Color = Color.red;
            }
        }

        public class WeaponSettings : ESPDisplaySettings
        {
            public bool Guns = true;
            public bool Melees = true;
            public bool Shields = true;
            public bool IgnoreHeld = true;
            public bool ShowHeld = false;
            public bool ShowType = true;

            public WeaponSettings()
            {
                Color = Color.green;
            }
        }

        public class ButtonSettings : ESPDisplaySettings
        {
            public bool IgnorePressed = true;
            public bool ShowPressedState = false;

            public ButtonSettings()
            {
                Enabled = false;
                Color = Color.magenta;
            }
        }

        public class Settings
        {
            public float MaxRange = 2000f;
            public EnemySettings Enemies = new();
            public WeaponSettings Weapons = new();
            public ButtonSettings Buttons = new();
        }

        public Settings ESPSettings = new();

        public override string GetName()
        {
            return "ESP";
        }

        public override void OnGUI()
        {
            if (!Enabled) return;
            DisplayEnemies();
            DisplayWeapons();
            DisplayButtons();
        }

        private void DisplayEnemies()
        {
            EnemySettings ESPEnemySettings = ESPSettings.Enemies;
            if (!ESPEnemySettings.Enabled) return;
            foreach (Enemy enemy in GameObjectManager.Enemies.Where(e => e != null && !e.IsDead()).ToList())
            {
                DrawESP(enemy, ESPEnemySettings.Color, (screen, color, distance) =>
                {
                    float offset = 0f;
                    if (ESPEnemySettings.ShowName) DrawText("Enemy", color, screen, ref offset);
                    if (ESPEnemySettings.ShowHealth) DrawText($"[HP {enemy.health * 100f:F0}]", color, screen, ref offset);
                    if (ESPEnemySettings.ShowType) DrawText($"[{Regex.Replace(enemy.thisEnemyType.ToString(), "(\\B[A-Z])", " $1")}]", color, screen, ref offset);
                    if (ESPEnemySettings.ShowDistance) DrawText($"[{distance:F2}m]", color, screen, ref offset);
                });
            }
        }

        private void DisplayWeapons()
        {
            WeaponSettings ESPWeaponSettings = ESPSettings.Weapons;
            if (!ESPWeaponSettings.Enabled) return;
            foreach (Weapon weapon in GameObjectManager.Weapons.Where(w => w != null && w is { hasBroken: false, heldByPlayer: false } && (!w.held || !ESPWeaponSettings.IgnoreHeld) && (ESPWeaponSettings is { Guns: false, Melees: false, Shields: false } || (ESPWeaponSettings.Guns && w.IsGunWeapon()) || (ESPWeaponSettings.Melees && w.IsMelee()) || (ESPWeaponSettings.Shields && w.IsShield()))).ToList())
            {
                DrawESP(weapon, ESPWeaponSettings.Color, (screen, color, distance) => 
                {
                    float offset = 0f;
                    if (ESPWeaponSettings.ShowName) DrawText(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(weapon.name, @"\s*\(Clone\)\s*", ""), @"\s*\(?\s*\d+\s*\)?\s*$", ""), @"(_|(?<!^)(?=[A-Z]))", " "), @"\s{2,}", " ").Trim(), color, screen, ref offset);
                    if (ESPWeaponSettings.ShowType) DrawText($"[{(weapon.IsGunWeapon() ? "Gun" : weapon.IsShield() ? "Shield" : weapon.IsMelee() ? "Melee" : "Weapon")}]", color, screen, ref offset);
                    if (ESPWeaponSettings.ShowHeld) DrawText($"[{(weapon.held ? "Held" : "Unheld")}]", color, screen, ref offset);
                    if (ESPWeaponSettings.ShowDistance) DrawText($"[{distance:F2}m]", color, screen, ref offset);
                });
            }
        }

        private void DisplayButtons()
        {
            ButtonSettings ESPButtonSettings = ESPSettings.Buttons;
            if (!ESPButtonSettings.Enabled) return;
            foreach (WorldButton button in GameObjectManager.WorldButtons.Where(b => b != null && (!b.pressed || !ESPButtonSettings.IgnorePressed)).ToList())
            {
                DrawESP(button, ESPButtonSettings.Color, (screen, color, distance) =>
                {
                    float offset = 0f;
                    if (ESPButtonSettings.ShowName) DrawText(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(button.name, @"\s*\(Clone\)\s*", ""), @"(_\d+|\d+)$", ""), @"(_|(?<!^)(?=[A-Z]))", " "), @"\s{2,}", " ").Trim(), color, screen, ref offset); 
                    if (ESPButtonSettings.ShowPressedState) DrawText($"[{(button.pressed ? "Pressed" : "Unpressed")}]", color, screen, ref offset);
                    if (ESPButtonSettings.ShowDistance) DrawText($"[{distance:F2}m]", color, screen, ref offset);
                });
            }
        }

        private void DrawESP<T>(T obj, Color color, Action<Vector2, Color, float> drawAction) where T : Component
        {
            if (obj == null || !obj.gameObject.activeSelf) return;
            float distance = GetDistanceToPlayer(obj.transform.position);
            if (distance == 0f || distance > ESPSettings.MaxRange) return;
            if (!WorldToScreen(obj.transform.position, out Vector2 screen)) return;
            drawAction(screen, color, distance);
        }

        private static void DrawText(string text, Color color, Vector2 screen, ref float offset)
        {
            VisualUtil.Text(new VisualUtil.TextOptions
            {
                Text = text,
                TextColor = color,
                Position = new Vector2(screen.x, screen.y + offset),
                Centered = true,
                Outline = true,
                OutlineColor = Color.black
            });
            offset += 14f;
        }
    }
}
