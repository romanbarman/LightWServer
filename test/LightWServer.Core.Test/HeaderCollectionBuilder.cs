using LightWServer.Core.HttpContext;

namespace LightWServer.Core.Test
{
    internal static class HeaderCollectionBuilder
    {
        internal static HeaderCollection Create(params Header[] headers)
        {
            var headerCollection = HeaderCollection.CreateForRequest();

            foreach (var header in headers)
            {
                headerCollection.Add(header.Name, header.Value);
            }

            return headerCollection;
        }
    }
}
