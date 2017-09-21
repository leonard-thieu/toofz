using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using toofz.Tests.Properties;
using toofz.TestsShared;

namespace toofz.Tests
{
    class CategoriesTests
    {
        [TestClass]
        public class Serialization
        {
            [TestMethod]
            public void Serializes()
            {
                // Arrange
                var categories = new Categories
                {
                    {
                        "myCategory",
                        new Category
                        {
                            {
                                "myItem",
                                new CategoryItem
                                {
                                    Id = 12,
                                    DisplayName = "My Display Name",
                                }
                            }
                        }
                    },
                };

                // Act
                var json = JsonConvert.SerializeObject(categories, Formatting.Indented);

                // Assert
                Assert.That.NormalizedAreEqual(Resources.Categories, json);
            }

            [TestMethod]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.Categories;

                // Act
                var categories = JsonConvert.DeserializeObject<Categories>(json);

                // Assert
                Assert.IsInstanceOfType(categories, typeof(Categories));
            }
        }
    }
}
