using Cooldown_Tracker.CS_Contexts;
using Cooldown_Tracker.CS_JSON;
using Cooldown_Tracker.CS_Utility;
using Cooldown_Tracker.FormHandlers;
using Cooldown_Tracker.Properties;
using Cooldown_Tracker.UIStates;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Cooldown_Tracker
{
    public partial class Form1 : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;

        // classes
        private readonly TabPageUIState _tabPageUIState; // HOLDS ALL TAB PAGE INFORMATION
        private ContextCharacters _contextCharacters;

        private readonly SkillPanelHandler _skillPanelHandler = new SkillPanelHandler();
        private readonly TabPanelHandler _tabPanelHandler = new TabPanelHandler();
        private readonly SaveToFile _saveToFile = new SaveToFile();

        public Form1(TabPageUIState tabPageUIState)
        {
            AllocConsole();
            InitializeComponent();

            // allows key reading at the form before the controls can
            // used in OnKeyDown function
            this.KeyPreview = true;

            _tabPageUIState = tabPageUIState;

            _contextCharacters = new ContextCharacters
            {
                MainTabControl = tabControl1
                //Characters = new() by default
            };

            // SETUP APPLICATION TRAY ICON
            trayIcon = new NotifyIcon();
            trayIcon.Icon = Resources.ClockIco;
            trayIcon.Text = "Cooldown Tracker";
            trayIcon.Visible = true;

            trayMenu = new ContextMenuStrip();

            // very shorthand in-line function to set define the event actions here instead of elsewhere (senders, args)
            var hideOption = new ToolStripMenuItem("Hide");
            hideOption.Click += (s, e) => { this.Hide(); };
            var exitOption = new ToolStripMenuItem("Exit");
            exitOption.Click += (s, e) =>
            {
                trayIcon.Visible = false;
                Application.Exit();
            };

            trayMenu.Items.Add(hideOption);
            trayMenu.Items.Add(exitOption);

            trayIcon.ContextMenuStrip = trayMenu;
            trayIcon.DoubleClick += TrayIcon_DoubleClick;
            // SETUP APPLICATION TRAY ICON

            // LOAD CHARACTER DATA FROM JSON
            LoadJSON();
            // LOAD CHARACTER DATA FROM JSON
        }

        private void LoadJSON()
        {
            Dictionary<String, List<JSON_SkillData>> loadedData = new GetFromFile().LoadJson();

            int tabPageIndex = 0;
            foreach (string characterName in loadedData.Keys)
            {
                _tabPageUIState.panelsByTabPageDict.Add(characterName, new List<Panel>());
                _tabPanelHandler.AddTabPanel(characterName, tabControl1);

                foreach (JSON_SkillData skill in loadedData[characterName])
                {
                    Panel skillPanel = _skillPanelHandler.AddSkillFromJSON(
                        tabControl1.TabPages[tabPageIndex],
                        _tabPageUIState.panelsByTabPageDict[characterName],
                        skill);

                    tabControl1.TabPages[tabPageIndex].Controls.Add(skillPanel);
                }

                tabPageIndex++;
            }

            _contextCharacters.CurrentTabPage = tabControl1.TabPages[tabControl1.TabCount - 1];
            _tabPageUIState.CurrentTabPage = tabControl1.TabPages[tabControl1.TabCount - 1];
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                if (iconTrayCheck.Checked) { this.Hide(); }
            }
        }

        private void TrayIcon_DoubleClick(object? sender, EventArgs e)
        {
            // show form, set window state to normal, focus window
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.Focus();
        }

        private void AddSkillButton_Click(object sender, EventArgs e)
        {
            if (_contextCharacters.CurrentTabPage != null && tabControl1.TabCount > 0)
            {
                TabPage currentTabPage = _contextCharacters.CurrentTabPage;

                List<Panel> skillPanelList = _tabPageUIState.panelsByTabPageDict[currentTabPage.Name];

                Panel skillPanel = _skillPanelHandler.AddSkill(
                    currentTabPage,
                    skillPanelList
                );

                currentTabPage.Controls.Add(skillPanel);
            }
        }

        private void AddCharacterButton_Click(object sender, EventArgs e)
        {
            // track old tab count to make sure something is actually added later
            int oldTabCount = tabControl1.TabCount;

            using (Form_AddCharacter form =
                new Form_AddCharacter(_contextCharacters, _tabPanelHandler))
            {
                form.StartPosition = FormStartPosition.Manual;
                form.Location = new Point(
                    (this.Location.X + this.Width) - form.Width,
                    (this.Location.Y + this.Height / 2) - form.Height / 2);

                form.ShowDialog();
            }

            // oldTabCount check here
            if (tabControl1.TabCount > oldTabCount)
            {
                int index = tabControl1.SelectedIndex;

                // current index is equal to the amount of tabs minus one. Therefore set current tab to the new tab
                index = tabControl1.TabCount - 1;

                // set current tab page variables (yes there is two, one for UI logic and KeyHook_SoundPlay)
                _contextCharacters.CurrentTabPage = tabControl1.TabPages[index];
                _tabPageUIState.CurrentTabPage = tabControl1.TabPages[index]; 

                // make sure internal tab list tracker has a new list for panels every time a new tab page is made
                while (_tabPageUIState.panelsByTabPageDict.Count <= index)
                {
                    _tabPageUIState.panelsByTabPageDict.Add(_contextCharacters.CurrentTabPage.Name, new List<Panel>());
                }
                //Console.WriteLine($"ADDED: {_contextCharacters.CurrentTabPage.Name}");
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.TabCount > 0)
            {
                _contextCharacters.CurrentTabPage = tabControl1.TabPages[tabControl1.SelectedIndex];
                _tabPageUIState.CurrentTabPage = tabControl1.TabPages[tabControl1.SelectedIndex];
            }
        }

        private void DeleteCharacterButton_Click(object sender, EventArgs e)
        {
            if (_contextCharacters.CurrentTabPage != null && tabControl1.TabCount > 0)
            {
                TabPage targetTabPage = _contextCharacters.CurrentTabPage;

                tabControl1.TabPages.Remove(targetTabPage);
                _tabPageUIState.panelsByTabPageDict.Remove(targetTabPage.Name);

                Console.WriteLine("after delete: " + tabControl1.TabCount);
                Console.WriteLine($"after delete @ (LIST): " + _tabPageUIState.panelsByTabPageDict.Keys.Count);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            List<TabPage> tabPages = new List<TabPage>();

            foreach (TabPage tp in tabControl1.TabPages)
            {
                tabPages.Add(tp);
            }

            _saveToFile.WriteToFile(tabPages, _tabPageUIState);
        }

        // since KeyPreview = true, this can run before any other control can utilize the key press
        // there is no way to unfocus a textbox, so use escape to unfocus from textbox
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.ActiveControl = null;

                // suppress ding sound from escape key being pressed
                e.Handled = true; // means the key was processed
                e.SuppressKeyPress = true; // prevents system notification sound
            }

            base.OnKeyDown(e);
        }
    }
}
