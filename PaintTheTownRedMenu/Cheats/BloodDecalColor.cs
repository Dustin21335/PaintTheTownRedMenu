using HarmonyLib;
using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;

namespace PaintTheTownRedMenu.Cheats
{
    [HarmonyPatch(typeof(BloodDecalsManager))]
    public class BloodDecalColor() : ToggleCheat("Blood Decal Color")
    {
        public class Settings
        {
            public EntityManager.BloodType BloodColor = EntityManager.BloodType.Normal;
        }

        public Settings BloodDecalColorSettings = new();

        [HarmonyPatch(nameof(BloodDecalsManager.BloodColour)), HarmonyPrefix]
        public static void BloodColour(ref EntityManager.BloodType bloodType, ref bool doRandom)
        {
            BloodDecalColor? bloodDecalColor = Instance<BloodDecalColor>();
            if (bloodDecalColor is not { Enabled: true }) return;
            bloodType = bloodDecalColor.BloodDecalColorSettings.BloodColor;
            doRandom = false;
        }
    }
}
