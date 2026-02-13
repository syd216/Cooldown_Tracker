using Cooldown_Tracker.CS_Contexts;
using Cooldown_Tracker.UIStates;

namespace Cooldown_Tracker.FormHandlers
{
    public class SkillPanelHandler
    {
        KeyUIState _keyUIState;

        // default values
        int xOffset = 178, yOffset = 198;

        public SkillPanelHandler(KeyUIState keyUIState) 
        { 
            _keyUIState = keyUIState;
        }

        // requires currently focused tab page and list of panels in the tab page
        public Panel AddSkill(TabPage tabPage, List<Panel> skillPanels)
        {
            // PANEL
            Point final_point = PanelPositioning(tabPage, skillPanels);

            Panel panel = new Panel
            {
                Width = 161,
                Height = 150,
                Location = final_point,
                BorderStyle = BorderStyle.FixedSingle
            };

            skillPanels.Add(panel);
            // PANEL

            // TEXT BOXES
            TextBox TB_SkillName = new TextBox
            {
                Name = "TB_SkillName",
                Width = 100,
                Height = 23,
                Location = new Point(3, 3),
                BackColor = Color.Silver
            };

            TextBox TB_Key = new TextBox
            {
                Name = "TB_Key",
                Width = 100,
                Height = 23,
                Location = new Point(3, 32),
            };
            TB_Key.Tag = panel;
            _keyUIState.KeyTextBoxes.Add(TB_Key);

            TextBox TB_Time = new TextBox
            {
                Name = "TB_Time",
                Width = 100,
                Height = 23,
                Location = new Point(3, 61),
            };

            TextBox TB_SFXPath = new TextBox
            {
                Name = "TB_SFXPath",
                Width = 100,
                Height = 23,
                Location = new Point(3, 90),
            };
            // TEXT BOXES

            // LABELS
            Label LBL_SkillName = new Label
            {
                Text = "Skill",
                Location = new Point(111, TB_SkillName.Location.Y),
            };

            Label LBL_Key = new Label
            {
                Text = "Key",
                Location = new Point(111, TB_Key.Location.Y),
            };

            Label LBL_Time = new Label
            {
                Text = "Time (s)",
                Location = new Point(111, TB_Time.Location.Y),
            };
            // LABELS

            // BUTTONS
            Button BTN_SFXPath = new Button
            {
                Name = "BTN_SFXPath",
                Location = new Point(108, TB_SFXPath.Location.Y),
                Width = 45,
                Height = 23,
                Text = "Set",
            };
            BTN_SFXPath.Click += BTN_SFXPath_Click;
            BTN_SFXPath.Tag = TB_SFXPath;

            Button BTN_Delete = new Button
            {
                Name = "BTN_Delete",
                Location = new Point(3, 119),
                Width = 100,
                Height = 23,
                Text = "Delete",
            };
            BTN_Delete.Click += BTN_Delete_Click;
            BTN_Delete.Tag = new ContextSkillPanel
            {
                ParentTabPage = tabPage,
                SkillPanel = panel,
                SkillKeyTextBox = TB_Key,
                SkillPanelList = skillPanels
            };
            // BUTTONS

            panel.Controls.Add(TB_SkillName);
            panel.Controls.Add(TB_Key);
            panel.Controls.Add(TB_Time);
            panel.Controls.Add(TB_SFXPath);

            panel.Controls.Add(LBL_SkillName);
            panel.Controls.Add(LBL_Key);
            panel.Controls.Add(LBL_Time);

            panel.Controls.Add(BTN_SFXPath);
            panel.Controls.Add(BTN_Delete);

            Console.WriteLine(_keyUIState.KeyTextBoxes.Count);

            return panel;
        }

        private Point PanelPositioning(TabPage tabPage, List<Panel> skillPanels)
        {
            int xSpacing = xOffset;
            int ySpacing = yOffset;

            int col = skillPanels.Count / 2;
            int row = skillPanels.Count % 2;

            Point scrollOffset = tabPage.AutoScrollPosition;

            Point final_point = new Point(
                17 + (col * xSpacing) + scrollOffset.X,
                17 + (row * ySpacing) + scrollOffset.Y);

            return final_point;
        }

        private Point PanelRepositioning(TabPage tabPage, Panel panel, List<Panel> skillPanels)
        {
            int xSpacing = xOffset;
            int ySpacing = yOffset;

            int currentIndex = skillPanels.IndexOf(panel); // must be the panel in the updated list

            if (currentIndex == -1) return new Point(0, 0); // safety check

            int col = currentIndex / 2;
            int row = currentIndex % 2;

            Point scrollOffset = tabPage.AutoScrollPosition;

            return new Point(
                17 + (col * xSpacing) + scrollOffset.X,
                17 + (row * ySpacing) + scrollOffset.Y);
        }


        private void BTN_Delete_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn) { return; }

            if (btn.Tag is ContextSkillPanel context)
            {
                // remove the current panel from the form and list
                context.SkillPanelList.Remove(context.SkillPanel);
                context.SkillPanel.Dispose();

                // remove the current text box of this panel from the list
                _keyUIState.KeyTextBoxes.Remove(context.SkillKeyTextBox);

                foreach (Panel p in context.SkillPanelList)
                {
                    p.Location = PanelRepositioning(
                        context.ParentTabPage,
                        p,
                        context.SkillPanelList);
                }
            }
        }

        private void BTN_SFXPath_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn) { return; }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select Audio File";
                ofd.Filter = "Audio Files (*.wav;*.mp3;*.ogg)|*.wav;*.mp3;*.ogg|All Files (*.*)|*.*";
                ofd.Multiselect = false;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string filePath = ofd.FileName;
                    if (btn.Tag is TextBox textBox)
                    {
                        textBox.Text = filePath;
                    }
                }
            }
        }
    }
}
