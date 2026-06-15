using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;
using System.Linq;
using UnityEngine;

namespace PaintTheTownRedMenu.Cheats
{
    public class Aura : ToggleCheat
    {
        public class Settings
        {
            public bool Kill = false;
            public bool Explode = false;
            public bool KeepSkeleton = false;
            public bool SetOnFire = false;
            public bool Poison = false;
            public bool Knockdown = false;
            public bool Shockwave = false;
            public float MinForce = 20f;
            public float MaxForce = 70f;
            public float ShockwaveRange = 8f;
            public bool TargetPlayerIfHit = true;
            public float Range = 5f;
        }

        public Settings AuraSettings = new();

        public override string GetName()
        {
            return "Aura";
        }

        public override void Update()
        {
            if (!Enabled) return;
            Settings settings = AuraSettings;
            foreach (Enemy enemy in GameObjectManager.Enemies.Where(e => e != null && !e.IsDead()).ToList().Where(e => !(GetDistanceToPlayer(e.transform.position) > AuraSettings.Range) && !GameManager.Paused))
            {
                if (settings is { Kill: false, Explode: false, SetOnFire: false, Poison: false, KeepSkeleton: false, Shockwave: false })
                {
                    enemy.Kill();
                    continue;
                }
                if (settings.Explode)
                {
                    if (settings.KeepSkeleton) enemy.ExplodeKeepSkeleton();
                    else
                    {
                        enemy.Explode();
                        continue;
                    }
                    if (settings.SetOnFire) enemy.SetOnFire();
                    if (settings.Poison) enemy.SetPoisoned();
                    if (settings.Shockwave) GameObjectManager.LocalPlayer?.Shockwave(settings.MinForce, settings.MaxForce, settings.ShockwaveRange, settings.TargetPlayerIfHit);
                    continue;
                }
                if (settings.Kill) enemy.Kill();
                if (settings.SetOnFire) enemy.SetOnFire();
                if (settings.Poison) enemy.SetPoisoned();
                if (settings.Knockdown) enemy.Knockdown(Vector3.zero, Vector3.zero);
                if (settings.Shockwave) GameObjectManager.LocalPlayer?.Shockwave(settings.MinForce, settings.MaxForce, settings.ShockwaveRange, settings.TargetPlayerIfHit);
            }
        }
    }
}
