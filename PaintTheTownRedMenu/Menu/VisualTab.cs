using PaintTheTownRedMenu.Cheats;
using PaintTheTownRedMenu.Cheats.Core;
using PaintTheTownRedMenu.Menu.Core;
using PaintTheTownRedMenu.Utils;

namespace PaintTheTownRedMenu.Menu
{
    public class VisualTab() : Tab("Visual")
    {
        public override void Render()
        {
            ESP? esp = Cheat.Instance<ESP>();
            UIUtil.Checkbox(esp);
            ESP.Settings? espSettings = esp?.ESPSettings;
            if (espSettings != null)
            {
                UIUtil.Indent(() =>
                {
                    UIUtil.Slider("Max Range", ref espSettings.MaxRange, 1f, 2000f);
                    UIUtil.ID("Enemies", () =>
                    {
                        ESP.EnemySettings espEnemySettings = espSettings.Enemies;
                        UIUtil.Checkbox("Enemies", ref espEnemySettings.Enabled); 
                        UIUtil.Indent(() =>
                        {
                            UIUtil.ColorPickerButton("Color", ref espEnemySettings.Color);
                            UIUtil.Checkbox("Show Name", ref espEnemySettings.ShowName);
                            UIUtil.Checkbox("Show Health", ref espEnemySettings.ShowHealth);
                            UIUtil.Checkbox("Show Type", ref espEnemySettings.ShowType);
                            UIUtil.Checkbox("Show Distance", ref espEnemySettings.ShowDistance);
                        });
                    });
                    UIUtil.ID("Weapons", () =>
                    {
                        ESP.WeaponSettings espWeaponSettings = espSettings.Weapons;
                        UIUtil.Checkbox("Weapons", ref espWeaponSettings.Enabled);
                        UIUtil.Indent(() =>
                        {
                            UIUtil.ColorPickerButton("Color", ref espWeaponSettings.Color);
                            UIUtil.Checkbox("Show Name", ref espWeaponSettings.ShowName);
                            UIUtil.Checkbox("Show Type", ref espWeaponSettings.ShowType);
                            UIUtil.Checkbox("Ignore Held", ref espWeaponSettings.IgnoreHeld);
                            UIUtil.Checkbox("Show Distance", ref espWeaponSettings.ShowDistance);
                            UIUtil.Checkbox("Show Held", ref espWeaponSettings.ShowHeld);
                            UIUtil.Checkbox("Guns", ref espWeaponSettings.Guns);
                            UIUtil.Checkbox("Melees", ref espWeaponSettings.Melees);
                            UIUtil.Checkbox("Shields", ref espWeaponSettings.Shields);
                        });
                    });
                    UIUtil.ID("Buttons", () =>
                    {
                        ESP.ButtonSettings espButtonSettings = espSettings.Buttons;
                        UIUtil.Checkbox("Buttons", ref espButtonSettings.Enabled);
                        UIUtil.Indent(() =>
                        {
                            UIUtil.ColorPickerButton("Color", ref espButtonSettings.Color);
                            UIUtil.Checkbox("Show Name", ref espButtonSettings.ShowName);
                            UIUtil.Checkbox("Show Distance", ref espButtonSettings.ShowDistance);
                            UIUtil.Checkbox("Ignore Pressed", ref espButtonSettings.IgnorePressed);
                            UIUtil.Checkbox("Show Pressed State", ref espButtonSettings.ShowPressedState);
                        });
                    });
                });
            }
        }
    }
}