namespace LightWServer.Core.Services.FileOperation
{
    internal interface IFileOperationService
    {
        bool Exists(string path);
        FileInformation GetFileInfo(string path);
    }
}
