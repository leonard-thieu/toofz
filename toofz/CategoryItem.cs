using System.Runtime.Serialization;

namespace toofz
{
    [DataContract]
    public sealed class CategoryItem
    {
        [DataMember(Name = "id", IsRequired = true)]
        public int Id { get; set; }
        [DataMember(Name = "display_name", IsRequired = true)]
        public string DisplayName { get; set; }
    }
}