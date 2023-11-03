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
    public class Guest
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_guest { get; set; }
        public string Token { get; set; }
        public string Name_guest { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }

        [JsonIgnore]
        public ICollection<GuestCart> GuestCart { get; set; }
    }
}
