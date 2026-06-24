using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using PaintTheTownRedMenu.Cheats.Core;
using System.Collections;
using System.Linq;

namespace PaintTheTownRedMenu.Cheats
{
    [HarmonyPatch(typeof(Enemy))]
    public class DisableEnemyCombat() : ToggleCheat("Disable Enemy Combat")
    {
        public class Settings
        {
            public int EnemiesPerFrame = 5;
        }

        public Settings DisableEnemyCombatSettings = new();

        private object? _disableEnemyCombatCoroutine;

        public override void OnEnable()
        {
            if (_disableEnemyCombatCoroutine != null)
            {
                MelonCoroutines.Stop(_disableEnemyCombatCoroutine);
                _disableEnemyCombatCoroutine = null;
            }
            _disableEnemyCombatCoroutine = MelonCoroutines.Start(DisableEnemyCombatRoutine());
        }

        private IEnumerator DisableEnemyCombatRoutine()
        {
            Settings settings = DisableEnemyCombatSettings;
            int count = 0;
            foreach (Enemy enemy in GameObjectManager.Enemies.Where(e => e != null && !e.IsDead()).ToList())
            {
                enemy.StopCombat(0);
                count++;
                if (count < settings.EnemiesPerFrame) continue;
                count = 0;
                yield return null;
            }
        }

        [HarmonyPatch(nameof(Enemy.StartCombat)), HarmonyPrefix]
        public static bool StartCombat()
        {
            return Instance<DisableEnemyCombat>() is not { Enabled: true };
        }
    }
}
