using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class CustomerAccess
    {
        [Key, ForeignKey("Customer")]
        public int Id { get; set; }

        public string AccessCode { get; set; }

        public string AccessCodeHash { get; set; }

        public int? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    
    }
}