
namespace Cooldown_Tracker.CS_JSON
{
    public class JSON_SkillsData
    {
        // to be initialized and finalized in SaveToFile.cs
        // gets passed into CharacterData JSON model
        public required string SkillName { get; set; }
        public required string SkillKey { get; set; }
        public required string SkillTime { get; set; }
        public required string SKillSFXPath { get; set; }
    }
}
