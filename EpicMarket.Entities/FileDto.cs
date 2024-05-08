using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities
{
	public class FileDto
	{

		public Stream fileStream { get; set; }

        public string contentType { get; set; }
    }
}
