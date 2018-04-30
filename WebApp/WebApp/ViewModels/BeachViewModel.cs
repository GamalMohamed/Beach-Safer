using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.ViewModels
{
    public class BeachViewModel
    {
        public string Name { get; set; }

        public string StartPoint { get; set; }

        public string EndPoint { get; set; }

        public string LastSeaPoint { get; set; }

        public string CustomerName { get; set; }

        public string CustomerLogo { get; set; }

    }
}