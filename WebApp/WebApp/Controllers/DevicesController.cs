using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class DevicesController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Devices
        public ActionResult Index()
        {
            return View(_db.Devices.ToList());
        }

        // GET: Devices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var device = _db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }

            var deviceUser = _db.DeviceUsers.Find(device.DeviceUserId);
            if (deviceUser != null)
            {
                ViewData["DeviceUserName"] = deviceUser.Name;
            }
            else
            {
                ViewData["DeviceUserName"] = "NA";
            }
            return View(device);
        }

        // GET: Devices/Create
        public ActionResult Create()
        {
            ViewBag.CustomersList = new SelectList(_db.Customers.ToList(), "Id", "Name");
            return View();
        }

        // POST: Devices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Device device)
        {
            if (ModelState.IsValid)
            {
                _db.Devices.Add(device);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(device);
        }

        // GET: Devices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var device = _db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }

            var inactiveUsers = _db.DeviceUsers.Where(e => e.DeviceId == 0).ToList();
            var currentDeviceUser = _db.DeviceUsers.Find(device.DeviceUserId);
            if (currentDeviceUser != null)
            {
                inactiveUsers.Add(currentDeviceUser);
            }
            ViewBag.DeviceUsersList = new SelectList(inactiveUsers, "Id", "Name");
            ViewBag.CustomersList = new SelectList(_db.Customers.ToList(), "Id", "Name");
            if (device.DeviceUserId != null)
            {
                TempData["PrevDeviceUserId"] = device.DeviceUserId;
            }
            return View(device);
        }

        // POST: Devices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Device device)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(device).State = EntityState.Modified;
                _db.SaveChanges();

                if (!TempData.ContainsKey("PrevDeviceUserId")) // No prev user === 1st time user registration
                {
                    var deviceUser = _db.DeviceUsers.Find(device.DeviceUserId);
                    if (deviceUser != null)
                    {
                        deviceUser.DeviceId = device.Id;
                        _db.SaveChanges();
                    }
                }
                else
                {
                    var prevDeviceUserId = int.Parse(TempData["PrevDeviceUserId"].ToString());
                    if (device.DeviceUserId != prevDeviceUserId)
                    {
                        var deviceUser = _db.DeviceUsers.Find(prevDeviceUserId); // decrement prev
                        if (deviceUser != null)
                        {
                            deviceUser.DeviceId = 0;
                            _db.SaveChanges();
                        }

                        if (device.DeviceUserId != null) // increment new
                        {
                            deviceUser = _db.DeviceUsers.Find(device.DeviceUserId);
                            if (deviceUser != null)
                            {
                                deviceUser.DeviceId = device.Id;
                                _db.SaveChanges();
                            }
                        }
                    }
                }

            }

            return RedirectToAction("Index");
        }    

    // GET: Devices/Delete/5
    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        var device = _db.Devices.Find(id);
        if (device == null)
        {
            return HttpNotFound();
        }
        _db.Devices.Remove(device);
        _db.SaveChanges();
        return RedirectToAction("Index");
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
