namespace Cooldown_Tracker.FormHandlers
{
    public class TabPanelHandler
    {
        public void AddTabPanel(string character, TabControl tabControl)
        {
            if (!tabControl.TabPages.ContainsKey(character))
            {
                TabPage newPage = new TabPage();
                newPage.Name = character;
                newPage.Text = character;
                newPage.AutoScroll = true;

                tabControl.Controls.Add(newPage);
                tabControl.SelectedTab = newPage;
            }
            else
            {
                MessageBox.Show("That character already exists!", "Notice");
            }
        }
    }
}
