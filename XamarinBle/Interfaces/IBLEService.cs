using System;
using System.Collections.Generic;

namespace XamarinBle
{
    public interface IBLEService : IEquatable<IBLEService>
    {
        UUID UUID { get; }

        object NativeService { get; }

        List<IBLECharacteristic> Characteristics { get; }
    }
}