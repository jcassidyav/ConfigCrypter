using DevAttic.ConfigCrypter.ConfigCrypters.Json;
using Newtonsoft.Json;
using Xunit;

namespace DevAttic.ConfigCrypter.Tests.ConfigCrypters.Json
{
    public class JsonConfigCrypterTests
    {

        [Fact]
        public void RetrieveFields()
        {
            var crypterMock = Mocks.Crypter;
            var jsonCrypter = new JsonConfigCrypter(crypterMock.Object);
            var jsonObject = (Name: "Widget", FieldsToDecrypt: new string[] { "General:SXEncryptKey",
                                                                                "Logging:EncryptionKey",
                                                                                "EncryptionUtils:EncryptionKeys:TransferKey",
                                                                                "TheThing:SID",
                                                                                "TheThing:Token",
                                                                                "DocumentWriter:Username",
                                                                                "DocumentWriter:Password",
                                                                                "DocumentWriter:Domain",
                                                                                "AuthServer:Username",
                                                                                "AuthServer:Password",
                                                                                "AuthServer:Domain",
                                                                                "AuthServer:AgntDummyUsername",
                                                                                "AuthServer:AgntDummyPassword",
                                                                                "AuthServer:AgntDummyState",
                                                                                "AuthServer:AgntDummyCode",
                                                                                "AuthServer:InternalDummyUsername",
                                                                                "AuthServer:InternalDummyPassword",
                                                                                "AuthServer:InternalDummyDuoToken",
                                                                                "SecureModel:TokenKey",
                                                                                "TheThing:Username",
                                                                                "TheThing:Password",
                                                                                "TheThing:TheThing",
                                                                                "ConnectionStrings:TheThing",
                                                                                "ConnectionStrings:TheThing",
                                                                                "DataProtectionConfiguration:ConnectionString",
                                                                                "Translation:AsymKey",
                                                                                "Translation:GoogleProjectNo",
                                                                                "TheThing:TheThing",
                                                                                "TrackLoc:TheThing",
                                                                                "Csrf:HashKey"});
            var json = JsonConvert.SerializeObject(jsonObject);

            var list = jsonCrypter.GetKeyValueList(json, "Item2");
            Assert.Equal(30, list.Count);
            Assert.Equal("Csrf.HashKey", list[29]);
            Assert.Equal("General.SXEncryptKey", list[0]);

        }



        [Fact]
        public void EncryptKey_WithValidJson_CallsEncryptStringOnCrypter()
        {
            var crypterMock = Mocks.Crypter;
            var jsonCrypter = new JsonConfigCrypter(crypterMock.Object);
            var json = JsonConvert.SerializeObject(new TestAppSettings() { Key = "ValueToEncrypt" });

            var encryptedJson = jsonCrypter.EncryptKey(json, "Key");
            var parsedJson = JsonConvert.DeserializeObject<TestAppSettings>(encryptedJson);

            crypterMock.Verify(crypter => crypter.EncryptString("ValueToEncrypt"));

            // Additionally we test if the test crypter does its job.
            Assert.Equal("ValueToEncrypt_encrypted", parsedJson.Key);
        }

        [Fact]
        public void DecryptKey_WithValidJson_CallsDecryptStringOnCrypter()
        {
            var crypterMock = Mocks.Crypter;
            var jsonCrypter = new JsonConfigCrypter(crypterMock.Object);
            var json = JsonConvert.SerializeObject(new { Key = "ValueToEncrypt_encrypted" });

            var decryptedJson = jsonCrypter.DecryptKey(json, "Key");
            var parsedJson = JsonConvert.DeserializeObject<TestAppSettings>(decryptedJson);

            crypterMock.Verify(crypter => crypter.DecryptString("ValueToEncrypt_encrypted"));
            Assert.Equal("ValueToEncrypt", parsedJson.Key);
        }

        [Fact]
        public void Dispose_CallsDisposeOnCrypter()
        {
            var crypterMock = Mocks.Crypter;
            var jsonCrypter = new JsonConfigCrypter(crypterMock.Object);

            jsonCrypter.Dispose();

            crypterMock.Verify(crypter => crypter.Dispose());
        }
    }
}
