using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;

namespace PaintTheTownRedMenu.Cheats
{
    public class UnbreakableWeapons() : ToggleCheat("Unbreakable Weapons")
    {
        public override void OnStateChanged(bool state)
        {
            CheatsManager.UnbreakableWeapons = state;
        }
    }
}
