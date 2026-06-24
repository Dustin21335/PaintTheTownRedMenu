using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;

namespace PaintTheTownRedMenu.Cheats
{
    public class SetHealth() : ExecutableCheat("Set Health")
    {
        public class Settings
        {
            public int Health = 100;
        }

        public Settings SetHealthSettings = new();

        public override void Execute()
        {
            PTTRPlayer? localPlayer = GameObjectManager.LocalPlayer;
            if (localPlayer == null) return;
            float health = SetHealthSettings.Health / 100f;
            localPlayer.SetHealth(health, health < localPlayer.health);
        }
    }
}
