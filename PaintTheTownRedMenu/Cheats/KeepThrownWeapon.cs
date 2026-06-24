using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using PaintTheTownRedMenu.Cheats.Core;
using System.Collections;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace PaintTheTownRedMenu.Cheats
{
    [HarmonyPatch]
    public class KeepThrownWeapon() : ToggleCheat("Keep Thrown Weapon")
    {
        public class Settings
        {
            public bool Weapon = true;
            public bool Shield = true;
            public float ThrowTimeout = 1f;
        }

        public Settings KeepThrownWeaponSettings = new();

        private Weapon? _lastThrownWeapon;
        private object? _timeoutRoutine;

        public override void OnDisable()
        {
            _lastThrownWeapon = null;
            if (_timeoutRoutine != null) MelonCoroutines.Stop(_timeoutRoutine);
        }

        [HarmonyPatch(typeof(Weapon), nameof(Weapon.Throw)), HarmonyPrefix]
        public static void Throw(Weapon __instance)
        {
            KeepThrownWeapon? keepThrownWeapon = Instance<KeepThrownWeapon>();
            if (keepThrownWeapon is not { Enabled: true }) return;
            Settings settings = keepThrownWeapon.KeepThrownWeaponSettings;
            if ((settings.Weapon || settings.Shield) && (((__instance.IsGunWeapon() || __instance.IsMelee()) && !settings.Weapon) || (__instance.IsShield() && !settings.Shield))) return;
            keepThrownWeapon._lastThrownWeapon = __instance;
            if (keepThrownWeapon._timeoutRoutine != null) MelonCoroutines.Stop(keepThrownWeapon._timeoutRoutine);
            keepThrownWeapon._timeoutRoutine = MelonCoroutines.Start(keepThrownWeapon.ThrowTimeout(__instance));
        }

        [HarmonyPatch(typeof(Weapon), nameof(Weapon.CheckForBreak)), HarmonyPrefix]
        public static bool CheckForBreak(Weapon __instance, ref bool __result)
        {
            KeepThrownWeapon? keepThrownWeapon = Instance<KeepThrownWeapon>();
            if (keepThrownWeapon is not { Enabled: true } || keepThrownWeapon._lastThrownWeapon != __instance) return true;
            __result = true;
            return false;
        }

        [HarmonyPatch(typeof(WeaponBase), nameof(WeaponBase.HitSomething)), HarmonyPrefix]
        public static void HitSomething(WeaponBase __instance)
        {
            KeepThrownWeapon? keepThrownWeapon = Instance<KeepThrownWeapon>();
            if (keepThrownWeapon is not { Enabled: true } || keepThrownWeapon._lastThrownWeapon != __instance) return;
            Settings settings = keepThrownWeapon.KeepThrownWeaponSettings;
            if ((settings.Weapon || settings.Shield) && (((__instance.IsGunWeapon() || __instance.IsMelee()) && !settings.Weapon) || (__instance.IsShield() && !settings.Shield))) return;
            GameObjectManager.LocalPlayer?.PickUpWeapon(keepThrownWeapon._lastThrownWeapon);
            keepThrownWeapon._lastThrownWeapon = null;
        }

        private IEnumerator ThrowTimeout(Weapon weapon)
        {
            yield return new WaitForSeconds(KeepThrownWeaponSettings.ThrowTimeout);
            if (!Enabled || _lastThrownWeapon != weapon) yield break;
            Settings settings = KeepThrownWeaponSettings;
            if ((settings.Weapon || settings.Shield) && (((weapon.IsGunWeapon() || weapon.IsMelee()) && !settings.Weapon) || (weapon.IsShield() && !settings.Shield))) yield break;
            GameObjectManager.LocalPlayer?.PickUpWeapon(weapon);
            _lastThrownWeapon = null;
        }
    }
}
