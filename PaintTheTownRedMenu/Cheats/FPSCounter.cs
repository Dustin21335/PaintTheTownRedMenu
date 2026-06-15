using MelonLoader;
using PaintTheTownRedMenu.Cheats.Core;
using System.Collections;
using UnityEngine;

namespace PaintTheTownRedMenu.Cheats
{
    public class FPSCounter : ToggleCheat
    {
        public int FPS { get; private set; }
        private object? _fpsCounterCoroutine;

        public override string GetName()
        {
            return "FPS Counter";
        }

        public override void OnEnable()
        {
            _fpsCounterCoroutine ??= MelonCoroutines.Start(FPSCounterCoroutine());
        }

        public override void OnDisable()
        {
            if (_fpsCounterCoroutine != null) MelonCoroutines.Stop(_fpsCounterCoroutine);
            _fpsCounterCoroutine = null;
            FPS = 0;
        }

        private IEnumerator FPSCounterCoroutine()
        {
            int frames = 0;
            float elapsed = 0f;
            while (true)
            {
                frames++;
                elapsed += Time.unscaledDeltaTime;
                if (elapsed >= 1f)
                {
                    FPS = Mathf.RoundToInt(frames / elapsed);
                    frames = 0;
                    elapsed = 0f;
                }
                yield return null; 
            }
        }
    }
}