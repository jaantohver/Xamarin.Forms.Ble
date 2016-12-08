#pragma warning disable XA0001 // Find issues with Android API usage
using System;
using System.Collections.Generic;

using Android.App;
using Android.Bluetooth;
using Android.Bluetooth.LE;

namespace XamarinBle.Droid
{
    public class BLEManager : ScanCallback, IBLEManager
    {
        public event EventHandler UpdatedState;
        public event EventHandler<BLEPeripheralEventArgs> ConnectedToPeripheral;
        public event EventHandler<BLEDiscoveredPeripheralEventArgs> DiscoveredPeripheral;
        public event EventHandler<BLEPeripheralErrorEventArgs> DisconnectedFromPeripheral;

        readonly BluetoothManager nativeManager;

        bool isScanning;
        public bool IsScanning {
            get {
                return isScanning;
            }
        }

        //FIXME always unknown
        BLEManagerState state;
        public BLEManagerState State {
            get {
                return state;
            }
        }

        readonly List<BLEPeripheral> discoveredPeripherals;

        public BLEManager ()
        {
            discoveredPeripherals = new List<BLEPeripheral> ();

            nativeManager = (BluetoothManager)Application.Context.GetSystemService ("bluetooth");

            state = BLEManagerState.Unknown;
        }

        public void ConnectToPeripheral (IBLEPeripheral peripheral)
        {
            (peripheral as BLEPeripheral).Connect ();
        }

        public void DisconnectFromPeripheral (IBLEPeripheral peripheral)
        {
            (peripheral as BLEPeripheral).Disconnect ();
        }

        public void ScanForPeripherals ()
        {
            isScanning = true;

            nativeManager.Adapter.BluetoothLeScanner.StartScan (this);
        }

        public void StopScanning ()
        {
            nativeManager.Adapter.BluetoothLeScanner.StopScan (this);

            isScanning = false;
        }

        public override void OnScanResult (ScanCallbackType callbackType, ScanResult result)
        {
            base.OnScanResult (callbackType, result);

            BLEPeripheral peripheral = null;

            foreach (BLEPeripheral blep in discoveredPeripherals) {
                if (blep.UUID.StringValue == result.Device.Address) {
                    peripheral = blep;

                    break;
                }
            }

            if (peripheral == null) {
                peripheral = new BLEPeripheral (result.Device);
                peripheral.Connected += OnPeripheralConnected;
                peripheral.Disconnected += OnPeripheralDisconnected;

                discoveredPeripherals.Add (peripheral);
            }

            EventHandler<BLEDiscoveredPeripheralEventArgs> handler = DiscoveredPeripheral;
            if (handler != null) {
                BLEDiscoveredPeripheralEventArgs args = new BLEDiscoveredPeripheralEventArgs ();
                args.AdvertisementData = new List<KeyValuePair<string, string>> ();
                args.Peripheral = peripheral;
                args.RSSI = result.Rssi;

                handler (this, args);
            }
        }

        public override void OnScanFailed (ScanFailure errorCode)
        {
            base.OnScanFailed (errorCode);
        }

        void OnPeripheralConnected (object sender, EventArgs e)
        {
            EventHandler<BLEPeripheralEventArgs> handler = ConnectedToPeripheral;
            if (handler != null) {
                BLEPeripheralEventArgs args = new BLEPeripheralEventArgs ();
                args.Peripheral = sender as BLEPeripheral;

                handler (this, args);
            }
        }

        void OnPeripheralDisconnected (object sender, EventArgs e)
        {
            EventHandler<BLEPeripheralErrorEventArgs> handler = DisconnectedFromPeripheral;
            if (handler != null) {
                BLEPeripheralErrorEventArgs args = new BLEPeripheralErrorEventArgs ();
                args.Peripheral = sender as BLEPeripheral;

                handler (this, args);
            }
        }
    }
}
#pragma warning restore XA0001 // Find issues with Android API usage