using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Device
    {
        [Key, ForeignKey("DeviceUser")]
        public int Id { get; set; }

        public bool? IsOwned { get; set; }

        public int? DeviceUserId { get; set; }

        [ForeignKey("DeviceUserId")]
        public virtual DeviceUser DeviceUser { get; set; }

        public virtual ICollection<DeviceLog> DeviceLogs { get; set; }
    }

    public class DeviceLog
    {
        [Key]
        public int Id { get; set; }

        public int? DeviceId { get; set; }

        [ForeignKey("DeviceId")]
        public virtual Device Device { get; set; }

        public DateTime? Timestamp { get; set; }

        public int? MessageId { get; set; }

        public string State { get; set; }

        public string Location { get; set; }
    }
}