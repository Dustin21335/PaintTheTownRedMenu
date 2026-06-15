using UnityEngine;

namespace PaintTheTownRedMenu.Utils
{
    public static class CameraUtil
    {
        public static Camera? GetActiveCamera()
        {
            return Camera.main;
        }
    }
}