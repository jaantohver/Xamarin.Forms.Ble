using CoreBluetooth;

namespace XamarinBle.iOS
{
    public class BLECharacteristic : IBLECharacteristic
    {
        readonly CBCharacteristic nativeCharacteristic;
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
            internal set {
                this.value = value;
            }
        }

        public BLECharacteristic (CBCharacteristic nativeCharacteristic)
        {
            this.nativeCharacteristic = nativeCharacteristic;

            uuid = new UUID (nativeCharacteristic.UUID.ToString (true));
        }

        public bool Equals (IBLECharacteristic other)
        {
            return uuid.Equals (other.UUID);
        }
    }
}