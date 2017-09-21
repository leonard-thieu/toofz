using System.Runtime.Serialization;

namespace toofz
{
    [DataContract]
    public sealed class CategoriesEnvelope
    {
        [DataMember(Name = "categories", IsRequired = true)]
        public Categories Categories { get; set; }
    }
}
