using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TezProjesi.Models
{
    public partial class Reservation
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? HotelId { get; set; }
        public int? RoomId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public double? Total { get; set; }
        public bool? Checkin { get; set; }
        public bool? Checkout { get; set; }
        public int? Days { get; set; }
        public int? Adults { get; set; }
        public int? Children { get; set; }
        public string Ip { get; set; }
        public string Note { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
