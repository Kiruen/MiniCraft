using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniCraft.Core
{
    public class BitSet
    {
        //private int stateValue;
        //public int Value { get => stateValue; set => stateValue = value; }
        public int StateValue;

        public bool this[int state]
        {
            get {
                state = (1 << state);
                return (StateValue & state) == state; }
            set
            {
                state = (1 << state);
                if (value == true)
                    StateValue |= state;
                else
                    StateValue &= (~state);
            }
        }

        public BitSet(int value)
        {
            StateValue = value;
        }

        public void Switch(int state)
        {
            state = 1 << state;
            StateValue ^= state;
        }

        public override string ToString()
        {
            var str = Convert.ToString(StateValue, 2);
            return str.PadLeft(6, '0');
        }
    }
}
