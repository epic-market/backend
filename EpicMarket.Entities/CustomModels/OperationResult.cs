using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Entities.CustomModels
{
	public class OperationResult<T>
	{

		public OperationResult()
		{
			this.Status = OperationStatus.SUCCESS;
		}

		public string Status { get; set; }
		public string Message { get; set; } = "";
		public string RedirectUrl { get; set; } = "";
		public T Data { get; set; }
		public string ErrorDetail { get; set; } = "";

	}
}
