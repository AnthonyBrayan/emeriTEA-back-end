using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities
{
    public class ProductSize
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_ProductSize { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }

        [ForeignKey("Size")]
        public int SizeId { get; set; }
        [JsonIgnore]
        public Size Size { get; set; }
    }
}
