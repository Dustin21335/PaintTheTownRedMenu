using Il2Cpp;
using ImGuiNET;
using MelonLoader;
using PaintTheTownRedMenu.Cheats.Core;
using PaintTheTownRedMenu.Menu.Core;
using PaintTheTownRedMenu.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace PaintTheTownRedMenu.Menu
{
    public class EnemyTab() : Tab("Enemy")
    {
        private Enemy? _selectedEnemy;
        private bool _keepSkeleton;

        public override void Render()
        {
            if (_selectedEnemy != null && _selectedEnemy.IsDead()) _selectedEnemy = null;
            Vector2 contentRegionAvail = ImGui.GetContentRegionAvail().ToUnity();
            float leftWidth = contentRegionAvail.x * 0.35f;
            UIUtil.ChildArea("EnemyList", () =>
            { 
                List<Enemy> enemies = GameObjectManager.Enemies.Where(e => e != null && !e.IsDead()).ToList();
                if (enemies.Count == 0)
                {
                    UIUtil.Text("No enemies");
                    return;
                }
                foreach (Enemy enemy in enemies.Where(e => UIUtil.Selectable($"Enemy [{Regex.Replace(e.thisEnemyType.ToString(), "(\\B[A-Z])", " $1")}]##{e.GetInstanceID()}", _selectedEnemy == e))) _selectedEnemy = enemy;
            }, new Vector2(leftWidth, contentRegionAvail.y), ImGuiChildFlags.Border);
            UIUtil.SameLine();
            UIUtil.ChildArea("EnemyActions", () =>
            {
                if (_selectedEnemy == null)
                {
                    UIUtil.Text("Select an enemy");
                    return;
                }
                UIUtil.Text($"Name: {_selectedEnemy.name}");
                UIUtil.Text($"Type: {_selectedEnemy.thisEnemyType}");
                UIUtil.Text($"Health: {_selectedEnemy.health * 100f:F0}");
                UIUtil.Text($"Can Knock Down: {_selectedEnemy.canBeKnockedDown}");
                UIUtil.Text($"Emotional State: {_selectedEnemy.emotionalState}");
                UIUtil.Text($"Distance: {Cheat.GetDistanceToPlayer(_selectedEnemy.transform.position)}");
                UIUtil.Button("Kill", () => _selectedEnemy.Kill());
                UIUtil.Button("Explode", () =>
                {
                    if (_keepSkeleton) _selectedEnemy.ExplodeKeepSkeleton();
                    else _selectedEnemy.Explode();
                });
                UIUtil.Indent(() => UIUtil.Checkbox("Keep Skeleton", ref _keepSkeleton));
                UIUtil.Button("Set On Fire", () => _selectedEnemy.SetOnFire());
                UIUtil.Button("Poison", () => _selectedEnemy.SetPoisoned());
                UIUtil.Button("Knockdown", () => _selectedEnemy.Knockdown(Vector2.zero, Vector2.zero));
                UIUtil.Button("Teleport To", () =>
                {
                    Collider collider = _selectedEnemy.GetComponent<Collider>();
                    Vector3 position = collider != null ? collider.bounds.center + Vector3.up * collider.bounds.extents.y : _selectedEnemy.transform.position + Vector3.up * 2f;
                    GameObjectManager.LocalPlayer?.Teleport(position);
                });
            }, new Vector2(contentRegionAvail.x - leftWidth - 8f, contentRegionAvail.y), ImGuiChildFlags.Border);
        }
    }
}
