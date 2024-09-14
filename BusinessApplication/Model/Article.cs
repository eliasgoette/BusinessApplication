using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace BusinessApplication.Model
{
    [ExcludeFromCodeCoverage]
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string ArticleNumber { get; set; }
        public required string Name { get; set; }
        public required double Price { get; set; }
    }
}
