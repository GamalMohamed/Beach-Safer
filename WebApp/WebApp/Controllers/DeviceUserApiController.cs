using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class DeviceUserApiController : ApiController
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: api/DeviceUserApi/5
        public IHttpActionResult GetDeviceUser(int id)
        {
            var device = _db.Devices.Find(id);
            if (device == null)
            {
                return NotFound();
            }
            var deviceUser = _db.DeviceUsers.Find(device.DeviceUserId);
            if (deviceUser == null)
            {
                return NotFound();
            }

            var deviceUserVm = new DeviceUserViewModel()
            {
                Name = deviceUser.Name,
                Gender = deviceUser.Gender,
                Age = deviceUser.Age,
                EmergencyPhone = deviceUser.EmergencyPhone,
                ProfilePic = deviceUser.ProfilePic,
                SwimmingSkills = deviceUser.SwimmingSkills,
                Notes = deviceUser.Notes
            };


            return Ok(deviceUserVm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}