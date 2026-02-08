using Cooldown_Tracker.FormHandlers;
using Cooldown_Tracker.UIStates;
using System.Runtime.InteropServices;

namespace Cooldown_Tracker
{
    public partial class Form1 : Form
    {
        private NotifyIcon trayIcon;

        // classes
        private readonly SkillPanelHandler _skillPanelHandler = new SkillPanelHandler();
        private readonly TabPageUIState _tabPageUIState = new TabPageUIState();

        // general vars
        TabPage currentTabPage;
        int tabIndex = 0;

        public Form1()
        {
            AllocConsole();
            InitializeComponent();

            trayIcon = new NotifyIcon();
            trayIcon.Icon = SystemIcons.Application; // replace with your own .ico later
            trayIcon.Text = "Cooldown Tracker";
            trayIcon.Visible = true;

            trayIcon.DoubleClick += TrayIcon_DoubleClick;

            // temporary
            currentTabPage = tabControl1.TabPages[tabControl1.TabIndex - 1];
            tabIndex = tabControl1.TabIndex - 1;

            List<Panel> panels = new List<Panel>();
            foreach (Control c in currentTabPage.Controls)
            {
                if (c is Panel) { panels.Add((Panel)c); }
            }

            _tabPageUIState.tabPageSkillPanelList.Add(panels);
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
        }

        private void AddSkillButton_Click(object sender, EventArgs e)
        {
            currentTabPage.Controls.Add(_skillPanelHandler.AddSkill(
                currentTabPage,
                _tabPageUIState.tabPageSkillPanelList[tabIndex]));
        }
    }
}
