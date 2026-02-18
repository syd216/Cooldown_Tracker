namespace Cooldown_Tracker.UIStates
{
    public class TabPageUIState
    {
        // helper class to contain a list of certain UI elements in each tab page
        // this is a list of a list, so each initial list entry can hold a list of panels (2D)
        // for example key1 = Character1Name, List<Panel> = all of its skills in that tab page
        public Dictionary<String, List<Panel>> panelsByTabPageDict { get; set; } = new();
        public TabPage? CurrentTabPage { get; set; }
        public bool IsTabActive { get; set; }
    }
}
