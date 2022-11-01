using System.Collections;

namespace LightWServer.Core.HttpContext
{
    internal interface IHeaderCollection : IEnumerable<KeyValuePair<string, string>>
    {
        void Add(string key, string value);
        bool ContainsKey(string key);
        string GetValue(string key);
        IEnumerable<string> GetKeys();
    }
}
