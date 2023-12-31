﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using System.Text.Json.Serialization;

namespace Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Product { get; set; }
        public string Name_product { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public double Price { get; set; }
        public int stock { get; set; }
        [NotMapped]
        public string[] size { get; set; }

        [ForeignKey("Category")]
        public int Id_Category { get; set; }
        [JsonIgnore]
        public virtual Category Category { get; set; }

        // Relación con TypeUser (muchos a uno)
        [ForeignKey("Administrador")]
        public int Id_Administrador { get; set; }
        [JsonIgnore]
        public virtual Administrador Administrador { get; set; }

        [JsonIgnore]
        public ICollection<ProductSize> ProductSize { get; set; }

        [JsonIgnore]
        public ICollection<GuestCart> GuestCart { get; set; }
    }
}
