using System.Collections.Generic;
using System.Runtime.Serialization;

namespace toofz
{
    [DataContract]
    public sealed class Categories : Dictionary<string, Category> { }
}
