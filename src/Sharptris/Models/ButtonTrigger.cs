using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharptris.Models
{
    public class ButtonTrigger
    {
        bool triggered;
        bool handled;

        public void Set()
        {
            if (triggered) return;
            if (handled) return;
            triggered = true;
            handled = false;
        }

        public void Reset()
        {
            triggered = false;
            handled = false;
        }

        public override string ToString()
        {
            return $"{triggered}";
        }

        public bool Handled { get => handled; }

        public bool Triggered()
        {
            if (handled) return false;
            if (!triggered) return false;
            handled = true;
            return true;
        }
    }
}
