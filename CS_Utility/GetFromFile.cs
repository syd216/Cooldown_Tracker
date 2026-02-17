using System.Text.Json;
using System.IO;
using Cooldown_Tracker.CS_JSON;

namespace Cooldown_Tracker.CS_Utility
{
    public class GetFromFile
    {
        public Dictionary<String, List<JSON_SkillData>> LoadJson()
        {
            String json = File.ReadAllText(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cooldown_Tracker_Data.json"));

            List<JSON_CharacterData>? characters = JsonSerializer.Deserialize<List<JSON_CharacterData>>(json);
            Dictionary<String, List<JSON_SkillData>> loadedData = new Dictionary<string, List<JSON_SkillData>>();
            
            if (characters != null)
            {
                foreach (var character in characters)
                {
                    List<JSON_SkillData> loadedSkills = new List<JSON_SkillData>();

                    foreach (var skill in character.Skills)
                    {
                        loadedSkills.Add(new JSON_SkillData()
                        {
                            SkillName = skill.SkillName,
                            SkillKey = skill.SkillKey,
                            SkillTime = skill.SkillTime,
                            SkillSFXPath = skill.SkillSFXPath
                        });
                    }

                    loadedData.Add(character.CharacterName, loadedSkills);
                }
            }

            return loadedData;
        }
    }
}
