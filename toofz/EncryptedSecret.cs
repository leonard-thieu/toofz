using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace toofz
{
    [XmlRoot(EncryptedSecretName)]
    public sealed class EncryptedSecret : IXmlSerializable
    {
        const string EncryptedSecretName = "encryptedSecret";
        const string SecretName = "secret";
        const string SaltName = "salt";
        const string IterationsName = "iterations";

        // Required for XML serialization
        EncryptedSecret() { }

        /// <summary>
        /// Initializes an instance of the <see cref="EncryptedSecret"/> class.
        /// </summary>
        /// <param name="secret">The secret to encrypt.</param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="secret"/> cannot be null or empty.
        /// </exception>
        public EncryptedSecret(string secret, int iterations)
        {
            (this.secret, salt) = Secrets.Encrypt(secret, iterations);
            this.iterations = iterations;
        }

        byte[] secret;
        byte[] salt;
        int iterations;

        /// <summary>
        /// Decrypts the encrypted secret.
        /// </summary>
        /// <returns>
        /// The decrypted secret.
        /// </returns>
        public string Decrypt() => Secrets.Decrypt(secret, salt, iterations);

        #region IXmlSerializable Members

        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">
        /// The <see cref="XmlReader"/> stream from which the object is deserialized.
        /// </param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(EncryptedSecretName);
            secret = Convert.FromBase64String(reader.ReadElementContentAsString(SecretName, ""));
            salt = Convert.FromBase64String(reader.ReadElementContentAsString(SaltName, ""));
            iterations = reader.ReadElementContentAsInt(IterationsName, "");
            reader.ReadEndElement();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="XmlWriter"/> stream to which the object is serialized.
        /// </param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(SecretName);
            writer.WriteBase64(secret, 0, secret.Length);
            writer.WriteEndElement();

            writer.WriteStartElement(SaltName);
            writer.WriteBase64(salt, 0, salt.Length);
            writer.WriteEndElement();

            writer.WriteStartElement(IterationsName);
            writer.WriteValue(iterations);
            writer.WriteEndElement();
        }

        #endregion
    }
}
