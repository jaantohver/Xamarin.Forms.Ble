using System;

namespace XamarinBle
{
    public class BLEPeripheralErrorEventArgs : EventArgs
    {
        public string Error { get; set; }

        public IBLEPeripheral Peripheral { get; set; }
    }
}