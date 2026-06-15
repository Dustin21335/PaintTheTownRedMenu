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
            SetAllEnemiesAction? setAllEnemies = Cheat.Instance<SetAllEnemiesAction>();
            UIUtil.Button(setAllEnemies);
            SetAllEnemiesAction.Settings? setAllEnemiesSettings = setAllEnemies?.SetAllEnemiesActionSettings;
            if (setAllEnemiesSettings != null)
            {
                UIUtil.ID(setAllEnemies, () =>
                {
                    UIUtil.Indent(() =>
                    {
                        UIUtil.Checkbox("Kill", ref setAllEnemiesSettings.Kill);
                        UIUtil.Checkbox("Explode", ref setAllEnemiesSettings.Explode);
                        UIUtil.Indent(() => UIUtil.Checkbox("Keep Skeleton", ref setAllEnemiesSettings.KeepSkeleton));
                        UIUtil.Checkbox("Set On Fire", ref setAllEnemiesSettings.SetOnFire);
                        UIUtil.Checkbox("Poison", ref setAllEnemiesSettings.Poison);
                        UIUtil.Checkbox("Knockdown", ref setAllEnemiesSettings.Knockdown);
                        UIUtil.Checkbox("Shockwave", ref setAllEnemiesSettings.Shockwave);
                        UIUtil.Indent(() =>
                        {
                            UIUtil.Slider("Min Force", ref setAllEnemiesSettings.MinForce, 1f, 500f);
                            UIUtil.Slider("Max Force", ref setAllEnemiesSettings.MaxForce, 1f, 1000f);
                            UIUtil.Slider("Range", ref setAllEnemiesSettings.Range, 1f, 250f);
                            UIUtil.Checkbox("Target Player If Hit", ref setAllEnemiesSettings.TargetPlayerIfHit);
                        });
                    });
                });
            }
            UIUtil.Separator();
            BreakAllWeapons? breakAllWeapons = Cheat.Instance<BreakAllWeapons>();
            UIUtil.Button(breakAllWeapons);
            BreakAllWeapons.Settings? breakAllWeaponsSettings = breakAllWeapons?.BreakAllWeaponsSettings;
            if (breakAllWeaponsSettings != null)
            {
                UIUtil.ID(breakAllWeapons, () => UIUtil.Indent(() => UIUtil.Checkbox("Ignore Stick In Child", ref breakAllWeaponsSettings.IgnoreStickInChild)));
            }
            UIUtil.Separator();
            UIUtil.Button(Cheat.Instance<ShootAllCannons>());
            UIUtil.Separator();
            SetLevelAction? setLevelAction = Cheat.Instance<SetLevelAction>();
            UIUtil.Button(setLevelAction);
            SetLevelAction.Settings? setLevelActionSettings = setLevelAction?.SetLevelActionSettings;
            if (setLevelActionSettings != null)
            {
                UIUtil.ID(setLevelAction, () => UIUtil.Indent(() => UIUtil.Dropdown("Action", ref setLevelActionSettings.Action)));
            }
            UIUtil.Separator();
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
