namespace LightWServer.Core.HttpContext
{
    internal interface IHeaderCollection
    {
        void Add(string key, string value);
        bool ContainsKey(string key);
        string GetValue(string key);
        IEnumerable<string> GetKeys();
    }
}
