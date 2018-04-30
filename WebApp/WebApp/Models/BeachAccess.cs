using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class BeachAccess
    {
        [Key, ForeignKey("Beach")]
        public int Id { get; set; }

        public string AccessCode { get; set; }

        public string AccessCodeHash { get; set; }

        public virtual Beach Beach { get; set; }
    
    }
}