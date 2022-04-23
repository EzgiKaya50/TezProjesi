using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TezProjesi.Models;

namespace TezProjesi.CustomModels
{
    public class RoomGaleriModels
    {
        public class RoomGaleriPage
        {
            public List<RoomImages> ImageList { get; set; }
            public RoomImages RoomImgCRUD { get; set; }
            public IFormFile RoomImg { get; set; }
            public List<Room> RoomList { get; set; }
        }
    }
}
