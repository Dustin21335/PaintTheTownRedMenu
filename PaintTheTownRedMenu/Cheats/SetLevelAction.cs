using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;
// ReSharper disable SwitchStatementHandlesSomeKnownEnumValuesWithDefault

namespace PaintTheTownRedMenu.Cheats
{
    public class SetLevelAction : ExecutableCheat
    {
        public class Settings
        {
            public LevelAction Action = LevelAction.Win;
        }

        public Settings SetLevelActionSettings = new();

        public enum LevelAction
        {
            Win,
            Restart
        }

        public override string GetName()
        {
            return "Set Level Action";
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
