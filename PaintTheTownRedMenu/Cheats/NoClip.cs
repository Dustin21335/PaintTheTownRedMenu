using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;
using UnityEngine;

namespace PaintTheTownRedMenu.Cheats
{
    public class NoClip() : ToggleCheat("No Clip", KeyCode.Delete)
    {
        public override void OnStateChanged(bool state)
        {
            DebugManager.FreeCam = state;
        }
    }
}