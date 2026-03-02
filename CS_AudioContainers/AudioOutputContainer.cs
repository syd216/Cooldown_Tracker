using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cooldown_Tracker.CS_AudioContainers
{
    public class AudioOutputContainer
    {
        public required AudioFileReader audioFile { get; set; }
        public required WaveOutEvent outputDevice { get; set; }
    }
}
