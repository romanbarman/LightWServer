using LightWServer.Core.Exceptions;

namespace LightWServer.Core.HttpContext
{
    internal sealed class Header : IEquatable<Header>
    {
        private const string HeaderSeparator = ": ";

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

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }

        internal static Header Parse(string header)
        {
            var index = header.IndexOf(HeaderSeparator);

            if (index == -1)
                throw new InvalidHeaderFormatException(header);

            var name = header.Substring(0, index).Trim();
            var value = header.Substring(index + HeaderSeparator.Length).Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(value))
                throw new InvalidHeaderFormatException(header);

            return new Header(name, value);
        }
    }
}
