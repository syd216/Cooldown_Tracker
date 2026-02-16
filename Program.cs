using Cooldown_Tracker.UIStates;

namespace Cooldown_Tracker
{
    class Program
    {
        static TabPageUIState _tabPageUIState = new TabPageUIState();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            KeyHook keyHook = new KeyHook(_tabPageUIState);
            Application.Run(new Form1(_tabPageUIState));
        }
    }
}