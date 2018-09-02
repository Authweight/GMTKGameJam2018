using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public static class PotatoSwitchEvents
    {
        private static List<Action> listeners = new List<Action>();

        public static void RegisterListener(Action listener)
        {
            listeners.Add(listener);
        }

        public static void SwitchTriggered()
        {
            listeners.ForEach(x => x.Invoke());
        }
    }
}
