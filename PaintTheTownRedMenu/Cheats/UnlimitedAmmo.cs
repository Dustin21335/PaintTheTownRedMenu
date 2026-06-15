using HarmonyLib;
using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;
// ReSharper disable InconsistentNaming

namespace PaintTheTownRedMenu.Cheats
{
    [HarmonyPatch]
    public class UnlimitedAmmo : ToggleCheat
    {
        public class Settings
        {
            public bool Cannons = true;
        }

        public Settings UnlimitedAmmoSettings = new();

        public override string GetName()
        {
            return "Unlimited Ammo";
        }

        public override void OnStateChanged(bool state)
        {
            CheatsManager.UnlimitedAmmo = state;
        }

        [HarmonyPatch(typeof(Cannon), nameof(Cannon.DoPressAction)), HarmonyPrefix]
        public static void DoPressAction_Prefix(Cannon __instance)
        {
            if (Instance<UnlimitedAmmo>() is not { Enabled: true, UnlimitedAmmoSettings.Cannons: true }) return;
            if (__instance.ammo <= 0) __instance.ammo = 1; 
        }     
        
        [HarmonyPatch(typeof(Cannon), nameof(Cannon.DoPressAction)), HarmonyPostfix]
        public static void DoPressAction_Postfix(Cannon __instance)
        {
            if (Instance<UnlimitedAmmo>() is not { Enabled: true, UnlimitedAmmoSettings.Cannons: true }) return;
            __instance.worldButton.canUnpress = true;
            __instance.worldButton.stayPressed = false;
        }

        [HarmonyPatch(typeof(WorldButton), nameof(WorldButton.PressButton)), HarmonyPostfix]
        public static void PressButton(WorldButton __instance)
        {
            if (Instance<UnlimitedAmmo>() is not { Enabled: true, UnlimitedAmmoSettings.Cannons: true } || __instance.GetComponent<Cannon>() == null) return;
            __instance.SetButtonMaterials(true); 
        }
    }
}