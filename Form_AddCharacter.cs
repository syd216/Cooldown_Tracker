using Cooldown_Tracker.CS_Contexts;
using Cooldown_Tracker.FormHandlers;

namespace Cooldown_Tracker
{
    public partial class Form_AddCharacter : Form
    {
        ContextCharacters _contextCharacters;
        TabPanelHandler _tabPanelHandler;

        public Form_AddCharacter(ContextCharacters CC, TabPanelHandler TPH)
        {
            _contextCharacters = CC;
            _tabPanelHandler = TPH;

            InitializeComponent();
        }

        private void ProcessName()
        {
            string tbText = TextBoxCharacterName.Text;

            _contextCharacters.Characters.Add(tbText);
            _tabPanelHandler.AddTabPanel(tbText, _contextCharacters.MainTabControl);
            this.Dispose();
        }

        private void Button_SubmitName_Click(object sender, EventArgs e)
        {
            ProcessName();
        }

        private void TextBoxCharacterName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender != null)
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true; // prevent "ding" sound
                    ProcessName();
                }
            }
        }
    }
}
