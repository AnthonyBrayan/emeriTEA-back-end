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
    public class Administrador
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Administrador { get; set; }
        public string Name_administrador { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [JsonIgnore]
        public ICollection<Product> Product { get; set; }
    }
}
