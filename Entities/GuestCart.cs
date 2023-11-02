using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Entities
{
    public class GuestCart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_guestCart { get; set; }
        public DateTime date { get; set; }

        public double Price_product { get; set; }
        public int Quantity_product { get; set; }
        public double Total_price { get; set; }

        [ForeignKey("Guest")]
        public int Id_guest { get; set; }
        [JsonIgnore]
        public virtual Guest Guest { get; set; }

        // Relación con TypeUser (muchos a uno)
        [ForeignKey("Product")]
        public int Id_Product { get; set; }
        [JsonIgnore]
        public virtual Product Product { get; set; }

    }
}
