using HBD.Libraries.Net.Email.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HBD.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            HBD.Libraries.Net.Email.EmailManager.SendAll();
        }
    }
}
