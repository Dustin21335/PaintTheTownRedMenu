using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;

namespace PaintTheTownRedMenu.Cheats
{
    public class EnemiesIgnoreYou() : ToggleCheat("Enemies Ignore You")
    {
        public override void OnStateChanged(bool state)
        {
            CheatsManager.EnemiesIgnorePlayer = state;
        }  
    }
}
