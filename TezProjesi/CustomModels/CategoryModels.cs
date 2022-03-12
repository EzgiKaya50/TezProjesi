using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TezProjesi.Models;

namespace TezProjesi.CustomModels
{
    public class CategoryModels
    {
        public class YeniKategoriEklePage
        {
            public List<Category> CategoryList { get; set; }
            public Category CategoryCRUD { get; set; }

            public IFormFile CategoryImage { get; set; }
        }
    }
}
