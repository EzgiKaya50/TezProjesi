using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TezProjesi.Models;

namespace TezProjesi.CustomModels
{
    public class RoomModels
    {
       public class RoomPage
        {
            public RoomType RoomType { get; set; }
            public List<Room> RoomList { get; set; }
            public Room RoomCRUD { get; set; }
            public IFormFile RoomImage { get; set; }
            public List<Hotel> HotelList { get; set; }

        }

        public partial class RoomType
        {
            public SelectList Type
            {
                get
                {
                    return new SelectList(
                    new List<SelectListItem>
                         {
                        new SelectListItem {Text = "Suit Oda", Value = "1"},
                        new SelectListItem {Text = "Dublex Oda", Value = "2"},
                        new SelectListItem {Text = "Kral Dairesi", Value = "3"},
                        new SelectListItem {Text = "Aile Odası", Value = "4"},
                        new SelectListItem {Text = "Tek Kişilik Oda", Value = "5"},
                        new SelectListItem {Text = "Çift Kişilik Oda", Value = "6"},
                         }, "Value", "Text");
                }
            }
        }
    }
}
