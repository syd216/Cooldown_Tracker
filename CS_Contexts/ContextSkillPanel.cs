
namespace Cooldown_Tracker.CS_Contexts
{
    public class ContextSkillPanel
    {
        public required TabPage ParentTabPage { get; set; }
        public required Panel SkillPanel { get; set; }
        public required TextBox SkillKeyTextBox { get; set; }
        public required List<Panel> SkillPanelList { get; set; }
    }
}
