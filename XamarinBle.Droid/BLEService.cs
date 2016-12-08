#pragma warning disable XA0001 // Find issues with Android API usage
using System.Collections.Generic;

using Android.Bluetooth;

namespace XamarinBle.Droid
{
    public class BLEService : IBLEService
    {
        readonly List<IBLECharacteristic> characteristics;
        public List<IBLECharacteristic> Characteristics {
            get {
                return characteristics;
            }
        }

        readonly BluetoothGattService nativeService;
        public object NativeService {
            get {
                return nativeService;
            }
        }

        readonly UUID uuid;
        public UUID UUID {
            get {
                return uuid;
            }
        }

        public BLEService (BluetoothGattService nativeService)
        {
            characteristics = new List<IBLECharacteristic> ();

            foreach (BluetoothGattCharacteristic bgc in nativeService.Characteristics) {
                BLECharacteristic characteristic = new BLECharacteristic (bgc);

                characteristics.Add (characteristic);
            }

            this.nativeService = nativeService;

            uuid = new UUID (nativeService.Uuid.ToString ());
        }

        public bool Equals (IBLEService other)
        {
            return uuid.Equals (other.UUID);
        }
    }
}
#pragma warning restore XA0001 // Find issues with Android API usage