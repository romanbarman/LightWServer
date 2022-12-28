namespace LightWServer.Core.HttpContext
{
    internal interface IHeaderCollection : IEnumerable<Header>
    {
        void Add(string name, string value);
        bool Contains(string name);
        string GetValue(string name);
        IEnumerable<string> GetHeadersNames();
    }
}
