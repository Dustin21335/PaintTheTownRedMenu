using PaintTheTownRedMenu.Cheats.Core;

namespace PaintTheTownRedMenu.Cheats
{
    public class SetSpeedBoost() : ExecutableCheat("Set Speed Boost")
    {
        public class Settings
        {
            public int SpeedBoost = 0;
        }

        public Settings SetSpeedBoostSettings = new();

        public override void Execute()
        {
            GameObjectManager.LocalPlayer?.SetSpeedBoost(SetSpeedBoostSettings.SpeedBoost);
        }
    }
}
