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
        public required Article ArticleDetails { get; set; }
        public int Quantity { get; set; }
    }
}
