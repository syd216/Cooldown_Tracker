using NAudio.Wave;

namespace Cooldown_Tracker.CS_Contexts
{
    public class ContextAudioInstances
    {
        public List<WaveOutEvent> Output { get; set; } = new();
        public List<AudioFileReader> Reader { get; set; } = new();
        public Dictionary<String, Task> ActiveDelayedTasks = new();
    }
}
