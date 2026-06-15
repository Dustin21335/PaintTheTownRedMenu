using PaintTheTownRedMenu.Cheats;
using PaintTheTownRedMenu.Cheats.Core;
using PaintTheTownRedMenu.Menu.Core;
using PaintTheTownRedMenu.Utils;

namespace PaintTheTownRedMenu.Menu
{
    public class SelfTab() : Tab("Self")
    {
        public override void Render()
        {
            UIUtil.Checkbox(Cheat.Instance<GodMode>());
            UIUtil.Checkbox(Cheat.Instance<EnemiesIgnoreYou>());
            UIUtil.Checkbox(Cheat.Instance<NoClip>());
            UIUtil.Checkbox(Cheat.Instance<UnbreakableWeapons>());
            UIUtil.Separator();
            UnlimitedAmmo? unlimitedAmmo = Cheat.Instance<UnlimitedAmmo>();
            UIUtil.Checkbox(unlimitedAmmo);
            UnlimitedAmmo.Settings? unlimitedAmmoSettings = unlimitedAmmo?.UnlimitedAmmoSettings;
            if (unlimitedAmmoSettings != null)
            {
                UIUtil.Indent(() => UIUtil.ID(unlimitedAmmo, () => UIUtil.Checkbox("Cannons", ref unlimitedAmmoSettings.Cannons)));
            }
            UIUtil.Separator();
            GrabRandomWeapon? grabRandomWeapon = Cheat.Instance<GrabRandomWeapon>();
            UIUtil.Button(grabRandomWeapon);
            GrabRandomWeapon.Settings? grabRandomWeaponSettings = grabRandomWeapon?.GrabRandomWeaponSettings;
            if (grabRandomWeaponSettings != null)
            {
                UIUtil.Indent(() =>
                { 
                    UIUtil.ID(grabRandomWeapon, () =>
                    {
                        UIUtil.Checkbox("Gun", ref grabRandomWeaponSettings.Gun);
                        UIUtil.Checkbox("Melee", ref grabRandomWeaponSettings.Melee);
                        UIUtil.Checkbox("Shield", ref grabRandomWeaponSettings.Shield);
                        UIUtil.Checkbox("Steal Held Weapons", ref grabRandomWeaponSettings.StealHeldWeapons);
                    });
                });
            }
            UIUtil.Separator();
            BreakHeldWeapon? breakHeldWeapon = Cheat.Instance<BreakHeldWeapon>();
            UIUtil.Button(breakHeldWeapon);
            BreakHeldWeapon.Settings? breakHeldWeaponSettings = breakHeldWeapon?.BreakHeldWeaponSettings;
            if (breakHeldWeaponSettings != null)
            {
                UIUtil.ID(breakHeldWeapon, () => UIUtil.Indent(() => UIUtil.Checkbox("Ignore Stick In Child", ref breakHeldWeaponSettings.IgnoreStickInChild)));
            }
            UIUtil.Separator();
            HitActions? hitActions = Cheat.Instance<HitActions>();
            UIUtil.Checkbox(hitActions);
            HitActions.Settings? hitActionSettings = hitActions?.HitActionSettings;
            if (hitActionSettings != null)
            {
                UIUtil.Indent(() =>
                {
                    UIUtil.ID(hitActions, () =>
                    {
                        UIUtil.Checkbox("Kill", ref hitActionSettings.Kill);
                        UIUtil.Checkbox("Explode", ref hitActionSettings.Explode);
                        UIUtil.Indent(() => UIUtil.Checkbox("Keep Skeleton", ref hitActionSettings.KeepSkeleton));
                        UIUtil.Checkbox("Set On Fire", ref hitActionSettings.SetOnFire);
                        UIUtil.Checkbox("Poison", ref hitActionSettings.Poison);
                        UIUtil.Checkbox("Knockdown", ref hitActionSettings.Knockdown);
                        UIUtil.Checkbox("Shockwave", ref hitActionSettings.Shockwave);
                        UIUtil.Indent(() =>
                        {
                            UIUtil.Slider("Min Force", ref hitActionSettings.MinForce, 1f, 500f);
                            UIUtil.Slider("Max Force", ref hitActionSettings.MaxForce, 1f, 1000f);
                            UIUtil.Slider("Range", ref hitActionSettings.Range, 1f, 250f);
                            UIUtil.Checkbox("Target Player If Hit", ref hitActionSettings.TargetPlayerIfHit);
                        });
                    });
                });
            }
            UIUtil.Separator();
            UnlockAllPowers? unlockAllPowers = Cheat.Instance<UnlockAllPowers>();
            UIUtil.Checkbox(unlockAllPowers);
            UnlockAllPowers.Settings? unlockAllPowersSettings = unlockAllPowers?.UnlockAllPowersSettings;
            if (unlockAllPowersSettings != null)
            {
                UIUtil.Indent(() =>
                {
                    UIUtil.ID(unlockAllPowers, () =>
                {
                        UIUtil.Checkbox("Power 1", ref unlockAllPowersSettings.Power1);
                        UIUtil.Checkbox("Power 2", ref unlockAllPowersSettings.Power2);
                        UIUtil.Checkbox("Power 3", ref unlockAllPowersSettings.Power3);
                    });
                });
            }
            UIUtil.Separator();
            SetEnchant? setEnchant = Cheat.Instance<SetEnchant>();
            UIUtil.Button(setEnchant);
            SetEnchant.Settings? setEnchantSettings = setEnchant?.SetEnchantSettings;
            if (setEnchantSettings != null)
            {
                UIUtil.Indent(() =>
                {
                    UIUtil.ID(setEnchant, () =>
                    {
                        UIUtil.Dropdown("Mode", ref setEnchantSettings.Mode);
                        UIUtil.Checkbox("Weapon", ref setEnchantSettings.Weapon);
                        UIUtil.Checkbox("Shield", ref setEnchantSettings.Shield);
                    }); 
                });
            }
            UIUtil.Separator();
            AlwaysEnchanted? alwaysEnchanted = Cheat.Instance<AlwaysEnchanted>();
            UIUtil.Checkbox(alwaysEnchanted);
            AlwaysEnchanted.Settings? alwaysEnchantedSettings = alwaysEnchanted?.AlwaysEnchantedSettings;
            if (alwaysEnchantedSettings != null)
            {
                UIUtil.Indent(() =>
                {
                    UIUtil.ID(alwaysEnchanted, () =>
                    {
                        UIUtil.Checkbox("Weapon", ref alwaysEnchantedSettings.Weapon);
                        UIUtil.Checkbox("Shield", ref alwaysEnchantedSettings.Shield);
                    });
                });
            }
            UIUtil.Separator();
            KeepThrownWeapon? keepThrownWeapon = Cheat.Instance<KeepThrownWeapon>(); 
            UIUtil.Checkbox(keepThrownWeapon);
            KeepThrownWeapon.Settings? keepThrownWeaponSettings = keepThrownWeapon?.KeepThrownWeaponSettings;
            if (keepThrownWeaponSettings != null)
            {
                UIUtil.Indent(() =>
                {
                    UIUtil.Checkbox("Weapon", ref keepThrownWeaponSettings.Weapon);
                    UIUtil.Checkbox("Shield", ref keepThrownWeaponSettings.Shield);
                    UIUtil.Slider("Throw Timeout", ref keepThrownWeaponSettings.ThrowTimeout, 0.5f, 10f);
                });
            }
            UIUtil.Separator();
            Aura? aura = Cheat.Instance<Aura>();
            UIUtil.Checkbox(aura);
            Aura.Settings? auraSettings = aura?.AuraSettings;
            if (auraSettings != null)
            {
                UIUtil.Indent(() =>
                {
                    UIUtil.ID(aura, () =>
                    {
                        UIUtil.Slider("Range", ref auraSettings.Range, 1f, 50f);
                        UIUtil.Checkbox("Kill", ref auraSettings.Kill);
                        UIUtil.Checkbox("Explode", ref auraSettings.Explode);
                        UIUtil.Indent(() => UIUtil.Checkbox("Keep Skeleton", ref auraSettings.KeepSkeleton));
                        UIUtil.Checkbox("Set On Fire", ref auraSettings.SetOnFire);
                        UIUtil.Checkbox("Poison", ref auraSettings.Poison);
                        UIUtil.Checkbox("Knockdown", ref auraSettings.Knockdown);
                        UIUtil.Checkbox("Shockwave", ref auraSettings.Shockwave);
                        UIUtil.ID("AuraShockwave", () =>
                        {
                            UIUtil.Indent(() =>
                            {
                                UIUtil.Slider("Min Force", ref auraSettings.MinForce, 1f, 500f);
                                UIUtil.Slider("Max Force", ref auraSettings.MaxForce, 1f, 1000f);
                                UIUtil.Slider("Range", ref auraSettings.ShockwaveRange, 1f, 250f);
                                UIUtil.Checkbox("Target Player If Hit", ref auraSettings.TargetPlayerIfHit);
                            });
                        });
                    });
                });
            }
            UIUtil.Separator();
            ClickAction? clickAction = Cheat.Instance<ClickAction>();
            UIUtil.Checkbox(clickAction);
            ClickAction.Settings? clickActionSettings = clickAction?.ClickActionSettings;
            if (clickActionSettings != null)
            {
                UIUtil.Indent(() =>
                {
                    UIUtil.ID(clickAction, () =>
                    {
                        UIUtil.Slider("Max Distance", ref clickActionSettings.MaxDistance, 1f, 2000f);
                        UIUtil.Dropdown("Mouse Button", ref clickActionSettings.MouseButton);
                        UIUtil.Checkbox("Teleport", ref clickActionSettings.Teleport);
                        UIUtil.Checkbox("Kill", ref clickActionSettings.Kill);
                        UIUtil.Checkbox("Explode", ref clickActionSettings.Explode);
                        UIUtil.Indent(() => UIUtil.Checkbox("Keep Skeleton", ref clickActionSettings.KeepSkeleton));
                        UIUtil.Checkbox("Set On Fire", ref clickActionSettings.SetOnFire);
                        UIUtil.Checkbox("Poison", ref clickActionSettings.Poison);
                        UIUtil.Checkbox("Knockdown", ref clickActionSettings.Knockdown);
                        UIUtil.Checkbox("Shockwave", ref clickActionSettings.Shockwave);
                        UIUtil.Indent(() =>
                        {
                            UIUtil.Slider("Min Force", ref clickActionSettings.MinForce, 1f, 500f);
                            UIUtil.Slider("Max Force", ref clickActionSettings.MaxForce, 1f, 1000f);
                            UIUtil.Slider("Range", ref clickActionSettings.Range, 1f, 250f);
                            UIUtil.Checkbox("Target Player If Hit", ref clickActionSettings.TargetPlayerIfHit);
                        });
                    });
                });
            }
        }
    }
}