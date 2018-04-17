﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {

        private string _to = Startup.Configuration["mailSettings:mailToAddress"];
        private string _from = Startup.Configuration["mailSettings:mailFromAddress"];

        public void Send(string subject, string message)
        {

            Debug.WriteLine($"mail from {_from} to {_to} , with localmailservice");
            Debug.WriteLine($" subject {subject}");
            Debug.WriteLine($" message {message}");


        }
    }
}
