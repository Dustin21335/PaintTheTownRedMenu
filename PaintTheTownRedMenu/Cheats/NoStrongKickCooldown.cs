using HarmonyLib;
using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;

namespace PaintTheTownRedMenu.Cheats
{
    [HarmonyPatch(typeof(PTTRPlayer))]
    public class NoStrongKickCooldown() : ToggleCheat("No Strong Kick Cooldown")
    {
        [HarmonyPatch("get_kickCooldown"), HarmonyPrefix]
        public static bool get_kickCooldown(ref float __result)
        {
            if (Instance<NoStrongKickCooldown>() is not { Enabled: true }) return true;
            __result = 1f;
            return false;
        }   
    }
}
