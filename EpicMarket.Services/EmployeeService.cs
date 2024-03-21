using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using NVelocity;
using NVelocity.App;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly IApplicationConfigurationService applicationConfiguration;
        private readonly ICommunicationService communicationService;

        public EmployeeService(ApplicationDbContext context, IMapper mapper , IApplicationConfigurationService applicationConfiguration,ICommunicationService communicationService)
        {
            _context = context;
            this.mapper = mapper;
            this.applicationConfiguration = applicationConfiguration;
            this.communicationService = communicationService;
        }
        public AddEmployeeResult Register(AddEmployeeParam addEmployeeParam)
        {
            var employee = new AddEmployeeResult();

            var EmailTemplete = applicationConfiguration.GetApplicationConfigurationValue("BusinessOwnerInvitation");


            Hashtable ListValues = new Hashtable();

            ListValues.Add("fromAddress", "akhil@epicmarket.in");
            ListValues.Add("BussinessName", "Kart");
            ListValues.Add("BusinessOwnerName", "Gadamsetti Akhil");
            ListValues.Add("RedirectUrl", "www.google.com");

            VelocityEngine v = new VelocityEngine();
            v.Init();

            VelocityContext context = new VelocityContext(ListValues);
            StringWriter writer = new StringWriter();
            v.Evaluate(context, writer, string.Empty, EmailTemplete);
            var message = writer.ToString();

            communicationService.SendEmailAsync("akhil@epicmarket.in", "Invitation", message);

            return employee;


        }
    }
}
