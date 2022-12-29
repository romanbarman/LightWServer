namespace LightWServer.Core.Services.FileOperation
{
    internal sealed class FileInformation
    {
        public long Length { get; }
        public string Extension { get; }

        public FileInformation(long length, string extension)
        {
            Length = length;
            Extension = extension;
        }
    }
}
