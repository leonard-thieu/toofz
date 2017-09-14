using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace toofz
{
    [DataContract]
    public sealed class Category : Dictionary<string, CategoryItem>
    {
        public string GetName(int id)
        {
            try
            {
                return this.Single(c => c.Value.Id == id).Key;
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException(message: $"Unable to find an item with id '{id}'.", paramName: nameof(id));
            }
        }

        public string GetNamesAsCommaSeparatedValues()
        {
            var itemNames = this.Select(c => c.Key);

            return string.Join(",", itemNames);
        }
    }
}