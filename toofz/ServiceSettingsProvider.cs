using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace toofz
{
    public sealed class ServiceSettingsProvider : SettingsProvider
    {
        const string ConfigFileName = "user.config";
        const string SettingsName = "settings";
        const string SettingName = "setting";
        const string NameName = "name";
        const string ValueName = "value";

        static XmlSerializerNamespaces emptyNamespaces;
        static XmlSerializerNamespaces EmptyNamespaces
        {
            get
            {
                if (emptyNamespaces == null)
                {
                    emptyNamespaces = new XmlSerializerNamespaces();
                    emptyNamespaces.Add("", "");
                }
                return emptyNamespaces;
            }
        }

        Func<TextReader> getSettingsReader = () => File.OpenText(ConfigFileName);
        /// <summary>
        /// Gets or sets a factory function that returns an instance of <see cref="TextReader"/> which is used to read settings.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// value cannot be null.
        /// </exception>
        public Func<TextReader> GetSettingsReader
        {
            get => getSettingsReader;
            set => getSettingsReader = value ?? throw new ArgumentNullException(nameof(value));
        }

        Func<TextWriter> getSettingsWriter = () => File.CreateText(ConfigFileName);
        /// <summary>
        /// Gets or sets a factory function that returns an instance of <see cref="TextWriter"/> which is used to write settings.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// value cannot be null.
        /// </exception>
        public Func<TextWriter> GetSettingsWriter
        {
            get => getSettingsWriter;
            set => getSettingsWriter = value ?? throw new ArgumentNullException(nameof(value));
        }

        string applicationName = Assembly.GetExecutingAssembly().GetName().Name;
        /// <summary>
        /// Gets or sets the name of the currently running application.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// value cannot be null.
        /// </exception>
        public override string ApplicationName
        {
            get => applicationName;
            set => applicationName = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="name">The friendly name of the provider.</param>
        /// <param name="config">
        /// A collection of the name/value pairs representing the provider-specific attributes 
        /// specified in the configuration for this provider.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// An attempt is made to call <see cref="Initialize(string, NameValueCollection)"/> on a provider after the provider has already been initialized.
        /// </exception>
        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(ApplicationName, config);
        }

        /// <summary>
        /// Returns the collection of settings property values for the specified application instance and settings property group.
        /// </summary>
        /// <param name="context">A <see cref="SettingsContext"/> describing the current application use.</param>
        /// <param name="properties">
        /// A <see cref="SettingsPropertyCollection"/> containing the settings property group whose values are to be retrieved.
        /// </param>
        /// <returns>
        /// A <see cref="SettingsPropertyValueCollection"/> containing the values for the specified settings property group.
        /// </returns>
        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection properties)
        {
            var values = new SettingsPropertyValueCollection();

            XDocument doc;
            using (var reader = GetSettingsReader())
            {
                try
                {
                    doc = XDocument.Load(reader);
                }
                catch (XmlException)
                {
                    doc = new XDocument(new XElement(SettingsName));
                }
            }
            var settings = doc.Element(SettingsName);

            foreach (SettingsProperty property in properties)
            {
                var value = new SettingsPropertyValue(property);

                var setting = settings
                    .Elements(SettingName)
                    .FirstOrDefault(s => s.Attribute(NameName).Value == property.Name);
                if (setting != null)
                {
                    if (property.SerializeAs == SettingsSerializeAs.String)
                    {
                        value.SerializedValue = setting.Value;
                    }
                    else if (property.SerializeAs == SettingsSerializeAs.Xml)
                    {
                        var valueEl = setting.Element(ValueName)?.Elements()?.FirstOrDefault();
                        if (valueEl != null)
                        {
                            // The XmlReader returned from XObject.CreateReader() cannot read base64 encoded values.
                            // To get around that, the value is serialized into memory and then deserialized using XmlSerializer.
                            using (var ms = new MemoryStream())
                            {
                                valueEl.Save(ms);
                                ms.Position = 0;
                                var serializer = new XmlSerializer(property.PropertyType);
                                value.PropertyValue = serializer.Deserialize(ms);
                            }
                        }
                    }
                    // SettingsSerializeAs.Binary and SettingsSerializeAs.ProviderSpecific are not supported.
                }

                // ServiceSettingsProvider doesn't make use of the IsDirty flag but it's reset in case other parts of the API makes use of it.
                value.IsDirty = false;
                values.Add(value);
            }

            return values;
        }

        /// <summary>
        /// Sets the values of the specified group of property settings.
        /// </summary>
        /// <param name="context">A <see cref="SettingsContext"/> describing the current application usage.</param>
        /// <param name="values">
        /// A <see cref="SettingsPropertyValueCollection"/> representing the group of property settings to set.
        /// </param>
        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection values)
        {
            var doc = new XDocument();
            var settings = new XElement(SettingsName);
            doc.Add(settings);

            foreach (SettingsPropertyValue value in values)
            {
                var property = value.Property;
                if (property.SerializeAs == SettingsSerializeAs.String)
                {
                    settings.Add(
                        new XElement(SettingName, new XAttribute(NameName, value.Name),
                            new XElement(ValueName, value.SerializedValue)));
                }
                else if (property.SerializeAs == SettingsSerializeAs.Xml)
                {
                    using (var sw = new StringWriter())
                    {
                        var serializer = new XmlSerializer(property.PropertyType);
                        serializer.Serialize(sw, value.PropertyValue, EmptyNamespaces);
                        var valueDoc = XDocument.Parse(sw.ToString());
                        settings.Add(
                            new XElement(SettingName, new XAttribute(NameName, value.Name),
                            new XElement(ValueName, valueDoc.Root)));
                    }
                }
                // SettingsSerializeAs.Binary and SettingsSerializeAs.ProviderSpecific are not supported.
            }

            using (var writer = GetSettingsWriter())
            {
                doc.Save(writer);
            }
        }
    }
}
