using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;
using System.Linq;
using UnityEngine;

namespace PaintTheTownRedMenu.Cheats
{
    public class BreakAllWeapons : ExecutableCheat
    {
        public class Settings
        {
            public bool IgnoreStickInChild = true;
        }

        public Settings BreakAllWeaponsSettings = new();

        public override string GetName()
        {
            return "Break All Weapons";
        }

        public override void Execute()
        {
            foreach(Weapon weapon in GameObjectManager.Weapons.Where(w => w != null).ToList()) weapon.Break(Vector3.zero, BreakAllWeaponsSettings.IgnoreStickInChild, false, true);
        }
    }
}
