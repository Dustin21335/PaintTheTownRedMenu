using HarmonyLib;
using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;
using UnityEngine;

namespace PaintTheTownRedMenu.Cheats
{
    [HarmonyPatch(typeof(PTTRPlayer))]
    public class HitActions : ToggleCheat
    {
        public class Settings
        {
            public bool Kill = false;
            public bool Explode = true;
            public bool KeepSkeleton = true;
            public bool SetOnFire = true; 
            public bool Poison = true; 
            public bool Knockdown = false;
            public bool Shockwave = false;
            public float MinForce = 20f;
            public float MaxForce = 70f;
            public float Range = 8f;
            public bool TargetPlayerIfHit = true;
        }

        public Settings HitActionSettings = new();

        public override string GetName()
        {
            return "Hit Action";
        }

        [HarmonyPatch(nameof(PTTRPlayer.PlayerHitItemEvaluation)), HarmonyPrefix]
        public static void PlayerHitItemEvaluation(Enemy enemy)
        {
            if (enemy == null) return;
            HitActions? hitEffects = Instance<HitActions>();
            if (hitEffects is not { Enabled: true }) return;
            Settings settings = hitEffects.HitActionSettings;
            if (settings is { Kill: false, Explode: false, SetOnFire: false, Poison: false, KeepSkeleton: false, Shockwave: false })
            {
                enemy.Kill();
                return;
            }
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