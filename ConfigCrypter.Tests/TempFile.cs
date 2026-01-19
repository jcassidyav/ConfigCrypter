using System;
using System.IO;
namespace DevAttic.ConfigCrypter.Tests
{
    public class TempFile : IDisposable
    {
        public string FilePath { get; }

        public TempFile(string content)
        {
            FilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            File.WriteAllText(FilePath, content);
        }

        public void Dispose()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete temp file: {ex.Message}");
            }
        }
    }
}