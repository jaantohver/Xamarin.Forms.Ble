using System;
using System.Collections.Generic;

using Foundation;
using CoreBluetooth;

namespace XamarinBle.iOS
{
    public class BLEManager : IBLEManager
    {
        public event EventHandler UpdatedState;
        public event EventHandler<BLEPeripheralEventArgs> ConnectedToPeripheral;
        public event EventHandler<BLEPeripheralErrorEventArgs> DisconnectedFromPeripheral;
        public event EventHandler<BLEDiscoveredPeripheralEventArgs> DiscoveredPeripheral;

        readonly CBCentralManager nativeManager;

        public bool IsScanning {
            get {
                return nativeManager.IsScanning;
            }
        }

        BLEManagerState state;
        public BLEManagerState State {
            get {
                return state;
            }
        }

        List<BLEPeripheral> discoveredPeripherals;

        public BLEManager ()
        {
            discoveredPeripherals = new List<BLEPeripheral> ();

            state = BLEManagerState.Unknown;

            nativeManager = new CBCentralManager ();
            nativeManager.UpdatedState += OnUpdatedState;
            nativeManager.DiscoveredPeripheral += OnDiscoveredPeripheral;
            nativeManager.ConnectedPeripheral += OnConnectedPeripheral;
            nativeManager.DisconnectedPeripheral += OnDisconnectedPeripheral;
        }

        public void ConnectToPeripheral (IBLEPeripheral peripheral)
        {
            nativeManager.ConnectPeripheral (((peripheral as BLEPeripheral).NativePeripheral as CBPeripheral));
        }

        public void DisconnectFromPeripheral (IBLEPeripheral peripheral)
        {
            nativeManager.CancelPeripheralConnection (((peripheral as BLEPeripheral).NativePeripheral as CBPeripheral));
        }

        public void ScanForPeripherals ()
        {
            nativeManager.ScanForPeripherals (peripheralUuids: null);
        }

        public void StopScanning ()
        {
            nativeManager.StopScan ();
        }

        void OnUpdatedState (object sender, EventArgs e)
        {
            switch (nativeManager.State) {
            case CBCentralManagerState.PoweredOff:
                state = BLEManagerState.PoweredOff;
                break;
            case CBCentralManagerState.PoweredOn:
                state = BLEManagerState.PoweredOn;
                break;
            case CBCentralManagerState.Resetting:
                state = BLEManagerState.Resetting;
                break;
            case CBCentralManagerState.Unauthorized:
                state = BLEManagerState.Unauthorized;
                break;
            case CBCentralManagerState.Unsupported:
                state = BLEManagerState.Unsupported;
                break;
            default:
                state = BLEManagerState.Unknown;
                break;
            }

            EventHandler handler = UpdatedState;
            if (handler != null) {
                handler (this, EventArgs.Empty);
            }
        }

        void OnDiscoveredPeripheral (object sender, CBDiscoveredPeripheralEventArgs e)
        {
            BLEPeripheral peripheral = null;

            foreach (BLEPeripheral blep in discoveredPeripherals) {
                if (blep.UUID.StringValue.Equals (e.Peripheral.ToString ())) {
                    peripheral = blep;

                    break;
                }
            }

            if (peripheral == null) {
                peripheral = new BLEPeripheral (e.Peripheral);

                discoveredPeripherals.Add (peripheral);
            }

            EventHandler<BLEDiscoveredPeripheralEventArgs> handler = DiscoveredPeripheral;
            if (handler != null) {
                BLEDiscoveredPeripheralEventArgs args = new BLEDiscoveredPeripheralEventArgs ();
                args.AdvertisementData = new List<KeyValuePair<string, string>> ();
                args.Peripheral = peripheral;
                args.RSSI = e.RSSI.Int32Value;

                foreach (KeyValuePair<NSObject, NSObject> kvp in e.AdvertisementData) {
                    args.AdvertisementData.Add (new KeyValuePair<string, string> (kvp.Key.ToString (), kvp.Value.ToString ()));
                }

                handler (this, args);
            }
        }

        void OnConnectedPeripheral (object sender, CBPeripheralEventArgs e)
        {
            BLEPeripheral peripheral = null;

            foreach (BLEPeripheral blep in discoveredPeripherals) {
                if (blep.UUID.StringValue == e.Peripheral.ToString ()) {
                    peripheral = blep;

                    break;
                }
            }

            EventHandler<BLEPeripheralEventArgs> handler = ConnectedToPeripheral;
            if (handler != null) {
                BLEPeripheralEventArgs args = new BLEPeripheralEventArgs ();
                args.Peripheral = peripheral;

                handler (this, args);
            }
        }

        void OnDisconnectedPeripheral (object sender, CBPeripheralErrorEventArgs e)
        {
            BLEPeripheral peripheral = null;

            foreach (BLEPeripheral blep in discoveredPeripherals) {
                if (blep.UUID.StringValue == e.Peripheral.ToString ()) {
                    peripheral = blep;

                    break;
                }
            }

            EventHandler<BLEPeripheralErrorEventArgs> handler = DisconnectedFromPeripheral;
            if (handler != null) {
                BLEPeripheralErrorEventArgs args = new BLEPeripheralErrorEventArgs ();
                args.Peripheral = peripheral;
                args.Error = e.Error?.ToString ();
            }
        }
    }
}