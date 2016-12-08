using System;
using System.Collections.Generic;

namespace XamarinBle
{
    public interface IBLEPeripheral : IEquatable<IBLEPeripheral>
    {
        void DiscoverServices ();

        void DiscoverCharacteristics (IBLEService service);

        string Name { get; }

        UUID UUID { get; }

        object NativePeripheral { get; }

        List<IBLEService> Services { get; }

        event EventHandler<BLEErrorEventArgs> DiscoveredService;

        event EventHandler<BLEServiceEventArgs> DiscoveredCharacteristic;

        event EventHandler<BLECharacteristicEventArgs> UpdatedCharacteristicValue;
    }
}