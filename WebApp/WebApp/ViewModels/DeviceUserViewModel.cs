using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class DeviceUserViewModel
    {
        public string Name { get; set; }

        public string ProfilePic { get; set; }

        public int? Age { get; set; }

        public string EmergencyPhone { get; set; }

        public string Gender { get; set; }

        public string SwimmingSkills { get; set; }

        public string Notes { get; set; }
    }
}