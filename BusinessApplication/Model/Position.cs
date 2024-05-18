using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessApplication.Model
{
    public class Position
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required int PositionNumber { get; set; }
        public required int Quantity { get; set; }
        public required virtual Article ArticleDetails { get; set; }
    }
}
