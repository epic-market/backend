using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
    public class DropDownOptions
    {
        public string  Text { get; set; }

        public int Value { get; set; }
    }
    public class HelpItemDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int? PageID { get; set; }

        public bool IsShownOnPage { get; set; }
    }
    public class BranchsDropDownOptions
    {
        public string Text { get; set; }

        public int Value { get; set; }
        public bool? IsOpen { get; set; } = false;
    }
}
