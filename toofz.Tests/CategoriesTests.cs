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
                var json = JsonConvert.SerializeObject(categories);

                // Assert
                AssertHelper.NormalizedAreEqual(Resources.Categories, json);
            }

            [TestMethod]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.Categories;

                // Act
                var categories = JsonConvert.DeserializeObject<Categories>(json);

                // Assert
                var categoryExists = categories.TryGetValue("myCategory", out var category);
                Assert.IsTrue(categoryExists);
                var itemExists = category.TryGetValue("myItem", out var item);
                Assert.IsTrue(itemExists);
                Assert.AreEqual(12, item.Id);
                Assert.AreEqual("My Display Name", item.DisplayName);
            }
        }
    }
}
