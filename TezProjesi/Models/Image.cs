using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TezProjesi.Models
{
    public partial class Image
    {
        public int Id { get; set; }
        public int? HotelId { get; set; }
        public string Title { get; set; }
        public string Image1 { get; set; }
    }
}
