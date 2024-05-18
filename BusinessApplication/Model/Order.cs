using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessApplication.Model
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string OrderNumber { get; set; }
        public DateTime Date { get; } = DateTime.UtcNow;
        public required virtual Customer CustomerDetails { get; set; }
        public required virtual ICollection<Position> Positions { get; set; } = new List<Position>();
    }
}
