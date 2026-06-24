using UnityEngine;

namespace PaintTheTownRedMenu.Cheats.Core
{
    public abstract class ToggleCheat(string name, KeyCode keybind = KeyCode.None, bool hidden = false) : Cheat(name, keybind, hidden)
    {
        public bool Enabled
        {
            get;
            set
            {
                if (field == value) return;
                field = value;
                if (field) OnEnable();
                else OnDisable();
                OnStateChanged(field);
            }
        } = false;

        public virtual void OnSceneChanged(string sceneName) { }
        public virtual void OnStateChanged(bool state) { }
        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
        public virtual void Update() { }
        public virtual void OnGUI() { }
    }
}