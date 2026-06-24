using Il2Cpp;
using MelonLoader;
using PaintTheTownRedMenu.Cheats.Core;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace PaintTheTownRedMenu.Cheats
{
    public class SetAllEnemiesAction() : ExecutableCheat("Set All Enemies Action")
    {
        public class Settings
        {
            public int EnemiesPerFrame = 5;
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

        private object? _setAllEnemiesActionsCoroutine;

        public override void Execute()
        {
            if (_setAllEnemiesActionsCoroutine != null)
            {
                MelonCoroutines.Stop(_setAllEnemiesActionsCoroutine);
                _setAllEnemiesActionsCoroutine = null;
            }
            _setAllEnemiesActionsCoroutine = MelonCoroutines.Start(SetAllEnemiesActionRoutine());
        }

        private IEnumerator SetAllEnemiesActionRoutine()
        {
            Settings settings = SetAllEnemiesActionSettings;
            int count = 0;
            foreach (Enemy enemy in GameObjectManager.Enemies.Where(e => e != null && !e.IsDead()).ToList())
            {
                if (settings is { Kill: false, Explode: false, SetOnFire: false, Poison: false, KeepSkeleton: false, Shockwave: false }) enemy.Kill();
                else
                {
                    if (settings.Explode)
                    {
                        if (settings.KeepSkeleton)
                        {
                            enemy.ExplodeKeepSkeleton();
                            if (settings.SetOnFire) enemy.SetOnFire();
                            if (settings.Poison) enemy.SetPoisoned();
                        }
                        else enemy.Explode();
                    }
                    else
                    {
                        if (settings.Kill) enemy.Kill();
                        if (settings.SetOnFire) enemy.SetOnFire();
                        if (settings.Poison) enemy.SetPoisoned();
                        if (settings.Knockdown) enemy.Knockdown(Vector3.zero, Vector3.zero);
                        if (settings.Shockwave) GameObjectManager.LocalPlayer?.Shockwave(settings.MinForce, settings.MaxForce, settings.Range, settings.TargetPlayerIfHit);
                    }
                }
                count++;
                if (count < settings.EnemiesPerFrame) continue;
                count = 0;
                yield return null;
            }
        }
    }
}