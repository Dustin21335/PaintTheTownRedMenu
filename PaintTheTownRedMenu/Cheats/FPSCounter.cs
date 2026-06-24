using PaintTheTownRedMenu.Cheats.Core;
using UnityEngine;

namespace PaintTheTownRedMenu.Cheats
{
    public class FPSCounter() : ToggleCheat("FPS Counter")
    {
        public int FPS { get; private set; }

        private int _frameCount;
        private float _elapsedTime;

        public override void Update()
        {
            _frameCount++;
            _elapsedTime += Time.unscaledDeltaTime;
            if (_elapsedTime < 1f) return;
            FPS = Mathf.RoundToInt(_frameCount / _elapsedTime);
            _frameCount = 0;
            _elapsedTime = 0f;
        }

        public override void OnDisable()
        {
            FPS = 0;
            _frameCount = 0;
            _elapsedTime = 0f;
        }
    }
}