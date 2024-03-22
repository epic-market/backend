using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface ICommunicationService
    {
        public void SendEmailAsync(string email, string subject, string message);
    }
}
