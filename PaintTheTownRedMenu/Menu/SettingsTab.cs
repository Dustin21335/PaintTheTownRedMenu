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

        public override void Render()
        {
            UIUtil.Button("Reset Settings", Settings.ResetSettings);
            UIUtil.Button("Save Settings", Settings.SaveSettings);
            UIUtil.Button("Reload Settings", Settings.LoadSettings);
            UIUtil.Button("Open Settings", Settings.OpenSettings);
            UIUtil.Separator();
            ToggleMenu? toggleMenu = Cheat.Instance<ToggleMenu>();
            if (toggleMenu != null) UIUtil.ColorPickerButton("Menu Text Color", ref toggleMenu.ToggleMenuSettings.MenuTextColor);
            UIUtil.Checkbox(Cheat.Instance<FPSCounter>());
            UIUtil.Checkbox(Cheat.Instance<DebugMode>());
            UIUtil.Separator();
            UIUtil.Text("Keybinds");
            UIUtil.Input("Search", ref _search, 64);
            foreach (Cheat cheat in PaintTheTownRedMenuMod.Instance.Cheats.Where(c => !c.Hidden && (string.IsNullOrEmpty(_search) || c.GetName().ToLower().Contains(_search.ToLower())))) UIUtil.KeyPicker(cheat);
        }
    }
}
