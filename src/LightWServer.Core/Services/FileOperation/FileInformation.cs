namespace LightWServer.Core.Services.FileOperation
{
    internal sealed class FileInformation
    {
        internal long Length { get; }
        internal string Extension { get; }

        internal FileInformation(long length, string extension)
        {
            Length = length;
            Extension = extension;
        }
    }
}
