namespace LightWServer.Core.Test
{
    internal class TempFileCreator : IDisposable
    {
        private readonly string folderName;
        private readonly string fullPath;

        public string FullPath => fullPath;

        internal TempFileCreator(string folderName, string fileName, string content)
        {
            this.folderName = folderName;
            fullPath = Path.Combine(folderName, fileName);

            Create(content);
        }

        public void Dispose()
        {
            if (Directory.Exists(folderName))
                Directory.Delete(folderName, true);
        }

        private void Create(string content)
        {
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            File.AppendAllText(fullPath, content);
        }
    }
}
