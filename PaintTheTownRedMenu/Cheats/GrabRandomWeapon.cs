using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PaintTheTownRedMenu.Cheats
{
    public class GrabRandomWeapon() : ExecutableCheat("Grab Random Weapon")
    {
        public class Settings
        {
            public bool Gun = true;
            public bool Melee = true;
            public bool Shield = true;
            public bool AllowHeldWeapons = true;
        }

        public Settings GrabRandomWeaponSettings = new();

        public override void Execute()
        {
            Settings settings = GrabRandomWeaponSettings;
            List<Weapon> weapons = GameObjectManager.Weapons.Where(w => w != null && w is { hasBroken: false, heldByPlayer: false } && (!w.held || settings.AllowHeldWeapons) && (settings is { Gun: false, Melee: false, Shield: false } || (settings.Gun && w.IsGunWeapon()) || (settings.Melee && w.IsMelee()) || (settings.Shield && w.IsShield()))).ToList();
            if (weapons.Count == 0) return;
            Weapon weapon = weapons[Random.Range(0, weapons.Count)];
            if (settings.AllowHeldWeapons && weapon.held)
            {
                Enemy? enemy = GameObjectManager.Enemies.FirstOrDefault(e => weapon.IsShield() ? e.shield == weapon : e.weapon == weapon); 
                if (enemy != null)
                {
                    if (weapon.IsShield()) enemy.DropShield();
                    else enemy.DropWeapon();
                }
            }
            PTTRPlayer? localPlayer = GameObjectManager.LocalPlayer;
            if (localPlayer == null) return;
            if (weapon.IsShield()) localPlayer.DropShield();
            else localPlayer.DropWeapon();
            localPlayer.PickUpWeapon(weapon);
        }
    }
}
