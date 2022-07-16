using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TezProjesi.Models;

namespace TezProjesi.CustomModels
{
    public class RandevuModels
    {
        public List<Reservation> Reservations { get; set; }
        public int hotelId { get; set; }
        public int userId { get; set; }
        public int roomId{ get; set; }
        public string comment { get; set; }
        public float generalRate { get; set; }
        public float foodRate { get; set; }
        public float locationRate { get; set; }
        public float serviceRate { get; set; }
        public float pricePerformanceRate { get; set; }
        public float roomRate { get; set; }
    }
}
