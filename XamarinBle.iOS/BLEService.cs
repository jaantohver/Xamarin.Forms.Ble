using System.Collections.Generic;

using CoreBluetooth;

namespace XamarinBle.iOS
{
    public class BLEService : IBLEService
    {
        readonly CBService nativeService;
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

        readonly List<IBLECharacteristic> characteristics;
        public List<IBLECharacteristic> Characteristics {
            get {
                return characteristics;
            }
        }

        public BLEService (CBService nativeService)
        {
            characteristics = new List<IBLECharacteristic> ();

            this.nativeService = nativeService;

            uuid = new UUID (nativeService.UUID.ToString (true));
        }

        public bool Equals (IBLEService other)
        {
            return uuid.Equals (other.UUID);
        }
    }
}