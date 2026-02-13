using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cooldown_Tracker.UIStates
{
    public class TabPageUIState
    {
        // helper class to contain a list of certain UI elements in each tab page
        // this is a list of a list, so each initial list entry can hold a list of panels (2D)
        public List<List<Panel>> tabPageSkillPanelList { get; set; } = new();
    }
}
