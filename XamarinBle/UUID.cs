using System;

namespace XamarinBle
{
    public class UUID : IEquatable<UUID>
    {
        internal string StringValue;

        public UUID (string stringValue)
        {
            StringValue = stringValue;
        }

        public bool Equals (UUID other)
        {
            return StringValue.Equals (other.StringValue);
        }

        public override string ToString ()
        {
            return StringValue;
        }
    }
}