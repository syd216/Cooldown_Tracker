namespace Cooldown_Tracker.CS_Contexts
{
    public class ContextCharacters
    {
        public required TabControl MainTabControl { get; set; }
        public TabPage? CurrentTabPage { get; set; }
        public List<String> Characters { get; set; } = new();
    }
}
