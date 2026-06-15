using PaintTheTownRedMenu.Cheats.Core;
using UnityEngine;

namespace PaintTheTownRedMenu.Cheats
{
    public class BreakHeldWeapon : ExecutableCheat
    {
        public class Settings
        {
            public bool IgnoreStickInChild = true;
        }

        public Settings BreakHeldWeaponSettings = new();

        public override string GetName()
        {
            return "Break Held Weapon";
        }

        public override void Execute()
        {
            GameObjectManager.LocalPlayer?.weaponAttack.currentWeapon?.Break(Vector3.zero, BreakHeldWeaponSettings.IgnoreStickInChild, false, false);
        }
    }
}
