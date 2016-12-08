using System;

namespace XamarinBle
{
    public class BLECharacteristicEventArgs : EventArgs
    {
        public string Error { get; set; }

        public IBLECharacteristic Characteristic { get; set; }
    }
}