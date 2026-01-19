using System;
using System.Collections.Generic;

namespace DevAttic.ConfigCrypter.ConfigCrypters
{
    /// <summary>
    /// Encrypts/Decrypts keys in configuration files.
    /// </summary>
    public interface IConfigCrypter : IDisposable
    {
        /// <summary>
        /// Decrypts the key in the given content of a config file.
        /// </summary>
        /// <param name="configFileContent">String content of a config file.</param>
        /// <param name="configKey">Key of the config entry.</param>
        /// <returns>The content of the config file where the key has been decrypted.</returns>
        /// <remarks>It up to the implementer how to interpret the format of the config key.</remarks>
        string DecryptKey(string configFileContent, string configKey);

        /// <summary>
        /// Encrypts the key in the given content of a config file.
        /// </summary>
        /// <param name="configFileContent">String content of a config file.</param>
        /// <param name="configKey">Key of the config entry.</param>
        /// <returns>The content of the config file where the key has been encrypted.</returns>
        /// <remarks>It up to the implementer how to interpret the format of the config key.</remarks>
        string EncryptKey(string configFileContent, string configKey);

        /// <summary>
        /// Returns the key value from an appsettings.json file
        /// </summary>
        /// <param name="json"></param>
        /// <param name="configKey"></param>
        /// <returns></returns>
        public List<string> GetKeyValueList(string json, string configKey);
    }

    public class KeyNotFoundException : Exception { 
        public KeyNotFoundException(string? message) : base(message) { }
    }
}