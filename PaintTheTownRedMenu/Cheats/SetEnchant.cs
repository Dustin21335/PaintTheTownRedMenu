using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;
// ReSharper disable SwitchStatementHandlesSomeKnownEnumValuesWithDefault

namespace PaintTheTownRedMenu.Cheats
{
    public class SetEnchant() : ExecutableCheat("Set Enchant")
    {
        public class Settings
        {
            public bool Weapon = false;
            public bool Shield = false;
            public EnchantMode Mode = EnchantMode.Enchant;
        }

        public Settings SetEnchantSettings = new();

        public enum EnchantMode
        {
            Enchant,
            Unenchant
        }

        public override void Execute()
        {
            PTTRPlayer? localPlayer = GameObjectManager.LocalPlayer;
            if (localPlayer == null) return;
            WeaponAttack weaponAttack = localPlayer.weaponAttack;
            bool weaponEnabled = SetEnchantSettings.Weapon;
            bool shieldEnabled = SetEnchantSettings.Shield;
            bool noneEnabled = !weaponEnabled && !shieldEnabled;
            if ((weaponEnabled || noneEnabled) && weaponAttack.currentWeapon is { } weapon) SetEnchantState(weapon);
            if ((shieldEnabled || noneEnabled) && weaponAttack.currentShield is { } shield) SetEnchantState(shield);
        }

        private void SetEnchantState(Weapon weapon)
        {
            switch (SetEnchantSettings.Mode)
            {
                case EnchantMode.Enchant:
                    if (!weapon.enchantedWeapon) EntityManager.EnchantWeapon(weapon);
                    break;
                case EnchantMode.Unenchant:
                    if (weapon.enchantedWeapon) EntityManager.UnenchantWeapon(weapon);
                    break;
            }
        }
    }
}