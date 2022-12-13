using System.Collections;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LightWServer.Core.Test")]

namespace LightWServer.Core.HttpContext
{
    public sealed class HeaderCollection : IHeaderCollection
    {
        private readonly Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public void Add(string key, string value)
        {
            if (key.Trim().Equals(string.Empty))
                throw new ArgumentException("Key is empty", nameof(key));
            if (value.Trim().Equals(string.Empty))
                throw new ArgumentException("Value is empty", nameof(value));

            headers[key] = value;
        }

        public bool ContainsKey(string key)
        {
            if (key.Trim().Equals(string.Empty))
                throw new ArgumentException("Key is empty", nameof(key));

            return headers.ContainsKey(key);
        }

        public IEnumerable<string> GetKeys()
        {
            return headers.Keys;
        }

        public string GetValue(string key)
        {
            if (key.Trim().Equals(string.Empty))
                throw new ArgumentException("Key is empty", nameof(key));

            if (!headers.ContainsKey(key))
                throw new ArgumentOutOfRangeException(nameof(key), $"The header '{key}' does not exist");

            return headers[key];
        }

        internal static IHeaderCollection CreateForResponse()
        {
            var headerCollection =  new HeaderCollection();
            headerCollection.Add("Server", "LightWServer/0.0.01");

            return headerCollection;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return headers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
