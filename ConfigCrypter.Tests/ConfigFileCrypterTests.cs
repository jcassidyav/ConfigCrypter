using DevAttic.ConfigCrypter.ConfigCrypters.Json;
using DevAttic.ConfigCrypter.Crypters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace DevAttic.ConfigCrypter.Tests
{
    public class ConfigFileCrypterTests
    {

        [Fact]
        public void EncryptKeys_MissingKey()
        {
            var certificateLoaderMock = Mocks.CertificateLoader;

            var configCrypter = new JsonConfigCrypter(new RSACrypter(certificateLoaderMock.Object));

            var fileCrypter = new ConfigFileCrypter(configCrypter, new ConfigFileCrypterOptions()
            {
                ReplaceCurrentConfig = true
            });

            var json = JsonConvert.SerializeObject(new { Key = "ValueToEncrypt" });

            using (var f = new TempFile(json))
            {
                fileCrypter.EncryptKeysInFile(f.FilePath, new List<string>(new string[] { "NotTHere", "Key" }));
                

                Assert.True(IsKeyNotSomething(f.FilePath));

                fileCrypter.DecryptKeysInFile(f.FilePath, new List<string>(new string[] { "NotTHere", "Key" }));
                Assert.False(IsKeyNotSomething(f.FilePath));
            }

        }

        static bool IsKeyNotSomething(string jsonFilePath)
        {
            var json = File.ReadAllText(jsonFilePath);

            using var document = JsonDocument.Parse(json);

            if (!document.RootElement.TryGetProperty("Key", out var keyElement))
                return true; // Key doesn't exist, so it's definitely not "something"

            return keyElement.GetString() != "ValueToEncrypt";
        }
    }
}
