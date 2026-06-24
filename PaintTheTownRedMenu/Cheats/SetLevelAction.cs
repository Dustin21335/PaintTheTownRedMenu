using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;
// ReSharper disable SwitchStatementHandlesSomeKnownEnumValuesWithDefault

namespace PaintTheTownRedMenu.Cheats
{
    public class SetLevelAction() : ExecutableCheat("Set Level Action")
    {
        public class Settings
        {
            public LevelAction Action = LevelAction.Restart;
        }

        public Settings SetLevelActionSettings = new();

        public enum LevelAction
        {
            Win,
            Restart
        }

        public override void Execute()
        {
            if (GameObjectManager.LocalPlayer == null) return;
            switch (SetLevelActionSettings.Action)
            {
                case LevelAction.Win:
                    NetSceneManager.Instance.LevelWon();
                    break;
                case LevelAction.Restart:
                    GameManager.RestartLevel();
                    break;
            }
        }
    }
}
