using Cooldown_Tracker.UIStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cooldown_Tracker.FormHandlers
{
    public class SkillPanelHandler
    {
        // requires currently focused tab page and list of panels in the tab page
        public Panel AddSkill(TabPage tabPage, List<Panel> skillPanels)
        {
            // PANEL
            int xSpacing = 178;
            int ySpacing = 200;

            int col = skillPanels.Count / 2;
            int row = skillPanels.Count % 2;

            Point scrollOffset = tabPage.AutoScrollPosition;

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

            Console.WriteLine(skillPanels.Count);

            // TEXT BOXES
            TextBox TB_SkillName = new TextBox
            {
                Width = 100,
                Height = 23,
                Location = new Point(3, 3),
                BackColor = Color.Silver
            };

            TextBox TB_Key = new TextBox
            {
                Width = 100,
                Height = 23,
                Location = new Point(3, 32),
            };

            TextBox TB_Time = new TextBox
            {
                Width = 100,
                Height = 23,
                Location = new Point(3, 61),
            };

            TextBox TB_SFXPath = new TextBox
            {
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

            Button BTN_Delete = new Button
            {
                Name = "BTN_Delete",
                Location = new Point(3, 119),
                Width = 100,
                Height = 23,
                Text = "Delete",
            };
            BTN_Delete.Click += BTN_Delete_Click;
            BTN_Delete.Tag = new SkillPanelContext
            {
                CurrentTabPage = tabPage,
                CurrentPanel = panel,
                CurrentPanelList = skillPanels
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

            return panel;
        }

        private Point PanelPositioning(TabPage tabPage, List<Panel> skillPanels)
        {
            int xSpacing = 178;
            int ySpacing = 200;

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
            int xSpacing = 178;
            int ySpacing = 200;

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

            if (btn.Tag is SkillPanelContext context)
            {
                context.CurrentPanelList.Remove(context.CurrentPanel);
                context.CurrentPanel.Dispose();

                foreach (Panel p in context.CurrentPanelList)
                {
                    p.Location = PanelRepositioning(
                        context.CurrentTabPage,
                        p,
                        context.CurrentPanelList);
                }
            }
        }
    }
}
