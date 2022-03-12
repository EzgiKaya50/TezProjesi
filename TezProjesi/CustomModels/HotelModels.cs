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
    }
}
