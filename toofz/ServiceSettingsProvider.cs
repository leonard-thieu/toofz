using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace toofz
{
    public sealed class ServiceSettingsProvider : SettingsProvider
    {
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

        Func<Stream> getSettingsStream = () => File.Create("user.config");
        public Func<Stream> GetSettingsStream
        {
            get => getSettingsStream;
            set => getSettingsStream = value ?? throw new ArgumentNullException(nameof(value));
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
            using (var settingStream = GetSettingsStream())
            {
                doc = XDocument.Load(settingStream);
            }
            var settings = doc.Element("settings");

            foreach (SettingsProperty property in properties)
            {
                var value = new SettingsPropertyValue(property);
                value.SerializedValue = settings
                    .Elements("setting")
                    .FirstOrDefault(s => s.Attribute("name").Value == property.Name)
                    .Value;
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
            var settings = new XElement("settings");
            doc.Add(settings);

            foreach (SettingsPropertyValue value in values)
            {
                var setting = new XElement("setting");
                setting.SetAttributeValue("name", value.Name);
                setting.Add(new XElement("value", value.SerializedValue));
                settings.Add(setting);
            }

            using (var settingStream = GetSettingsStream())
            {
                doc.Save(settingStream);
            }
        }
    }
}
