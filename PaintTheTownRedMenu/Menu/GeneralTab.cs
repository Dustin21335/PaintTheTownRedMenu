using PaintTheTownRedMenu.Menu.Core;
using PaintTheTownRedMenu.Utils;

namespace PaintTheTownRedMenu.Menu
{
    public class GeneralTab() : Tab("General")
    {
        public override void Render()
        {
            UIUtil.Text("Thank you for using the Paint The Town Red Menu!");
        }
    }
}
