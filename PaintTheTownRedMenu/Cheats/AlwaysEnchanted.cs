using HarmonyLib;
using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;

namespace PaintTheTownRedMenu.Cheats
{
    [HarmonyPatch(typeof(WeaponAttack))]
    public class AlwaysEnchanted : ToggleCheat
    {
        public class Settings
        {
            public bool Weapon = true;
            public bool Shield = true;
        }

        public Settings AlwaysEnchantedSettings = new();

        public override string GetName()
        {
            return "Always Enchanted";
        }

        public override void Update()
        {
            if (!Enabled) return;
            PTTRPlayer? localPlayer = GameObjectManager.LocalPlayer;
            if (localPlayer == null) return;
            WeaponAttack weaponAttack = localPlayer.weaponAttack;
            bool weaponEnabled = AlwaysEnchantedSettings.Weapon;
            bool shieldEnabled = AlwaysEnchantedSettings.Shield;
            bool noneEnabled = !weaponEnabled && !shieldEnabled;
            if ((weaponEnabled || noneEnabled) && weaponAttack.currentWeapon is { enchantedWeapon: false } weapon) EntityManager.EnchantWeapon(weapon);
            if ((shieldEnabled || noneEnabled) && weaponAttack.currentShield is { enchantedWeapon: false } shield) EntityManager.EnchantWeapon(shield);
        }

        public override void OnDisable()
        {
            PTTRPlayer? localPlayer = GameObjectManager.LocalPlayer;
            if (localPlayer == null) return;
            WeaponAttack weaponAttack = localPlayer.weaponAttack;
            if (weaponAttack.currentWeapon is { enchantedWeapon: true } weapon) EntityManager.UnenchantWeapon(weapon);
            if (weaponAttack.currentShield is { enchantedWeapon: true } shield) EntityManager.UnenchantWeapon(shield);
        }
    }
}
