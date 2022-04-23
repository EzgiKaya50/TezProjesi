using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TezProjesi.Models
{
    public partial class Room
    {
        public Room()
        {
            RoomImages = new HashSet<RoomImages>();
        }

        public int Id { get; set; }
        public int? HotelId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double? Price { get; set; }
        public int? Adet { get; set; }
        public int? Type { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedInt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? Adult { get; set; }
        public int? Children { get; set; }

        public virtual Hotel Hotel { get; set; }
        public virtual ICollection<RoomImages> RoomImages { get; set; }
    }
}
