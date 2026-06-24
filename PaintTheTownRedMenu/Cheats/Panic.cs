using PaintTheTownRedMenu.Cheats.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PaintTheTownRedMenu.Cheats
{
    public class Panic() : ToggleCheat("Panic", KeyCode.Pause)
    {
        private readonly HashSet<ToggleCheat> _previouslyEnabledCheats = [];

        public override void OnEnable()
        {
            foreach (ToggleCheat toggleCheat in PaintTheTownRedMenuMod.Instance.ToggleCheats.Where(tc => tc.Enabled && tc != this))
            {
                _previouslyEnabledCheats.Add(toggleCheat);
                toggleCheat.Enabled = false;
            }
        }

        public override void OnDisable()
        {
            foreach (ToggleCheat toggleCheat in _previouslyEnabledCheats) toggleCheat.Enabled = true;
            _previouslyEnabledCheats.Clear();
        }
    }
}
