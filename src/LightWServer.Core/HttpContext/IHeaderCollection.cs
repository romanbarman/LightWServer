namespace LightWServer.Core.HttpContext
{
    internal interface IHeaderCollection : IEnumerable<Header>
    {
        void Add(Header header);
        bool Contains(string name);
        Header Get(string name);
        IEnumerable<string> GetHeadersNames();
    }
}
