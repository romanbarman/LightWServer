namespace LightWServer.Core.HttpContext
{
    internal sealed class Header : IEquatable<Header>
    {
        internal string Name { get; }
        internal string Value { get; }

        internal Header(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public bool Equals(Header? other)
        {
            if (ReferenceEquals(other, null))
                return false;

            return ReferenceEquals(this, other)
                || (Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase)
                    && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase));
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            if (ReferenceEquals(obj, this))
                return true;

            return obj.GetType() == GetType() && Equals((Header)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name.ToLower(), Value.ToLower());
        }
    }
}
