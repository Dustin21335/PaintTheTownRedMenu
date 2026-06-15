using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;
using System.Linq;
using UnityEngine;

namespace PaintTheTownRedMenu.Cheats
{
    public class SetAllEnemiesAction : ExecutableCheat
    {
        public class Settings
        {
            public bool Kill = false;
            public bool Explode = true;
            public bool KeepSkeleton = true;
            public bool SetOnFire = false;
            public bool Poison = false;
            public bool Knockdown = false;
            public bool Shockwave = false;
            public float MinForce = 20f;
            public float MaxForce = 70f;
            public float Range = 8f;
            public bool TargetPlayerIfHit = true;
        }

        public Settings SetAllEnemiesActionSettings = new();

        public override string GetName()
        {
            return "Set All Enemies Action";
        }

        public override void Execute()
        {
            Settings settings = SetAllEnemiesActionSettings;
            foreach (Enemy enemy in GameObjectManager.Enemies.Where(e => e != null).ToList())
            {
                if (settings.Explode)
                {
                    if (settings.KeepSkeleton) enemy.ExplodeKeepSkeleton();
                    else
                    {
                        enemy.Explode();
                        return;
                    }
                    if (settings.SetOnFire) enemy.SetOnFire();
                    if (settings.Poison) enemy.SetPoisoned();
                    return;
                }
                if (settings.Kill) enemy.Kill();
                if (settings.SetOnFire) enemy.SetOnFire();
                if (settings.Poison) enemy.SetPoisoned();
                if (settings.Knockdown) enemy.Knockdown(Vector3.zero, Vector3.zero);
                if (settings.Shockwave) GameObjectManager.LocalPlayer?.Shockwave(settings.MinForce, settings.MaxForce, settings.Range, settings.TargetPlayerIfHit);
            }
        }
    }
}
