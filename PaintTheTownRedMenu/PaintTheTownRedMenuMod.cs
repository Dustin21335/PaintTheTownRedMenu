using MelonLoader;
using PaintTheTownRedMenu;
using PaintTheTownRedMenu.Cheats;
using PaintTheTownRedMenu.Cheats.Core;
using SharpGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Assembly = System.Reflection.Assembly;
using Renderer = PaintTheTownRedMenu.Menu.Core.Renderer;

[assembly: MelonInfo(typeof(PaintTheTownRedMenuMod), "Paint The Town Red Menu", "1.0.1", "Dustin")]
[assembly: MelonGame("South East Games", "Paint The Town Red")]

namespace PaintTheTownRedMenu
{
    public class PaintTheTownRedMenuMod : MelonMod
    {
        public static PaintTheTownRedMenuMod Instance { get; private set; } = null!;

        public Renderer? Renderer { get; set; }
        public List<Cheat> Cheats { get; } = [];
        public List<ToggleCheat> ToggleCheats { get; } = [];
        public List<ExecutableCheat> ExecutableCheats { get; } = [];

        public override void OnInitializeMelon()
        {
            Instance = this;
            Renderer = new Renderer();
        }

        public override void OnApplicationQuit()
        { 
            GUI.Shutdown();
        }

        public void Initialize()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace != null && t.Namespace.Contains("Cheats") && !t.Namespace.Contains("Core") && !t.IsAbstract && typeof(Cheat).IsAssignableFrom(t)))
            {
                if (Activator.CreateInstance(type) is not Cheat cheat)
                {
                    MelonLogger.Warning($"Failed to create instance of {type.FullName}");
                    continue;
                }
                Cheats.Add(cheat);
                string name = cheat.Name;
                bool log = !cheat.Hidden;
                switch (cheat)
                {
                    case ToggleCheat toggle:
                        ToggleCheats.Add(toggle);
                        if (log) MelonLogger.Msg($"Loaded {name} (Toggle Cheat)");
                        break;
                    case ExecutableCheat executable:
                        ExecutableCheats.Add(executable);
                        if (log) MelonLogger.Msg($"Loaded {name} (Executable Cheat)");
                        break;
                    default:
                        if (log) MelonLogger.Msg($"Loaded {name} (Cheat)");
                        break;
                }
            }
            Settings.SaveDefaultSettings();
            Settings.LoadSettings();
            Settings.SaveSettings();
            foreach (MethodBase methodBase in HarmonyInstance.GetPatchedMethods()) MelonLogger.Msg($"Patched method {methodBase.DeclaringType?.FullName}.{methodBase.Name}");
            MelonLogger.Msg($"{Info.Name} {Info.Version} is loaded!");
        }

        public override void OnUpdate()
        {
            bool anyKeybindWaiting = Cheats.Any(c => c.WaitingForKeybind);
            foreach (Cheat cheat in Cheats)
            {
                bool keyDown = cheat is
                {
                    HasKeybind: true,
                    WaitingForKeybind: false
                } && !anyKeybindWaiting && Input.GetKeyDown(cheat.Keybind);
                switch (cheat)
                {
                    case ToggleCheat toggleCheat:
                        if (keyDown) toggleCheat.Enabled = !toggleCheat.Enabled;
                        toggleCheat.Update();
                        break;
                    case ExecutableCheat executableCheat:
                        if (keyDown) executableCheat.Execute();
                        break;
                }
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (Cheat.Instance<DebugMode>() is { Enabled: true }) MelonLogger.Msg($"Scene changed to '{sceneName}'");
            ToggleCheats.ForEach(tc => tc.OnSceneChanged(sceneName));
        }
    }
}