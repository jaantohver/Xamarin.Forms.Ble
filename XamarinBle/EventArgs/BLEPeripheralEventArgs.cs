using System;

namespace XamarinBle
{
    public class BLEPeripheralEventArgs : EventArgs
    {
        public IBLEPeripheral Peripheral { get; set; }
    }
}