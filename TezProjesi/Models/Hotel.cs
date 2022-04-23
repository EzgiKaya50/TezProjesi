using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TezProjesi.Models
{
    public partial class Hotel
    {
        public Hotel()
        {
            Image = new HashSet<Image>();
            Room = new HashSet<Room>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public string Detail { get; set; }
        public int? Star { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Location { get; set; }
        public int? Userid { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Image> Image { get; set; }
        public virtual ICollection<Room> Room { get; set; }
    }
}
