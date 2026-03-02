using Cooldown_Tracker.CS_Contexts;
using Cooldown_Tracker.UIStates;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NAudio.Wave;
using Cooldown_Tracker.CS_AudioContainers;

namespace Cooldown_Tracker
{
    public class KeyHook_SoundPlay
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;

        // TabPageUIState required here as keyhook needs the panel properties to 
        // properly play sounds and delay them
        readonly TabPageUIState _tabPageUIState;

        // SOUND HANDLING
        private Dictionary<String, CancellationTokenSource> _activeCooldowns = new();
        private Dictionary<String, AudioOutputContainer> _audioOutputContainers = new();

        public KeyHook_SoundPlay(TabPageUIState tabUIState)
        {
            _tabPageUIState = tabUIState;

            _proc = HookCallback;
            _hookID = SetHook(_proc);
            ApplicationConfiguration.Initialize();
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // check if edit mode is off here too
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN && _tabPageUIState.IsTabActive == true)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                CheckKeys(((Keys)vkCode).ToString());// send read key to string
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        // begin awaited async functions if matched key
        private async void CheckKeys(string pressedKey)
        {
            if (_tabPageUIState.panelsByTabPageDict.Keys.Count > 0 && _tabPageUIState.CurrentTabPage != null)
            {
                foreach (String characterName in _tabPageUIState.panelsByTabPageDict.Keys)
                {
                    // check if the key aligns with the currently selected tab page (character)
                    if (characterName == _tabPageUIState.CurrentTabPage.Name)
                    {
                        // for each skill in that character tab page
                        foreach (Panel p in _tabPageUIState.panelsByTabPageDict[characterName])
                        {
                            if (p.Tag is ContextSkillPanel csp)
                            {
                                // if the current key matches the iterated skill key 
                                if (csp.SkillKeyTextBox.Text.ToUpper() == pressedKey.ToUpper())
                                {
                                    // throw away variable for playing the sound. These are executed async
                                    // and handle sound playing/canellation itself
                                    // input validation here too (only need to validate time and SkillSFXPath

                                    bool validate_Time = int.TryParse(csp.SkillTimeTextBox.Text, out int time);
                                    bool validate_Path = File.Exists(csp.SkillSFXPathTextBox.Text);

                                    if (!validate_Time) { MessageBox.Show($"Invalid time input for skill: {csp.SkillNameTextBox.Text}", "Notice"); return; }
                                    if (!validate_Path) { MessageBox.Show($"Invalid SFX path input for skill: {csp.SkillNameTextBox.Text}", "Notice"); return; }

                                    _ = PlaySoundAfterDelay(
                                        time,
                                        csp.SkillNameTextBox.Text,
                                        csp.SkillSFXPathTextBox.Text);
                                }
                            }
                        }
                    }
                }
            }
        }

        private async Task PlaySoundAfterDelay(int delayMs, string skillName, string sfxPath)
        {
            // this is how the program prevents the same skill from being called multiple times
            // if, in a dictionary, it contains the key/name of the skill --> return
            if (_activeCooldowns.ContainsKey(skillName))
            {
                _activeCooldowns[skillName].Cancel();
                _activeCooldowns[skillName].Dispose();
                _activeCooldowns.Remove(skillName);

                Console.WriteLine("Cooldown Cancelled");
                return; 
            }

            // Create cancellation token
            CancellationTokenSource cts = new CancellationTokenSource();
            _activeCooldowns[skillName] = cts;

            try
            {
                await Task.Delay(delayMs * 1000, cts.Token);

                // if task is not yet cancelled, play sound
                AudioOutputContainer AOU = new AudioOutputContainer
                {
                    audioFile = new AudioFileReader(sfxPath),
                    outputDevice = new WaveOutEvent(),
                };
                _audioOutputContainers.Add(skillName, AOU);

                AOU.outputDevice.Init(AOU.audioFile);
                AOU.outputDevice.Play();

                // pass event into outputDevice to cleanup itself after finishing playback
                AOU.outputDevice.PlaybackStopped += (s, e) =>
                {
                    AOU.audioFile.Dispose();
                    AOU.outputDevice.Dispose();
                    _activeCooldowns.Remove(skillName);
                    _audioOutputContainers.Remove(skillName);
                };

                Console.WriteLine($"EXECUTED AFTER DELAY: {skillName} ({delayMs} S)");
            }
            catch (TaskCanceledException) // if the CTS is cancelled in a new thread
            {
                // the previously created thread for this skill will move here
                Console.WriteLine($"CANCELLED BEFORE EXECUTION: {skillName}");
            }
            finally
            {
                if (_activeCooldowns.ContainsKey(skillName))
                {
                    _activeCooldowns[skillName].Dispose();
                    _activeCooldowns.Remove(skillName);
                }
            }
        }

        // WINDOW DLL IMPORTS
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
