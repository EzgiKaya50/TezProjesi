using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TezProjesi.Models;

namespace TezProjesi.CustomModels
{
    public class CalendarModel
    {
        public List<Reservation> checkinList { get; set; }
        public List<Reservation> checkoutList { get; set; }
        public List<Reservation> finishedList { get; set; }
    }
}
