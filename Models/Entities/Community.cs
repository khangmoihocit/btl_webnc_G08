using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace btl_webnc_G08.Models.Entities
{
    public class Community
    {
        public int Id { get; set; }
        [MaxLength(200)] 
        public string Name { get; set; } = string.Empty;
        [MaxLength(500)] public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public virtual ICollection<User> Members { get; set; } = new List<User>();
        public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}