#pragma warning disable XA0001 // Find issues with Android API usage
using System;
using System.Collections.Generic;

using Android.App;
using Android.Bluetooth;

namespace XamarinBle.Droid
{
    public class BLEPeripheral : BluetoothGattCallback, IBLEPeripheral
    {
        BluetoothGatt gatt;

        public string Name {
            get {
                return nativePeripheral.Name;
            }
        }

        readonly UUID uuid;
        public UUID UUID {
            get {
                return uuid;
            }
        }

        readonly BluetoothDevice nativePeripheral;
        public object NativePeripheral {
            get {
                return nativePeripheral;
            }
        }

        readonly List<IBLEService> services;
        public List<IBLEService> Services {
            get {
                return services;
            }
        }

        internal event EventHandler Connected;
        internal event EventHandler Disconnected;
        public event EventHandler<BLEErrorEventArgs> DiscoveredService;
        public event EventHandler<BLEServiceEventArgs> DiscoveredCharacteristic;
        public event EventHandler<BLECharacteristicEventArgs> UpdatedCharacteristicValue;

        public BLEPeripheral (BluetoothDevice nativePeripheral)
        {
            services = new List<IBLEService> ();

            this.nativePeripheral = nativePeripheral;

            uuid = new UUID (nativePeripheral.Address);
        }

        public void DiscoverCharacteristics (IBLEService service)
        {
            EventHandler<BLEServiceEventArgs> handler = DiscoveredCharacteristic;

            if (handler != null) {
                BLEServiceEventArgs args = new BLEServiceEventArgs ();
                args.Service = service;

                handler (this, args);
            }
        }

        public void DiscoverServices ()
        {
            gatt.DiscoverServices ();
        }

        internal void Connect ()
        {
            gatt = nativePeripheral.ConnectGatt (Application.Context, false, this);
        }

        internal void Disconnect ()
        {
            gatt.Disconnect ();
            gatt.Close ();
            gatt.Dispose ();
        }

        public override void OnCharacteristicChanged (BluetoothGatt gatt, BluetoothGattCharacteristic characteristic)
        {
            base.OnCharacteristicChanged (gatt, characteristic);

            BLECharacteristic bleCharacteristic = null;

            foreach (BLEService bles in services) {
                foreach (BLECharacteristic blec in bles.Characteristics) {
                    if (blec.UUID.StringValue == bleCharacteristic.UUID.ToString ()) {
                        bleCharacteristic = blec;

                        break;
                    }
                }
            }

            EventHandler<BLECharacteristicEventArgs> handler = UpdatedCharacteristicValue;
            if (handler != null) {
                BLECharacteristicEventArgs args = new BLECharacteristicEventArgs ();
                args.Characteristic = bleCharacteristic;

                handler (this, args);
            }
        }

        public override void OnCharacteristicRead (BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, GattStatus status)
        {
            base.OnCharacteristicRead (gatt, characteristic, status);

            BLECharacteristic bleCharacteristic = null;

            foreach (BLEService bles in services) {
                foreach (BLECharacteristic blec in bles.Characteristics) {
                    if (blec.UUID.StringValue == bleCharacteristic.UUID.ToString ()) {
                        bleCharacteristic = blec;

                        break;
                    }
                }
            }

            EventHandler<BLECharacteristicEventArgs> handler = UpdatedCharacteristicValue;
            if (handler != null) {
                BLECharacteristicEventArgs args = new BLECharacteristicEventArgs ();
                args.Characteristic = bleCharacteristic;

                if (status != GattStatus.Success) {
                    args.Error = status.ToString ();
                }

                handler (this, args);
            }
        }

        public override void OnCharacteristicWrite (BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, GattStatus status)
        {
            base.OnCharacteristicWrite (gatt, characteristic, status);
        }

        public override void OnConnectionStateChange (BluetoothGatt gatt, GattStatus status, ProfileState newState)
        {
            base.OnConnectionStateChange (gatt, status, newState);

            if (newState == ProfileState.Connected) {
                EventHandler handler = Connected;
                if (handler != null) {
                    handler (this, EventArgs.Empty);
                }
            } else if (newState == ProfileState.Disconnected) {
                EventHandler handler = Disconnected;
                if (handler != null) {
                    handler (this, EventArgs.Empty);
                }
            }
        }

        public override void OnServicesDiscovered (BluetoothGatt gatt, GattStatus status)
        {
            base.OnServicesDiscovered (gatt, status);

            foreach (BluetoothGattService bgs in gatt.Services) {
                BLEService service = new BLEService (bgs);

                services.Add (service);
            }

            EventHandler<BLEErrorEventArgs> handler = DiscoveredService;
            if (handler != null) {
                BLEErrorEventArgs args = new BLEErrorEventArgs ();

                if (status != GattStatus.Success) {
                    args.Error = status.ToString ();
                }

                handler (this, args);
            }
        }

        public bool Equals (IBLEPeripheral other)
        {
            return uuid.Equals (other.UUID);
        }
    }
}
#pragma warning restore XA0001 // Find issues with Android API usage