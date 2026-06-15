using HarmonyLib;
using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;

namespace PaintTheTownRedMenu.Cheats
{
    [HarmonyPatch(typeof(PTTRPlayer))]
    public class GodMode : ToggleCheat
    {
        public override string GetName()
        {
            return "God Mode";
        }

        public override void OnStateChanged(bool state)
        {
            CheatsManager.Invincibility = state;
        }

        [HarmonyPatch(nameof(PTTRPlayer.SetHealth)), HarmonyPrefix]
        public static bool SetHealth()
        {
            return Instance<GodMode>() is not { Enabled: true };
        }
    }
}
