using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;
using UnityEngine;

namespace PaintTheTownRedMenu.Cheats
{
    public class NoClip() : ToggleCheat(KeyCode.Delete)
    {
        public override string GetName()
        {
            return "No Clip";
        }

        public override void OnStateChanged(bool state)
        {
            DebugManager.FreeCam = state;
        }
    }
}