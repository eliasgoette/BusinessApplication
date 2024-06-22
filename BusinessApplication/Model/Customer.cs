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
        [JsonIgnore]
        [XmlIgnore]
        public string? FirstName { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        public string? LastName { get; set; }
        [NotMapped]
        public string Name
        {
            get => $"{FirstName} {LastName}";
            set
            {
                var parts = value.Split(' ');
                if (parts.Length > 1)
                {
                    FirstName = parts[0];
                    LastName = string.Join(" ", parts.Skip(1));
                }
            }
        }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? PasswordHash { get; set; }
    }
}