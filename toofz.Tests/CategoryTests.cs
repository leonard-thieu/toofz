using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using toofz.Tests.Properties;
using toofz.TestsShared;

namespace toofz.Tests
{
    class CategoryTests
    {
        [TestClass]
        public class GetNameMethod
        {
            [TestMethod]
            public void ItemWithIdIsNotInCategory_ThrowsArgumentException()
            {
                // Arrange
                var category = new Category();
                var id = 10;

                // Act -> Assert
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    category.GetName(id);
                });
            }

            [TestMethod]
            public void ReturnsName()
            {
                // Arrange
                var category = new Category
                {
                    { "myItem", new CategoryItem { Id = 10 } },
                };
                var id = 10;

                // Act
                var result = category.GetName(id);

                // Assert
                Assert.AreEqual("myItem", result);
            }
        }

        [TestClass]
        public class GetNamesAsCommaSeparatedValuesMethod
        {
            [TestMethod]
            public void ReturnsNamesAsCommaSeparatedValues()
            {
                // Arrange
                var category = new Category
                {
                    { "myItem", new CategoryItem { Id = 10 } },
                    { "myOtherItem", new CategoryItem { Id = 11 } },
                };

                // Act
                var result = category.GetNamesAsCommaSeparatedValues();

                // Assert
                Assert.AreEqual("myItem,myOtherItem", result);
            }
        }

        [TestClass]
        public class Serialization
        {
            [TestMethod]
            public void Serializes()
            {
                // Arrange
                var category = new Category
                {
                    {
                        "myItem",
                        new CategoryItem
                        {
                            Id = 10,
                            DisplayName = "myDisplayName",
                        }
                    },
                };

                // Act
                var json = JsonConvert.SerializeObject(category, Formatting.Indented);

                // Assert
                Assert.That.NormalizedAreEqual(Resources.Category, json);
            }

            [TestMethod]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.Category;

                // Act
                var category = JsonConvert.DeserializeObject<Category>(json);

                // Assert
                Assert.IsInstanceOfType(category, typeof(Category));
            }
        }
    }
}
