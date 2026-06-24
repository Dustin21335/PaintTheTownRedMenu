using Il2Cpp;
using Il2CppRTEditor;
using PaintTheTownRedMenu.Cheats.Core;
using PaintTheTownRedMenu.Utils;
using UnityEngine;

namespace PaintTheTownRedMenu.Cheats
{
    public class ClickAction() : ToggleCheat("Click Action")
    {
        public class Settings
        {
            public bool Teleport = true;
            public float MaxDistance = 500f;
            public MouseButton MouseButton = MouseButton.Middle;
            public bool Kill = false;
            public bool Explode = false;
            public bool KeepSkeleton = false;
            public bool SetOnFire = false;
            public bool Poison = false;
            public bool Knockdown = false;
            public bool Shockwave = false;
            public float MinForce = 20f;
            public float MaxForce = 70f;
            public float Range = 8f;
            public bool TargetPlayerIfHit = true;
        }

        public Settings ClickActionSettings = new();

        public override void Update()
        {
            if (!Enabled) return;
            Settings settings = ClickActionSettings;
            if (!Input.GetMouseButtonDown((int)settings.MouseButton)) return;
            Camera? activeCamera = CameraUtil.GetActiveCamera();
            if (activeCamera == null) return;
            if (!Physics.Raycast(activeCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, settings.MaxDistance)) return;
            Enemy? enemy = hit.collider.GetComponentInParent<Enemy>();
            if (enemy != null)
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
            if (settings.Teleport) GameObjectManager.LocalPlayer?.Teleport(hit.point);
        }
    }
}
