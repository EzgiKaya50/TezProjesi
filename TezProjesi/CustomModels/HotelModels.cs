using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TezProjesi.Models;

namespace TezProjesi.CustomModels
{
    public class HotelModels
    {
        public class HotelPage
        {
            public List<Hotel> HotelList { get; set; }
            public Hotel HotelCRUD { get; set; }
            public IFormFile HotelImage { get; set; }
            public List<Category> CategoryList { get; set; }

        }

        public class Payment
        {
            public double totalPrice { get; set; }
            public string hotelName { get; set; }
            public int hotelID { get; set; }
            public string roomName { get; set; }
            public int roomID { get; set; }
            public DateTime startDate { get; set; }
            public DateTime endDate { get; set; }
            public int adult { get; set; }
            public int children { get; set; }
            public string cardNo { get; set; }
            public string cvv { get; set; }
            public int? cardMonth { get; set; }
            public int? cardYear { get; set; }
            public string cardNameSurname { get; set; }
        }
    }
}
