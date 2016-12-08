using System;
using System.Collections.Generic;

namespace XamarinBle
{
    public class BLEDiscoveredPeripheralEventArgs : EventArgs
    {
        public List<KeyValuePair<string, string>> AdvertisementData { get; set; }

        public IBLEPeripheral Peripheral { get; set; }

        public int RSSI { get; set; }
    }
}