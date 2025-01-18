using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class CategoriesDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BusinessID { get; set; }
    }


    public class UpdateCategoryDto {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}