using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using toofz.Tests.Properties;

namespace toofz.Tests
{
    class CategoriesEnvelopeTests
    {
        [TestClass]
        public class Serialization
        {
            [TestMethod]
            public void WithoutCategories_DoesNotDeserialize()
            {
                // Arrange
                var json = Resources.CategoriesEnvelopeWithoutCategories;

                // Act -> Assert
                Assert.ThrowsException<JsonSerializationException>(() =>
                {
                    JsonConvert.DeserializeObject<CategoriesEnvelope>(json);
                });
            }

            [TestMethod]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.CategoriesEnvelope;

                // Act
                var categoriesEnvelope = JsonConvert.DeserializeObject<CategoriesEnvelope>(json);

                // Assert
                Assert.IsInstanceOfType(categoriesEnvelope, typeof(CategoriesEnvelope));
                Assert.IsInstanceOfType(categoriesEnvelope.Categories, typeof(Categories));
            }
        }
    }
}
