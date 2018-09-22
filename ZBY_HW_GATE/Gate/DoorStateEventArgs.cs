using System;

namespace ZBY_HW_GATE.Gate
{
    class DoorStateEventArgs : EventArgs
    {
        public DoorStateEventArgs(int state,Int32 SN)
        {
            this.State = state;
            this.SN = SN;
        }

        public int State { get; private set; }
        public Int32 SN { get; private set; }
    }
}
