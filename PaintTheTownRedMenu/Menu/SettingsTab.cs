using PaintTheTownRedMenu.Cheats;
using PaintTheTownRedMenu.Cheats.Core;
using PaintTheTownRedMenu.Menu.Core;
using PaintTheTownRedMenu.Utils;
using System.Linq;

namespace PaintTheTownRedMenu.Menu
{
    public class SettingsTab() : Tab("Settings")
    {
        private string _search = string.Empty;
        private string _configCode = string.Empty;

        public override void Render()
        {
            UIUtil.Button("Reset Settings", Settings.ResetSettings);
            UIUtil.Button("Save Settings", Settings.SaveSettings);
            UIUtil.Button("Reload Settings", Settings.LoadSettings);
            UIUtil.Button("Open Settings", Settings.OpenSettings);
            UIUtil.Button("Copy Config Code", Settings.CopyConfigCode);
            UIUtil.Input("Config Code", ref _configCode, 2000);
            UIUtil.Button("Load Config Code", () =>
            {
                Settings.LoadConfigCode(_configCode);
                _configCode = string.Empty;
            });
            ToggleMenu? toggleMenu = Cheat.Instance<ToggleMenu>();
            if (toggleMenu != null) UIUtil.ColorPickerButton("Menu Watermark Color", ref toggleMenu.ToggleMenuSettings.MenuWatermarkColor);
            UIUtil.Checkbox(Cheat.Instance<FPSCounter>());
            UIUtil.Checkbox(Cheat.Instance<DebugMode>());
            UIUtil.Text("Keybinds");
            UIUtil.Input("Search", ref _search, 64);
            foreach (Cheat cheat in PaintTheTownRedMenuMod.Instance.Cheats.Where(c => !c.Hidden && (string.IsNullOrEmpty(_search) || c.Name.ToLower().Contains(_search.ToLower())))) UIUtil.KeyPicker(cheat);
        }
    }
}
