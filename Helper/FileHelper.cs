namespace api_csharp.Helper
{
    public class FileHelper
    {
        public static MemoryStream GetFile(string path)
        {
            var bytes = File.ReadAllBytes(path);
            return new MemoryStream(bytes);
        }
    }
}
