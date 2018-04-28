using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class DeviceUser
    {
        [Key, ForeignKey("Device")]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Profile Picture")]
        public string ProfilePic { get; set; }

        public int? Age { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Phone]
        public string EmergencyPhone { get; set; }

        public string Gender { get; set; }

        [Display(Name = "Swimming Skills")]
        public string SwimmingSkills { get; set; }

        public string Notes { get; set; }

        public int DeviceId { get; set; }

        [ForeignKey("DeviceId")]
        public virtual Device Device { get; set; }

        public int? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

    }

    
}