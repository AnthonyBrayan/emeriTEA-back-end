
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities
{
    public class Size
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_size { get; set; }
        public string Name_size { get; set; }

        [JsonIgnore]
        public ICollection<ProductSize> ProductSize { get; set; }

    }
}
