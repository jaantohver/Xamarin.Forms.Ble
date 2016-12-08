using System;
using System.Collections.Generic;

using Foundation;
using CoreBluetooth;

namespace XamarinBle.iOS
{
    public class BLEPeripheral : IBLEPeripheral
    {
        public event EventHandler<BLEErrorEventArgs> DiscoveredService;
        public event EventHandler<BLEServiceEventArgs> DiscoveredCharacteristic;
        public event EventHandler<BLECharacteristicEventArgs> UpdatedCharacteristicValue;

        readonly CBPeripheral nativePeripheral;
        public object NativePeripheral {
            get {
                return nativePeripheral;
            }
        }

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

        readonly List<IBLEService> services;
        public List<IBLEService> Services {
            get {
                return services;
            }
        }

        public BLEPeripheral (CBPeripheral nativePeripheral)
        {
            services = new List<IBLEService> ();

            this.nativePeripheral = nativePeripheral;
            this.nativePeripheral.DiscoveredService += OnDiscoveredService;
            this.nativePeripheral.DiscoveredCharacteristic += OnDiscoveredCharacteristic;
            this.nativePeripheral.UpdatedCharacterteristicValue += OnUpdatedCharacteristicValue;

            uuid = new UUID (nativePeripheral.UUID.ToString (true));
        }

        public void DiscoverCharacteristics (IBLEService service)
        {
            nativePeripheral.DiscoverCharacteristics (service.NativeService as CBService);
        }

        public void DiscoverServices ()
        {
            nativePeripheral.DiscoverServices ();
        }

        void OnDiscoveredService (object sender, NSErrorEventArgs e)
        {
            foreach (CBService cbs in nativePeripheral.Services) {
                BLEService bles = new BLEService (cbs);

                services.Add (bles);
            }

            EventHandler<BLEErrorEventArgs> handler = DiscoveredService;
            if (handler != null) {
                BLEErrorEventArgs args = new BLEErrorEventArgs ();
                args.Error = e.Error?.ToString ();

                handler (this, args);
            }
        }

        void OnDiscoveredCharacteristic (object sender, CBServiceEventArgs e)
        {
            BLEService service = null;

            foreach (BLEService bles in services) {
                if (bles.UUID.StringValue == e.Service.UUID.ToString (true)) {
                    service = bles;

                    break;
                }
            }

            foreach (CBCharacteristic cbc in e.Service.Characteristics) {
                BLECharacteristic blec = new BLECharacteristic (cbc);

                service.Characteristics.Add (blec);
            }

            EventHandler<BLEServiceEventArgs> handler = DiscoveredCharacteristic;
            if (handler != null) {
                BLEServiceEventArgs args = new BLEServiceEventArgs ();
                args.Service = service;
                args.Error = e.Error?.ToString ();

                handler (this, args);
            }
        }

        void OnUpdatedCharacteristicValue (object sender, CBCharacteristicEventArgs e)
        {
            BLECharacteristic characteristic = null;

            foreach (BLEService bles in services) {
                foreach (BLECharacteristic blec in bles.Characteristics) {
                    if (blec.UUID.StringValue.Equals (e.Characteristic.UUID.ToString ())) {
                        characteristic = blec;

                        break;
                    }
                }
            }

            characteristic.Value = e.Characteristic.Value.ToArray ();

            EventHandler<BLECharacteristicEventArgs> handler = UpdatedCharacteristicValue;
            if (handler != null) {
                BLECharacteristicEventArgs args = new BLECharacteristicEventArgs ();
                args.Characteristic = characteristic;
                args.Error = e.Error?.ToString ();

                handler (this, args);
            }
        }

        public bool Equals (IBLEPeripheral other)
        {
            return uuid.Equals (other.UUID);
        }
    }
}