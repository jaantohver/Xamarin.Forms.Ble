#pragma warning disable XA0001 // Find issues with Android API usage
using Android.Bluetooth;

namespace XamarinBle.Droid
{
    public class BLECharacteristic : IBLECharacteristic
    {
        readonly BluetoothGattCharacteristic nativeCharacteristic;
        public object NativeCharacteristic {
            get {
                return nativeCharacteristic;
            }
        }

        readonly UUID uuid;
        public UUID UUID {
            get {
                return uuid;
            }
        }

        byte [] value;
        public byte [] Value {
            get {
                return value;
            }
        }

        public BLECharacteristic (BluetoothGattCharacteristic nativeCharacteristic)
        {
            this.nativeCharacteristic = nativeCharacteristic;

            uuid = new UUID (nativeCharacteristic.Uuid.ToString ());
        }

        public bool Equals (IBLECharacteristic other)
        {
            return uuid.Equals (other.UUID);
        }
    }
}
#pragma warning restore XA0001 // Find issues with Android API usage