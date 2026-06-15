using UnityEngine;

namespace PaintTheTownRedMenu.Cheats.Core
{
    public abstract class ExecutableCheat(KeyCode keybind = KeyCode.None, bool hidden = false) : Cheat(keybind, hidden)
    {
        public abstract void Execute();
    }
}
