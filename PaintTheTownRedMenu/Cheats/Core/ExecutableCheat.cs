using UnityEngine;

namespace PaintTheTownRedMenu.Cheats.Core
{
    public abstract class ExecutableCheat(string name, KeyCode keybind = KeyCode.None, bool hidden = false) : Cheat(name, keybind, hidden)
    {
        public abstract void Execute();
    }
}
