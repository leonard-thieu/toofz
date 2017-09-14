using System.Runtime.Serialization;

namespace toofz
{
    [DataContract]
    public sealed class CategoryItem
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "display_name")]
        public string DisplayName { get; set; }
    }
}