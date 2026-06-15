using Il2Cpp;
using MelonLoader;
using PaintTheTownRedMenu.Menu.Core;
using PaintTheTownRedMenu.Utils;
using UnityEngine;

namespace PaintTheTownRedMenu.Menu
{
    public class DebugTab() : Tab("Debug", false)
    {
        public override void Render()
        {
            UIUtil.Button("Log All Layers", () =>
            {
                for (int i = 0; i < 32; i++) MelonLogger.Msg($"[Layer {i}] {LayerMask.LayerToName(i) ?? "null"}");
            });
            UIUtil.Button("Log All Held Weapon Components", () =>
            {
                Weapon? weapon = GameObjectManager.LocalPlayer?.weaponAttack?.currentWeapon;
                if (weapon == null) return;
                foreach (Component component in weapon.GetComponentsInChildren<Component>(true))
                {
                    if (component == null) continue;
                    MelonLogger.Msg($"[Weapon] {component.name} - {component.GetType().FullName}");
                }
            });
            UIUtil.Button("Log All Held Shield Components", () =>
            {
                Shield? shield = GameObjectManager.LocalPlayer?.weaponAttack?.currentShield;
                if (shield == null) return;
                foreach (Component component in shield.GetComponentsInChildren<Component>(true))
                {
                    if (component == null) continue;
                    MelonLogger.Msg($"[Shield] {component.name} - {component.GetType().FullName}");
                }
            });
        }
    }
}
