
using System;
using System.Collections.Generic;
using TezProjesi.Models;

namespace TezProjesi.CustomModels
{
    public class HomePageModels
    {
        public List<Hotel> HotelList { get; set; }
        public List<Faq> FaqList { get; set; }
        public List<Comments> CommentList { get; set; }
        public List<Room> RoomList { get; set; }
        public ReservationCRUD reservation { get; set; }
        public List<Hotel> RandomList { get; set; }
        public List<Category> CategoryList { get; set; }
        public Hotel hotelInfo { get; set; }
    }
    public class ReservationCRUD
    {
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public int HotelID { get; set; }
        public int? adults { get; set; }
        public int? children { get; set; }
        public int CategoryID { get; set; }
        public int RoomID { get; set; }
    }
    
}
