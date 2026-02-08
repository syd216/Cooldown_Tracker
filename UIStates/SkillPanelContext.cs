namespace Cooldown_Tracker.UIStates
{
    public class SkillPanelContext
    {
        public required TabPage CurrentTabPage { get; init; }
        public required Panel CurrentPanel { get; init; }
        public required List<Panel> CurrentPanelList { get; init; }
    }
}
