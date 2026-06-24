using HarmonyLib;
using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;

namespace PaintTheTownRedMenu.Cheats
{
    [HarmonyPatch(typeof(EnemyTriggerZone))]
    public class DisableEnemyCombatTriggerZones() : ToggleCheat("Disable Enemy Combat Trigger Zones")
    {
        [HarmonyPatch(nameof(EnemyTriggerZone.OnTriggerEnter)), HarmonyPrefix]
        public static bool OnTriggerEnter()
        {
            return Instance<DisableEnemyCombatTriggerZones>() is not { Enabled: true };
        }       

        [HarmonyPatch(nameof(EnemyTriggerZone.OnTriggerExit)), HarmonyPrefix]
        public static bool OnTriggerExit()
        {
            return Instance<DisableEnemyCombatTriggerZones>() is not { Enabled: true };
        }
    }
}
