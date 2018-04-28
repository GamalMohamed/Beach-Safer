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
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Devices
        public ActionResult Index()
        {
            var devices = db.Devices.Include(d => d.DeviceUser);
            return View(devices.ToList());
        }

        // GET: Devices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // GET: Devices/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.DeviceUsers, "Id", "Name");
            return View();
        }

        // POST: Devices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IsOwned,DeviceUserId")] Device device)
        {
            if (ModelState.IsValid)
            {
                db.Devices.Add(device);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.DeviceUsers, "Id", "Name", device.Id);
            return View(device);
        }

        // GET: Devices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.DeviceUsers, "Id", "Name", device.Id);
            return View(device);
        }

        // POST: Devices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IsOwned,DeviceUserId")] Device device)
        {
            if (ModelState.IsValid)
            {
                db.Entry(device).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.DeviceUsers, "Id", "Name", device.Id);
            return View(device);
        }

        // GET: Devices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            db.Devices.Remove(device);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
