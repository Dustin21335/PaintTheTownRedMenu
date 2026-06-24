using PaintTheTownRedMenu.Cheats.Core;
using System.Linq;

namespace PaintTheTownRedMenu.Cheats
{
    public class DebugMode() : ToggleCheat("Debug Mode")
    {
        public override void OnStateChanged(bool state)
        {
            PaintTheTownRedMenuMod.Instance.Renderer?.Tabs.FirstOrDefault(t => t.Name == "Debug")?.Enabled = state;
        }
    }
}