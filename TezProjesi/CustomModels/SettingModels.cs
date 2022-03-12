using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TezProjesi.Models;

namespace TezProjesi.CustomModels
{
    public class SettingModels
    {
        public class SettingPage
        {
            public List<Setting> SettingList { get; set; }
            public List<Hotel> HotelList { get; set; }
            public Setting SettingCRUD { get; set; }          
        }
    }
}
