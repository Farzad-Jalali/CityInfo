using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {

        private string _to = "admin@jalai.com";
        private string _from = "noreply@jalai.com";

        public void Send(string subject , string message)
        {

            Debug.WriteLine($"mail from {_from} to {_to} , with localmailservice");
            Debug.WriteLine($" subject {subject}");
            Debug.WriteLine($" message {message}");


        }
    }
}
