using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Device
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Device State")]
        public DeviceState DeviceState { get; set; }

        [Display(Name = "Device Type")]
        public DeviceType DeviceType { get; set; }

        public int? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public int? DeviceUserId { get; set; }

        public virtual ICollection<DeviceLog> DeviceLogs { get; set; }
    }

    public enum DeviceState
    {
        InStock,
        Sold,
        Rented,
        Suspended
    }

    public enum DeviceType
    {
        Wearable,
        Drone
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