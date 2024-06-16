using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace BusinessApplication.Model
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        [XmlIgnore]
        public int Id { get; set; }
        public required virtual Address CustomerAddress { get; set; }
        public required string CustomerNumber { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? PasswordHash { get; set; }
    }
}