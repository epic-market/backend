using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{

    public class ReviewResult
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string Rating { get; set; }

        public string Review { get; set; }
            
        public DateTime CreateDate { get; set; }
    }

}
