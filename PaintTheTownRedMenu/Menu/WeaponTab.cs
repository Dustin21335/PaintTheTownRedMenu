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
using System.Text.RegularExpressions;
using UnityEngine;

namespace PaintTheTownRedMenu.Menu
{
    public class WeaponTab() : Tab("Weapon")
    {
        private sealed class WeaponSpawn(string name, string path)
        {
            public string Name { get; } = name;
            public string Path { get; } = path;
        }

        private List<WeaponSpawn> _allWeaponSpawns = [];
        private WeaponSpawn? _selectedWeaponSpawn;
        private int _spawnAmount = 1;
        private bool _canBePickedUpByEnemies = true;
        private bool _unbreakable;
        private bool _enchanted;
        private Weapon? _selectedWeapon;
        private bool _ignoreStickInChild = true;
        private string _listSearch = "";
        private string _spawnerSearch = "";
        private bool _grabAllowHeldWeapons = true;
        private bool _teleportAllowHeldWeapons = true;
        private bool _ignoreHeld;
        private int _maxSpawnsPerFrame = 5;
        private object? _spawnCoroutine;

        public override void Render()
        {
            UIUtil.TabBar("WeaponTab", () =>
            {
                Vector2 contentRegionAvail = ImGui.GetContentRegionAvail().ToUnity();
                UIUtil.TabBarItem("Weapon List", () =>
                {
                    if (_selectedWeapon != null && _selectedWeapon.hasBroken) _selectedWeapon = null;
                    UIUtil.Table("WeaponList", () =>
                    {
                        UIUtil.Column(() =>
                        {
                            UIUtil.Input("Search", ref _listSearch, 128);
                            UIUtil.Checkbox("Ignore Held", ref _ignoreHeld);
                            UIUtil.ChildArea("WeaponList", () =>
                            {
                                List<Weapon> weapons = GameObjectManager.Weapons.Where(w => w != null && w is { hasBroken: false, heldByPlayer: false } && (!w.held || !_ignoreHeld)).ToList();
                                if (weapons.Count == 0)
                                {
                                    UIUtil.Text("No weapons");
                                    UIUtil.Dummy(new Vector2(0, contentRegionAvail.y));
                                    return;
                                }
                                foreach (Weapon weapon in weapons.Where(w => string.IsNullOrEmpty(_listSearch) || w.GetName().Contains(_listSearch, StringComparison.OrdinalIgnoreCase)).Where(w => UIUtil.Selectable($"{w.GetName()} [{(w.held ? "Held" : "Unheld")}] [{Cheat.GetDistanceToPlayer(w.transform.position):F2}m]##{w.GetInstanceID()}", _selectedWeapon == w))) _selectedWeapon = weapon;
                                UIUtil.Dummy(new Vector2(0, contentRegionAvail.y));
                            }, null, ImGuiChildFlags.None, ImGuiWindowFlags.HorizontalScrollbar);
                        });
                        UIUtil.Column(() =>
                        {
                            if (_selectedWeapon == null)
                            {
                                UIUtil.Text("Select a weapon");
                                return;
                            }
                            PTTRPlayer? localPlayer = GameObjectManager.LocalPlayer;
                            UIUtil.Text($"Name {_selectedWeapon.GetName()}"); 
                            UIUtil.Text($"Type: {(_selectedWeapon.IsGunWeapon() ? "Gun" : _selectedWeapon.IsShield() ? "Shield" : _selectedWeapon.IsMelee() ? "Melee" : "Weapon")}");
                            UIUtil.Text($"Held: {_selectedWeapon.held}");
                            UIUtil.Text($"Distance: {Cheat.GetDistanceToPlayer(_selectedWeapon.transform.position)}");
                            UIUtil.Button("Break", () => _selectedWeapon.Break(Vector3.zero, _ignoreStickInChild, false, true));
                            UIUtil.Indent(() => UIUtil.Checkbox("Ignore Stick In Child", ref _ignoreStickInChild));
                            UIUtil.Button("Grab", () =>
                            {
                                if (localPlayer == null) return;
                                if (_grabAllowHeldWeapons && _selectedWeapon.held)
                                {
                                    Enemy? enemy = GameObjectManager.Enemies.FirstOrDefault(e => _selectedWeapon.IsShield() ? e.shield == _selectedWeapon : e.weapon == _selectedWeapon);
                                    if (enemy != null)
                                    {
                                        if (_selectedWeapon.IsShield()) enemy.DropShield();
                                        else enemy.DropWeapon();
                                    }
                                }
                                if (_selectedWeapon.IsShield()) localPlayer.DropShield();
                                else localPlayer.DropWeapon();
                                localPlayer.PickUpWeapon(_selectedWeapon);
                            });
                            UIUtil.Indent(() => UIUtil.Checkbox("Allow Held Weapons", ref _grabAllowHeldWeapons));
                            UIUtil.ID("Teleport", () =>
                            {
                                UIUtil.Button("Teleport To", () => localPlayer?.Teleport(_selectedWeapon.transform.position));
                                UIUtil.Button("Teleport To You", () =>
                                {
                                    Transform? localPlayerTransform = localPlayer?.transform;
                                    if (localPlayerTransform == null) return;
                                    if (_teleportAllowHeldWeapons && _selectedWeapon.held)
                                    {
                                        Enemy? enemy = GameObjectManager.Enemies.FirstOrDefault(e => _selectedWeapon.IsShield() ? e.shield == _selectedWeapon : e.weapon == _selectedWeapon);
                                        if (enemy != null)
                                        {
                                            if (_selectedWeapon.IsShield()) enemy.DropShield();
                                            else enemy.DropWeapon();
                                        }
                                    }
                                    _selectedWeapon.Teleport(localPlayerTransform.position);
                                });
                                UIUtil.Indent(() => UIUtil.Checkbox("Allow Held Weapons", ref _teleportAllowHeldWeapons));
                            });
                        });
                    }, ImGuiTableFlags.Borders | ImGuiTableFlags.Reorderable, ImGuiTableColumnFlags.None, null, "Weapon List", "Weapon Actions");
                });
                UIUtil.TabBarItem("Weapon Spawner", () =>
                {
                    UIUtil.Table("WeaponSpawner", () =>
                    {
                        UIUtil.Column(() =>
                        {
                            UIUtil.Input("Search", ref _spawnerSearch, 128);
                            UIUtil.ChildArea("WeaponSpawner", () =>
                            {
                                if (_allWeaponSpawns.Count == 0)
                                {
                                    if (EntityManager.Instance == null)
                                    {
                                        UIUtil.Text("EntityManager is null");
                                        return;
                                    }
                                    if (EntityManager.Instance.allWeaponPaths == null)
                                    {
                                        UIUtil.Text("Weapon paths is null");
                                        return;
                                    }
                                    _allWeaponSpawns = EntityManager.Instance.allWeaponPaths.Select(wp => new WeaponSpawn(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(wp.Split('/')[^1].Replace("_", " ").Replace("-", " ").Trim(), @"(\d)by(\d)", "$1 by $2", RegexOptions.IgnoreCase), @"([A-Za-z])(\d+)([A-Za-z]*)", "$1$2 $3"), @"(?<=[a-z])(?=[A-Z])", " "), @"\b(\d+)\s+by\s+(\d+)\b", "$1 by $2", RegexOptions.IgnoreCase), @"\s+", " ").Trim(), wp)).OrderBy(i => i.Name).ToList();
                                }
                                foreach (WeaponSpawn weaponSpawn in _allWeaponSpawns.Where(ws => string.IsNullOrWhiteSpace(_spawnerSearch) || ws.Name.Contains(_spawnerSearch, StringComparison.OrdinalIgnoreCase)).Where(ws => UIUtil.Selectable($"{ws.Name}", _selectedWeaponSpawn == ws))) _selectedWeaponSpawn = weaponSpawn;
                                UIUtil.Dummy(new Vector2(0, contentRegionAvail.y));
                            });
                        });
                        UIUtil.Column(() =>
                        {
                            if (_selectedWeaponSpawn == null)
                            {
                                UIUtil.Text("Select a weapon");
                                return;
                            }
                            UIUtil.Text($"Name: {_selectedWeaponSpawn.Name}");
                            UIUtil.Text($"Path: {_selectedWeaponSpawn.Path}");
                            UIUtil.Checkbox("Can Be Picked Up By Enemies", ref _canBePickedUpByEnemies);
                            UIUtil.Checkbox("Unbreakable", ref _unbreakable);
                            UIUtil.Checkbox("Enchanted", ref _enchanted);
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
                                _spawnCoroutine = MelonCoroutines.Start(SpawnRoutine(_selectedWeaponSpawn, transform));
                            });
                        });
                    }, ImGuiTableFlags.Borders | ImGuiTableFlags.Reorderable, ImGuiTableColumnFlags.None, null, "Weapon Spawner", "Weapon Spawner Actions");
                }); 
            });
        }

        private IEnumerator SpawnRoutine(WeaponSpawn weaponSpawn, Transform transform)
        {
            int count = 0;
            for (int i = 0; i < _spawnAmount; i++)
            {
                EntityManager.CreateWeapon(weaponSpawn.Path, transform.position, transform.rotation, Vector3.zero, canBePickedUpByEnemies: _canBePickedUpByEnemies, unbreakable: _unbreakable, enchanted: _enchanted);
                count++;
                if (count < _maxSpawnsPerFrame) continue;
                count = 0;
                yield return null;
            }
        }
    }
}
