using PaintTheTownRedMenu.Utils;
using System.Linq;
using UnityEngine;

namespace PaintTheTownRedMenu.Cheats.Core
{
    public abstract class Cheat(string name, KeyCode keybind, bool hidden)
    {
        public string Name { get; } = name;
        public KeyCode Keybind { get; set; } = keybind;
        public bool Hidden { get; } = hidden;
        public bool HasKeybind => Keybind != KeyCode.None;
        public bool WaitingForKeybind { get; set; } = false;

        public static T? Instance<T>() where T : Cheat
        {
            return PaintTheTownRedMenuMod.Instance.Cheats.OfType<T>().FirstOrDefault();
        }

        public static bool WorldToScreen(Camera camera, Vector3 world, out Vector2 screen)
        {
            Vector3 position = camera.WorldToScreenPoint(world);
            if (position.z <= 0f)
            {
                screen = default;
                return false;
            }
            screen = new Vector2(position.x, Screen.height - position.y);
            return true;
        }

        public static bool WorldToScreen(Vector3 world, out Vector2 screen)
        {
            Camera? activeCamera = CameraUtil.GetActiveCamera();
            if (activeCamera != null) return WorldToScreen(activeCamera, world, out screen);
            screen = Vector3.zero;
            return false;
        }

        public static float GetDistanceToPlayer(Vector3 position)
        {
            Camera? activeCamera = CameraUtil.GetActiveCamera();
            return activeCamera != null ? Vector3.Distance(activeCamera.transform.position, position) : 0f;
        }
    }
}
