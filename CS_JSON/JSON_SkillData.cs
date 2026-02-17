
namespace Cooldown_Tracker.CS_JSON
{
    public class JSON_SkillData
    {
        // to be initialized and finalized in SaveToFile.cs
        // gets passed into CharacterData JSON model
        // also used for loading data for convenient data strucuter and access
        public required string SkillName { get; set; }
        public required string SkillKey { get; set; }
        public required string SkillTime { get; set; }
        public required string SkillSFXPath { get; set; }
    }
}
