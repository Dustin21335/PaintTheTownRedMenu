using HarmonyLib;
using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;
using System.Collections.Generic;
// ReSharper disable InconsistentNaming

namespace PaintTheTownRedMenu.Cheats
{
    [HarmonyPatch(typeof(Enemy))]
    public class KnockdownAnyone() : ToggleCheat("Knockdown Anyone")
    {
        private static readonly Dictionary<Enemy, bool> EnemiesOriginalKnockdownState = new();

        public override void OnDisable()
        {
            foreach (KeyValuePair<Enemy, bool> enemyKnockdownState in EnemiesOriginalKnockdownState)
            {
                Enemy enemy = enemyKnockdownState.Key; 
                if (enemy != null) enemy.canBeKnockedDown = enemyKnockdownState.Value;
            }
            EnemiesOriginalKnockdownState.Clear();
        }

        [HarmonyPatch(nameof(Enemy.DoKnockdown)), HarmonyPrefix]
        public static void DoKnockdown_Prefix(Enemy __instance)
        {
            if (Instance<KnockdownAnyone>() is not { Enabled: true }) return;
            if (!EnemiesOriginalKnockdownState.ContainsKey(__instance)) EnemiesOriginalKnockdownState[__instance] = __instance.canBeKnockedDown;
            __instance.canBeKnockedDown = true;
        }

        [HarmonyPatch(nameof(Enemy.DoKnockdown)), HarmonyPostfix]
        public static void DoKnockdown_Postfix(Enemy __instance)
        {
            if (Instance<KnockdownAnyone>() is not { Enabled: true }) return;
            if (!EnemiesOriginalKnockdownState.TryGetValue(__instance, out bool originalKnockdownState)) return;
            __instance.canBeKnockedDown = originalKnockdownState;
            EnemiesOriginalKnockdownState.Remove(__instance);
        }
    }
}
