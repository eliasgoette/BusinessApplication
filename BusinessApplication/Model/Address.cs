using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace BusinessApplication.Model
{
    [ExcludeFromCodeCoverage]
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        [XmlIgnore]
        public int Id { get; set; }

        public required string Country { get; set; }
        public required string ZipCode { get; set; }
        public required string City { get; set; }
        public required string StreetAddress { get; set; }
    }
}
