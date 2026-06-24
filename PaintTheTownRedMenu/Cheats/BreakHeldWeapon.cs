using PaintTheTownRedMenu.Cheats.Core;
using UnityEngine;

namespace PaintTheTownRedMenu.Cheats
{
    public class BreakHeldWeapon() : ExecutableCheat("Break Held Weapon")
    {
        public class Settings
        {
            public bool IgnoreStickInChild = true;
        }

        public Settings BreakHeldWeaponSettings = new();

        public override void Execute()
        {
            GameObjectManager.LocalPlayer?.weaponAttack.currentWeapon?.Break(Vector3.zero, BreakHeldWeaponSettings.IgnoreStickInChild, false, false);
        }
    }
}
