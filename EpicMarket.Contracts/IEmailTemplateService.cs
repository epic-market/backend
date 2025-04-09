using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IEmailTemplateService
    {
        public Task<string> RenderEmailTemplate(string templateName, object model);
    }
}