using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace toofz
{
    public sealed class EncryptedSecret : IXmlSerializable
    {
        // Required for XML serialization
        EncryptedSecret() { }

        /// <summary>
        /// Initializes an instance of the <see cref="EncryptedSecret"/> class.
        /// </summary>
        /// <param name="secret">The secret to encrypt.</param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="secret"/> cannot be null or empty.
        /// </exception>
        public EncryptedSecret(string secret)
        {
            (this.secret, salt) = Secrets.Encrypt(secret);
        }

        byte[] secret;
        byte[] salt;

        /// <summary>
        /// Decrypts the encrypted secret.
        /// </summary>
        /// <returns>
        /// The decrypted secret.
        /// </returns>
        public string Decrypt() => Secrets.Decrypt(secret, salt);

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
            reader.ReadStartElement();
            secret = ReadElementContentAsBase64();
            salt = ReadElementContentAsBase64();
            reader.ReadEndElement();

            byte[] ReadElementContentAsBase64()
            {
                var result = new List<byte>();
                var buffer = new byte[1024];
                var readBytes = 0;

                while ((readBytes = reader.ReadElementContentAsBase64(buffer, 0, buffer.Length)) > 0)
                {
                    result.AddRange(buffer.Take(readBytes));
                }

                return result.ToArray();
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="XmlWriter"/> stream to which the object is serialized.
        /// </param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Secret");
            writer.WriteBase64(secret, 0, secret.Length);
            writer.WriteEndElement();

            writer.WriteStartElement("Salt");
            writer.WriteBase64(salt, 0, salt.Length);
            writer.WriteEndElement();
        }

        #endregion
    }
}
