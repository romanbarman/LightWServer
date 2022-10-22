namespace LightWServer.Core.HttpContext
{
    internal sealed class HeaderCollection : IHeaderCollection
    {
        private readonly Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public void Add(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            headers[key] = value;
        }

        public bool ContainsKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return headers.ContainsKey(key);
        }

        public IEnumerable<string> GetKeys()
        {
            return headers.Keys;
        }

        public string GetValue(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

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
    }
}
