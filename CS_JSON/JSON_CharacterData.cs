
namespace Cooldown_Tracker.CS_JSON
{
    public class JSON_CharacterData
    {
        // to be initialized and finalized in SaveToFile.cs
        public required string CharacterName { get; set; }
        public List<JSON_SkillsData> Skills { get; set; } = new(); 
    }
}
