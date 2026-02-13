using Cooldown_Tracker.CS_Contexts;
using Cooldown_Tracker.FormHandlers;
using Cooldown_Tracker.Properties;
using Cooldown_Tracker.UIStates;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Cooldown_Tracker
{
    public partial class Form1 : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;

        // classes
        private readonly SkillPanelHandler _skillPanelHandler;
        private ContextCharacters _contextCharacters;

        private readonly TabPanelHandler _tabPanelHandler = new TabPanelHandler();
        private readonly TabPageUIState _tabPageUIState = new TabPageUIState();
        private readonly KeyUIState _keyUIState = new KeyUIState();

        public Form1()
        {
            //AllocConsole();
            InitializeComponent();

            _skillPanelHandler = new SkillPanelHandler(_keyUIState);
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
                List<List<Panel>> list = _tabPageUIState.tabPageSkillPanelList;

                TabPage currentTabPage = _contextCharacters.CurrentTabPage;

                Panel skillPanel = _skillPanelHandler.AddSkill(
                    currentTabPage,
                    list[tabControl1.SelectedIndex]
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
                List<List<Panel>> list = _tabPageUIState.tabPageSkillPanelList;

                // current index is equal to the amount of tabs minus one. Therefore set current tab to the new tab
                index = tabControl1.TabCount - 1;
                _contextCharacters.CurrentTabPage = tabControl1.TabPages[index];

                // make sure internal tab list tracker has a new list for panels every time a new tab page is made
                while (list.Count <= index)
                {
                    list.Add(new List<Panel>());
                }
                //Console.WriteLine($"ADDED: {_contextCharacters.CurrentTabPage.Name}");
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.TabCount > 0)
            {
                _contextCharacters.CurrentTabPage = tabControl1.TabPages[tabControl1.SelectedIndex];
            }
            //Console.WriteLine(tabControl1.SelectedIndex);
        }

        private void DeleteCharacterButton_Click(object sender, EventArgs e)
        {
            if (_contextCharacters.CurrentTabPage != null && tabControl1.TabCount > 0)
            {
                int index = tabControl1.SelectedIndex;
                tabControl1.TabPages.Remove(_contextCharacters.CurrentTabPage);
                _tabPageUIState.tabPageSkillPanelList.RemoveAt(index);

                Console.WriteLine("after delete: " + tabControl1.TabCount);
                Console.WriteLine($"after delete @ {index} (LIST): " + _tabPageUIState.tabPageSkillPanelList.Count);
            }
        }
    }
}
