using LightWServer.Core.Exceptions;
using System.Collections;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LightWServer.Core.Test")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace LightWServer.Core.HttpContext
{
    internal sealed class HeaderCollection : IHeaderCollection
    {
        private readonly Dictionary<string, Header> headers = new Dictionary<string, Header>(StringComparer.OrdinalIgnoreCase);

        private HeaderCollection() { }

        public void Add(Header header)
        {
            headers[header.Name] = header;
        }

        public bool Contains(string name) => headers.ContainsKey(name);

        public IEnumerable<string> GetHeadersNames() => headers.Keys;

        public string GetValue(string name)
        {
            if (!headers.ContainsKey(name))
                throw new HeaderNotExistException(name);

            return headers[name].Value;
        }

        public IEnumerator<Header> GetEnumerator() => headers.Select(x => x.Value).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        internal static HeaderCollection CreateForRequest() => new HeaderCollection();

        internal static HeaderCollection CreateForResponse()
        {
            var headerCollection = new HeaderCollection();
            headerCollection.Add(new Header("Server", "LightWServer/0.0.01"));

            return headerCollection;
        }
    }
}
