using System;

namespace XamarinBle
{
    public interface IBLECharacteristic : IEquatable<IBLECharacteristic>
    {
        UUID UUID { get; }

        object NativeCharacteristic { get; }

        byte [] Value { get; }
    }
}