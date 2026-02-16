
using Cooldown_Tracker.CS_Contexts;
using Cooldown_Tracker.CS_JSON;
using Cooldown_Tracker.UIStates;
using System.Text.Json;

namespace Cooldown_Tracker.CS_Utility
{
    public class SaveToFile
    {
        public void WriteToFile(List<TabPage> tabPages, TabPageUIState tabPageUIState)
        {
            String outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "save.json");

            List<JSON_CharacterData> characters = new List<JSON_CharacterData>();

            // get all the information from the skill panels
            // for each tab page in tabPages, add that tab page name to the characters list
            // for each control in the tab page, get each panel and the context from the tag
            // the context contains everything about that skill panel
            foreach (TabPage tp in tabPages)
            {
                // initialize JSON_CharacterData to prepare final pass to save file
                var character = new JSON_CharacterData { CharacterName = tp.Name };

                foreach (Control c in tp.Controls)
                {
                    if (c is Panel)
                    {
                        if (c.Tag is ContextSkillPanel csp)
                        {
                            character.Skills.Add(new JSON_SkillsData
                            {
                                SkillName = csp.SkillNameTextBox.Text,
                                SkillKey = csp.SkillKeyTextBox.Text,
                                SkillTime = csp.SkillTimeTextBox.Text,
                                SKillSFXPath = csp.SkillSFXPathTextBox.Text
                            });
                        }
                    }
                }

                characters.Add(character);
            }

            String json = JsonSerializer.Serialize(characters, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            Console.WriteLine(json);

            // ContextSkillPanel.cs variables
            // public required TabPage ParentTabPage { get; set; }
            // public required Panel SkillPanel { get; set; }
            // public required TextBox SkillKeyTextBox { get; set; }
            // public required TextBox SkillNameTextBox { get; set; }
            // public required List<Panel> SkillPanelList { get; set; }
        }
    }
}
