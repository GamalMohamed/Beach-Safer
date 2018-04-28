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
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DeviceUsers
        public ActionResult Index()
        {
            //var deviceUsers = db.DeviceUsers.Include(d => d.Device);
            return View(db.DeviceUsers.ToList());
        }

        // GET: DeviceUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceUser deviceUser = db.DeviceUsers.Find(id);
            if (deviceUser == null)
            {
                return HttpNotFound();
            }
            return View(deviceUser);
        }

        // GET: DeviceUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeviceUsers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,ProfilePic,Age,Email,Phone,EmergencyPhone,Gender,SwimmingSkills,Notes")] DeviceUser deviceUser)
        {
            if (ModelState.IsValid)
            {
                db.DeviceUsers.Add(deviceUser);
                db.SaveChanges();
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
            DeviceUser deviceUser = db.DeviceUsers.Find(id);
            if (deviceUser == null)
            {
                return HttpNotFound();
            }
            return View(deviceUser);
        }

        // POST: DeviceUsers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,ProfilePic,Age,Email,Phone,EmergencyPhone,Gender,SwimmingSkills,Notes")] DeviceUser deviceUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deviceUser).State = EntityState.Modified;
                db.SaveChanges();
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
            DeviceUser deviceUser = db.DeviceUsers.Find(id);
            if (deviceUser == null)
            {
                return HttpNotFound();
            }
            db.DeviceUsers.Remove(deviceUser);
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
