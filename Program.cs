using Cooldown_Tracker.UIStates;

namespace Cooldown_Tracker
{
    class Program
    {
        // TabPageUIState instance must be created here as the KeyHook also needs to be aware of each panel
        // It will process the key pressed, then each property of that skill from its cooldown, sound file, etc.
        static TabPageUIState _tabPageUIState = new TabPageUIState();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            KeyHook_SoundPlay keyHook = new KeyHook_SoundPlay(_tabPageUIState);
            Application.Run(new Form1(_tabPageUIState));
        }
    }
}