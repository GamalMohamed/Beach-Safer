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
    public class DeviceUsersController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: DeviceUsers
        public ActionResult Index()
        {
            return View(_db.DeviceUsers.ToList());
        }

        // GET: DeviceUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var deviceUser = _db.DeviceUsers.Find(id);
            if (deviceUser == null)
            {
                return HttpNotFound();
            }
            return View(deviceUser);
        }

        // GET: DeviceUsers/Create
        public ActionResult Create()
        {
            ViewBag.CustomersList = new SelectList(_db.Customers.ToList(), "Id", "Name");
            return View();
        }

        // POST: DeviceUsers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DeviceUser deviceUser)
        {
            if (ModelState.IsValid)
            {
                _db.DeviceUsers.Add(deviceUser);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deviceUser);
        }

        // GET: DeviceUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var deviceUser = _db.DeviceUsers.Find(id);
            if (deviceUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomersList = new SelectList(_db.Customers.ToList(), "Id", "Name");
            ViewBag.DevicesList = new SelectList(_db.Devices.ToList(), "Id", "Id");
            return View(deviceUser);
        }

        // POST: DeviceUsers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DeviceUser deviceUser)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(deviceUser).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deviceUser);
        }

        // GET: DeviceUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var deviceUser = _db.DeviceUsers.Find(id);
            if (deviceUser == null)
            {
                return HttpNotFound();
            }
            _db.DeviceUsers.Remove(deviceUser);
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
