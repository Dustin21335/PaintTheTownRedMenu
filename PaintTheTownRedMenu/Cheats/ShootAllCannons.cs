using Il2Cpp;
using PaintTheTownRedMenu.Cheats.Core;
using System.Linq;

namespace PaintTheTownRedMenu.Cheats
{
    public class ShootAllCannons : ExecutableCheat
    {
        public override string GetName()
        {
            return "Shoot All Cannons";
        }
        public override void Execute()
        {
            foreach (Cannon cannon in GameObjectManager.Cannons.Where(c => c != null).ToList())
            {
                if (cannon.ammo <= 0) cannon.ammo = 1;
                cannon.DoPressAction();
            }
        }
    }
}
