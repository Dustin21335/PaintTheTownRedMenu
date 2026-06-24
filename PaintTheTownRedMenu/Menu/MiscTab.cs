using PaintTheTownRedMenu.Cheats;
using PaintTheTownRedMenu.Cheats.Core;
using PaintTheTownRedMenu.Menu.Core;
using PaintTheTownRedMenu.Utils;

namespace PaintTheTownRedMenu.Menu
{
    public class MiscTab() : Tab("Misc")
    {
        public override void Render()
        {
            CustomSteamRichPresence? customSteamRichPresence = Cheat.Instance<CustomSteamRichPresence>();
            UIUtil.Checkbox(customSteamRichPresence);
            CustomSteamRichPresence.Settings? customSteamRichPresenceSettings = customSteamRichPresence?.CustomSteamRichPresenceSettings;
            if (customSteamRichPresenceSettings != null)
            {
                UIUtil.Indent(() => UIUtil.Input("Custom Message", ref customSteamRichPresenceSettings.CustomPresence, 256));
            }
            BloodDecalColor? bloodDecalColor = Cheat.Instance<BloodDecalColor>();
            UIUtil.Checkbox(bloodDecalColor);
            BloodDecalColor.Settings? bloodDecalColorSettings = bloodDecalColor?.BloodDecalColorSettings;
            if (bloodDecalColorSettings != null)
            {
                UIUtil.Indent(() => UIUtil.Dropdown("Blood Color", ref bloodDecalColorSettings.BloodColor));
            }
            SetAllEnemiesAction? setAllEnemiesAction = Cheat.Instance<SetAllEnemiesAction>();
            UIUtil.Button(setAllEnemiesAction);
            SetAllEnemiesAction.Settings? setAllEnemiesActionSettings = setAllEnemiesAction?.SetAllEnemiesActionSettings;
            if (setAllEnemiesActionSettings != null)
            {
                UIUtil.ID(setAllEnemiesAction, () =>
                {
                    UIUtil.Indent(() =>
                    {
                        UIUtil.Slider("Enemies Per Frame", ref setAllEnemiesActionSettings.EnemiesPerFrame, 1, 50);
                        UIUtil.Checkbox("Kill", ref setAllEnemiesActionSettings.Kill);
                        UIUtil.Checkbox("Explode", ref setAllEnemiesActionSettings.Explode);
                        UIUtil.Indent(() => UIUtil.Checkbox("Keep Skeleton", ref setAllEnemiesActionSettings.KeepSkeleton));
                        UIUtil.Checkbox("Set On Fire", ref setAllEnemiesActionSettings.SetOnFire);
                        UIUtil.Checkbox("Poison", ref setAllEnemiesActionSettings.Poison);
                        UIUtil.Checkbox("Knockdown", ref setAllEnemiesActionSettings.Knockdown);
                        UIUtil.Checkbox("Shockwave", ref setAllEnemiesActionSettings.Shockwave);
                        UIUtil.Indent(() =>
                        {
                            UIUtil.Slider("Min Force", ref setAllEnemiesActionSettings.MinForce, 1f, 500f);
                            UIUtil.Slider("Max Force", ref setAllEnemiesActionSettings.MaxForce, 1f, 1000f);
                            UIUtil.Slider("Range", ref setAllEnemiesActionSettings.Range, 1f, 250f);
                            UIUtil.Checkbox("Target Player If Hit", ref setAllEnemiesActionSettings.TargetPlayerIfHit);
                        });
                    });
                });
            }
            BreakAllWeapons? breakAllWeapons = Cheat.Instance<BreakAllWeapons>();
            UIUtil.Button(breakAllWeapons);
            BreakAllWeapons.Settings? breakAllWeaponsSettings = breakAllWeapons?.BreakAllWeaponsSettings;
            if (breakAllWeaponsSettings != null)
            {
                UIUtil.ID(breakAllWeapons, () =>
                {
                    UIUtil.Indent(() =>
                    {
                        UIUtil.Slider("Weapons Per Frame", ref breakAllWeaponsSettings.WeaponsPerFrame, 1, 50);
                        UIUtil.Checkbox("Ignore Stick In Child", ref breakAllWeaponsSettings.IgnoreStickInChild);
                    });
                });
            }
            UIUtil.Button(Cheat.Instance<ShootAllCannons>());
            UIUtil.Checkbox(Cheat.Instance<KnockdownAnyone>());
            SetLevelAction? setLevelAction = Cheat.Instance<SetLevelAction>();
            UIUtil.Button(setLevelAction);
            SetLevelAction.Settings? setLevelActionSettings = setLevelAction?.SetLevelActionSettings;
            if (setLevelActionSettings != null)
            {
                UIUtil.ID(setLevelAction, () => UIUtil.Indent(() => UIUtil.Dropdown("Action", ref setLevelActionSettings.Action)));
            }
            DisplayInfo? displayInfo = Cheat.Instance<DisplayInfo>();
            UIUtil.Checkbox(displayInfo);
            DisplayInfo.Settings? displayInfoSettings = displayInfo?.DisplayInfoSettings;
            if (displayInfoSettings != null)
            {
                UIUtil.ID(displayInfo, () =>
                {
                    UIUtil.Indent(() =>
                    {
                        UIUtil.Checkbox("Display Enemies Left", ref displayInfoSettings.DisplayEnemiesLeft);
                        UIUtil.Checkbox("Display Weapons Left", ref displayInfoSettings.DisplayWeaponsLeft);
                        UIUtil.Checkbox("Auto Resize Window", ref displayInfoSettings.AutoResizeWindow);
                    });
                });
            }
        }
    }
}
