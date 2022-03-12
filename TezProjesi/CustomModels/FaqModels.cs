using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TezProjesi.Models;

namespace TezProjesi.CustomModels
{
    public class FaqModels
    {
        public class FaqPage
        {
            public List<Faq> FaqList { get; set; }
            public Faq FaqCRUD { get; set; }
        }
    }
}
