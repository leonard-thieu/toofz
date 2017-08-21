using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using toofz.Tests.Properties;
using toofz.TestsShared;

namespace toofz.Tests
{
    class CategoryItemTests
    {
        [TestClass]
        public class Id
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange
                var item = new CategoryItem();
                var id = 2;

                // Act
                item.Id = id;
                var result = item.Id;

                // Assert
                Assert.AreEqual(id, result);
            }
        }

        [TestClass]
        public class DisplayName
        {
            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange
                var item = new CategoryItem();
                var displayName = "myDisplayName";

                // Act
                item.DisplayName = displayName;
                var result = item.DisplayName;

                // Assert
                Assert.AreEqual(displayName, result);
            }
        }

        [TestClass]
        public class Serialization
        {
            [TestMethod]
            public void Serializes()
            {
                // Arrange
                var item = new CategoryItem
                {
                    Id = 10,
                    DisplayName = "myDisplayName",
                };

                // Act
                var json = JsonConvert.SerializeObject(item);

                // Assert
                AssertHelper.NormalizedAreEqual(Resources.CategoryItem, json);
            }

            [TestMethod]
            public void Deserializes()
            {
                // Arrange
                var json = Resources.CategoryItem;

                // Act
                var item = JsonConvert.DeserializeObject<CategoryItem>(json);

                // Assert
                Assert.AreEqual(10, item.Id);
                Assert.AreEqual("myDisplayName", item.DisplayName);
            }
        }
    }
}
