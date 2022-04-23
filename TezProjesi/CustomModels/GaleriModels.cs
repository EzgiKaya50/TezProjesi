using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TezProjesi.Models;

namespace TezProjesi.CustomModels
{
    public class GaleriModels
    {
        public class GaleriPage
        {
           public List<Image> ImageList { get; set; }
            public Image ImageCRUD { get; set; } 
            public IFormFile  Image { get; set; }
            public List<Hotel> HotelList { get; set; }


        }
    }
}
