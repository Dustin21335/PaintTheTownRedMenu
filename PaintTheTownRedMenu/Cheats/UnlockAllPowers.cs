using HarmonyLib;
using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;
// ReSharper disable InconsistentNaming

namespace PaintTheTownRedMenu.Cheats
{
    [HarmonyPatch(typeof(PTTRPlayer))]
    public class UnlockAllPowers : ToggleCheat
    {
        public class Settings
        {
            public bool Power1 = true;
            public bool Power2 = true;
            public bool Power3 = true;
        }

        public Settings UnlockAllPowersSettings = new();

        public override string GetName()
        {
            return "Unlock All Powers";
        }

        [HarmonyPatch(nameof(PTTRPlayer.CanDoPower)), HarmonyPrefix]
        public static bool CanDoPower(int powerIndex, ref bool __result)
        {
            UnlockAllPowers? unlockAllPowers = Instance<UnlockAllPowers>();
            if (unlockAllPowers is { Enabled: false }) return true;
            Settings? settings = unlockAllPowers?.UnlockAllPowersSettings;
            if (settings != null && (settings.Power1 || settings.Power2 || settings.Power3) && (powerIndex != 0 || !settings.Power1) && (powerIndex != 1 || !settings.Power2) && (powerIndex != 2 || !settings.Power3)) return true;
            __result = true;
            return false;
        }
    }
}
