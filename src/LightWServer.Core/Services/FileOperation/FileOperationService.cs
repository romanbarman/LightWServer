namespace LightWServer.Core.Services.FileOperation
{
    internal class FileOperationService : IFileOperationService
    {
        public bool Exists(string path) => File.Exists(path);

        public FileInformation GetFileInfo(string path)
        {
            var fileInfo = new FileInfo(path);

            return new FileInformation(fileInfo.Length, fileInfo.Extension);
        }
    }
}
