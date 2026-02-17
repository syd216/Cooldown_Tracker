using Cooldown_Tracker.CS_Contexts;
using Cooldown_Tracker.CS_Utility;
using Cooldown_Tracker.UIStates;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NAudio.Wave;

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
        private Dictionary<String, Task> _activeDelayedTasks = new Dictionary<string, Task>();

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
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
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
                                if (csp.SkillKeyTextBox.Text.ToUpper() == pressedKey)
                                {
                                    await PrintAfterDelay(
                                        Convert.ToInt32(csp.SkillTimeTextBox.Text),
                                        pressedKey,
                                        csp.SkillSFXPathTextBox.Text);
                                }
                            }
                        }
                    }
                }
            }
        }

        private async Task PrintAfterDelay(int delayMs, string characterName, string sfxPath)
        {
            // this is how the program prevents the same skill from being called multiple times
            // if, in a dictionary, it contains the key/name of the skill --> return
            if (_activeDelayedTasks.ContainsKey(characterName)){ return; }

            // if not, create a new task and add it to that dictionary as it runs
            var task = InternalDelay(characterName, delayMs);
            _activeDelayedTasks[characterName] = task;

            await task;

            // after the delay, play the sound
            var audioFile = new AudioFileReader(sfxPath);
            var outputDevice = new WaveOutEvent();

            outputDevice.Init(audioFile);
            outputDevice.Play();

            // this just executes the sound player as another async task
            // _ is a generic throw away variable declaration
            _ = Task.Run(async () =>
            {
                await Task.Delay((int)audioFile.TotalTime.TotalMilliseconds);
                outputDevice.Dispose();
                audioFile.Dispose();
            });

            // after it has returned, remove it from that dictionary and the skill is available again
            _activeDelayedTasks.Remove(characterName);
            Console.WriteLine($"PRESSED AFTER DELAY: {characterName} ({delayMs} S)");
        }

        private async Task InternalDelay(String key, int delayMs)
        {
            await Task.Delay(delayMs * 1000);
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
