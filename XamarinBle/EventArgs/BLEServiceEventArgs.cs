using System;

namespace XamarinBle
{
    public class BLEServiceEventArgs : EventArgs
    {
        public string Error { get; set; }

        public IBLEService Service { get; set; }
    }
}