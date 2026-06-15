using HarmonyLib;
using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;

namespace PaintTheTownRedMenu.Cheats
{
    [HarmonyPatch(typeof(CharacterManager))]
    public class EnemiesIgnoreYou : ToggleCheat
    {
        public override string GetName()
        {
            return "Enemies Ignore You";
        }

        public override void OnStateChanged(bool state)
        {
            CheatsManager.EnemiesIgnorePlayer = state;
        }  
    }
}
