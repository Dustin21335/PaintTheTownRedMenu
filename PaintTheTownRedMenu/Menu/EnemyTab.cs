using Il2Cpp;
using ImGuiNET;
using MelonLoader;
using PaintTheTownRedMenu.Cheats.Core;
using PaintTheTownRedMenu.Menu.Core;
using PaintTheTownRedMenu.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EnemySettings = Il2Cpp.EnemySettings;

namespace PaintTheTownRedMenu.Menu
{
    public class EnemyTab() : Tab("Enemy")
    {
        private Enemy? _selectedEnemy;
        private bool _keepSkeleton;
        private int _spawnAmount = 1;
        private List<EnemySettings> _allEnemySettings = [];
        private EnemySettings? _selectedEnemySettings;
        private EnemyAppearance.EnumEnemyAppearance _selectedEnemyAppearance;
        private Enemy.Type _selectedEnemyType;
        private string _enemySearch = "";
        private string _enemySpawnSearch = "";
        private int _maxSpawnsPerFrame = 5;
        private object? _spawnCoroutine;

        public override void Render()
        {
            UIUtil.TabBar("EnemyTabs", () =>
            {
                Vector2 contentRegionAvail = ImGui.GetContentRegionAvail().ToUnity();
                UIUtil.TabBarItem("Enemy List", () =>
                {
                    if (_selectedEnemy != null && _selectedEnemy.IsDead()) _selectedEnemy = null;
                    UIUtil.Table("EnemyList", () =>
                    {
                        UIUtil.Column(() =>
                        {
                            UIUtil.Input("Search", ref _enemySearch, 128);
                            UIUtil.ChildArea("EnemyList", () =>
                            {
                                List<Enemy> enemies = GameObjectManager.Enemies.Where(e => e != null && !e.IsDead()).ToList();
                                if (enemies.Count == 0)
                                {
                                    UIUtil.Text("No enemies");
                                    UIUtil.Dummy(new Vector2(0, contentRegionAvail.y));
                                    return;
                                }
                                foreach (Enemy enemy in enemies.Where(e => string.IsNullOrEmpty(_enemySearch) || e.GetName().Contains(_enemySearch, StringComparison.OrdinalIgnoreCase)).Where(e => UIUtil.Selectable($"{e.GetName()} [{Cheat.GetDistanceToPlayer(e.transform.position):F2}m]##{e.GetInstanceID()}", _selectedEnemy == e))) _selectedEnemy = enemy;
                                UIUtil.Dummy(new Vector2(0, contentRegionAvail.y));
                            }, null, ImGuiChildFlags.None, ImGuiWindowFlags.HorizontalScrollbar);
                        });
                        UIUtil.Column(() =>
                        {
                            if (_selectedEnemy == null)
                            {
                                UIUtil.Text("Select an enemy");
                                return;
                            }
                            PTTRPlayer? localPlayer = GameObjectManager.LocalPlayer;
                            UIUtil.Text($"Name: {_selectedEnemy.GetName()}");
                            UIUtil.Text($"Type: {_selectedEnemy.thisEnemyType}");
                            UIUtil.Text($"Health: {_selectedEnemy.health * 100f:F0}");
                            UIUtil.Text($"In Combat: {_selectedEnemy.inCombat}");
                            UIUtil.Text($"Can Knock Down: {_selectedEnemy.canBeKnockedDown}");
                            UIUtil.Text($"Emotional State: {_selectedEnemy.emotionalState}");
                            UIUtil.Text($"Distance: {Cheat.GetDistanceToPlayer(_selectedEnemy.transform.position)}");
                            if (_selectedEnemy.inCombat) UIUtil.Button("Stop Combat", () => _selectedEnemy.StopCombat(0));
                            else UIUtil.Button("Start Combat", () => _selectedEnemy.StartCombat());
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
                            UIUtil.Button("Teleport To", () => localPlayer?.Teleport(_selectedEnemy.transform.position + new Vector3(0f, 0.3f, 0f)));
                            UIUtil.Button("Teleport To You", () =>
                            {
                                Transform? localPlayerTransform = localPlayer?.transform;
                                if (localPlayerTransform != null) _selectedEnemy.transform.position = localPlayerTransform.position;
                            });
                        });
                    }, ImGuiTableFlags.Borders | ImGuiTableFlags.Reorderable, ImGuiTableColumnFlags.None, null, "Enemy List", "Enemy Actions");
                });
                UIUtil.TabBarItem("Enemy Spawner", () =>
                {
                    UIUtil.Table("EnemySpawner", () =>
                    {
                        UIUtil.Column(() =>
                        {
                            UIUtil.Input("Search", ref _enemySpawnSearch, 128);
                            UIUtil.ChildArea("EnemySpawner", () =>
                            {
                                if (_allEnemySettings.Count == 0)
                                {
                                    if (EntityManager.Instance == null)
                                    {
                                        UIUtil.Text("EntityManager is null");
                                        return;
                                    }
                                    if (EntityManager.Instance.enemySettings == null)
                                    {
                                        UIUtil.Text("Enemy settings is null");
                                        return;
                                    }
                                    _allEnemySettings = EntityManager.Instance.enemySettings.OrderBy(es => es.GetName()).ToList();
                                    UIUtil.Text("No enemies");
                                    UIUtil.Dummy(new Vector2(0, contentRegionAvail.y));
                                    return;
                                }
                                foreach (EnemySettings enemySettings in _allEnemySettings.Where(es => string.IsNullOrEmpty(_enemySpawnSearch) || es.GetName().Contains(_enemySpawnSearch, StringComparison.OrdinalIgnoreCase)).Where(es => UIUtil.Selectable($"{es.GetName()}##{es.GetInstanceID()}", _selectedEnemySettings == es))) _selectedEnemySettings = enemySettings;
                                UIUtil.Dummy(new Vector2(0, contentRegionAvail.y));
                            }, null, ImGuiChildFlags.None, ImGuiWindowFlags.HorizontalScrollbar);
                        });
                        UIUtil.Column(() =>
                        {
                            if (_selectedEnemySettings == null)
                            {
                                UIUtil.Text("Select an enemy");
                                return;
                            }
                            UIUtil.Text($"Name: {_selectedEnemySettings.GetName()}");
                            UIUtil.Dropdown("Appearance", ref _selectedEnemyAppearance);
                            UIUtil.Dropdown("Type", ref _selectedEnemyType);
                            UIUtil.Slider("Max Spawns Per Frame", ref _maxSpawnsPerFrame, 1, 50);
                            UIUtil.Slider("Amount", ref _spawnAmount, 1, 100);
                            UIUtil.Button("Spawn", () =>
                            {
                                Transform? transform = GameObjectManager.LocalPlayer?.transform;
                                if (transform == null) return;
                                if (_spawnCoroutine != null)
                                {
                                    MelonCoroutines.Stop(_spawnCoroutine);
                                    _spawnCoroutine = null;
                                }
                                _spawnCoroutine = MelonCoroutines.Start(SpawnRoutine(_selectedEnemySettings, transform));
                            });
                        });
                    }, ImGuiTableFlags.Borders | ImGuiTableFlags.Reorderable, ImGuiTableColumnFlags.None, null, "Enemy Spawner", "Enemy Spawner Actions");
                });
            });
        }

        private IEnumerator SpawnRoutine(EnemySettings enemySettings, Transform transform)
        {
            int count = 0;
            for (int i = 0; i < _spawnAmount; i++)
            {
                EntityManager.SpawnEnemy(transform, enemySettings, _selectedEnemyAppearance, new Enemy.AdditionalSpawnInfo(), -1, -1, _selectedEnemyType);
                count++;
                if (count < _maxSpawnsPerFrame) continue;
                count = 0;
                yield return null;
            }
        }
    }
}
