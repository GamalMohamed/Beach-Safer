﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Logo { get; set; }

        public string SubscriptionType { get; set; }

        public DateTime? JoinDate { get; set; }

        public virtual ICollection<Beach> Beaches { get; set; }

    }


    public class Beach
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Longitude Begining")]
        public string LongitudeBegin { get; set; }

        [Display(Name = "Longitude Ending")]
        public string LongitudeEnd { get; set; }

        public string Latitude { get; set; }

        public int? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    }
}