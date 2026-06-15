namespace PaintTheTownRedMenu.Menu.Core
{
    public abstract class Tab(string name, bool enabled = true)
    {
        public string Name { get; } = name;
        public bool Enabled { get; set; } = enabled;

        public abstract void Render();
    }
}