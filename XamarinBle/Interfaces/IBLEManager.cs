using System;

namespace XamarinBle
{
    public interface IBLEManager
    {
        void ScanForPeripherals ();

        void StopScanning ();

        void ConnectToPeripheral (IBLEPeripheral peripheral);

        void DisconnectFromPeripheral (IBLEPeripheral peripheral);

        bool IsScanning { get; }

        BLEManagerState State { get; }

        event EventHandler UpdatedState;

        event EventHandler<BLEDiscoveredPeripheralEventArgs> DiscoveredPeripheral;

        event EventHandler<BLEPeripheralEventArgs> ConnectedToPeripheral;

        event EventHandler<BLEPeripheralErrorEventArgs> DisconnectedFromPeripheral;
    }
}