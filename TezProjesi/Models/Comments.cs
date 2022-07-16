using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TezProjesi.Models
{
    public partial class Comments
    {
        public int Id { get; set; }
        public string UserComment { get; set; }
        public double? FoodRate { get; set; }
        public double? ServiceRate { get; set; }
        public double? RoomRate { get; set; }
        public double? GeneralRate { get; set; }
        public double? PricePerformanceRate { get; set; }
        public double? LocationRate { get; set; }
        public int? HotelId { get; set; }
        public int? UserId { get; set; }
        public int? Ip { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Hotel Hotel { get; set; }
        public virtual User User { get; set; }
    }
}
