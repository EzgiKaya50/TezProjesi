
using System;
using System.Collections.Generic;
using TezProjesi.Models;

namespace TezProjesi.CustomModels
{
    public class HomePageModels
    {
        public List<Hotel> HotelList { get; set; }
        public List<Faq> FaqList { get; set; }
        public List<Comment> CommentList { get; set; }
        public List<Room> RoomList { get; set; }
        public ReservationCRUD reservation { get; set; }
    }
    public class ReservationCRUD
    {
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public int HotelID { get; set; }
        public int? adults { get; set; }
        public int? children { get; set; }
    }
    
}
