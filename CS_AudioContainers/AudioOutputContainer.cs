using NAudio.Wave;

namespace Cooldown_Tracker.CS_AudioContainers
{
    public class AudioOutputContainer
    {
        public required AudioFileReader audioFile { get; set; }
        public required WaveOutEvent outputDevice { get; set; }
    }
}
