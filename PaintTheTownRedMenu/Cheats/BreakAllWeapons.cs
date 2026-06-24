using Il2Cpp;
using MelonLoader;
using PaintTheTownRedMenu.Cheats.Core;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace PaintTheTownRedMenu.Cheats
{
    public class BreakAllWeapons() : ExecutableCheat("Break All Weapons")
    {
        public class Settings
        {
            public int WeaponsPerFrame = 5;
            public bool IgnoreStickInChild = true;
        }

        public Settings BreakAllWeaponsSettings = new();

        private object? _breakAllWeaponsCoroutine;

        public override void Execute()
        {
            if (_breakAllWeaponsCoroutine != null)
            {
                MelonCoroutines.Stop(_breakAllWeaponsCoroutine);
                _breakAllWeaponsCoroutine = null;
            }
            _breakAllWeaponsCoroutine = MelonCoroutines.Start(BreakAllWeaponsRoutine());
        }

        private IEnumerator BreakAllWeaponsRoutine()
        {
            Settings settings = BreakAllWeaponsSettings;
            int count = 0;
            foreach (Weapon weapon in GameObjectManager.Weapons.Where(w => w != null && !w.hasBroken).ToList())
            {
                weapon.Break(Vector3.zero, settings.IgnoreStickInChild, false, true);
                count++;
                if (count < settings.WeaponsPerFrame) continue;
                count = 0;
                yield return null;
            }
        }
    }
}
