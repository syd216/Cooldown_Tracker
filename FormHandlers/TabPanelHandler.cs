namespace Cooldown_Tracker.FormHandlers
{
    public class TabPanelHandler
    {
        public void AddTabPanel(string character, TabControl tabControl)
        {
            TabPage newPage = new TabPage();
            newPage.Name = character;
            newPage.Text = character;
            newPage.AutoScroll = true;

            tabControl.Controls.Add(newPage);
            tabControl.SelectedTab = newPage;
        }
    }
}
