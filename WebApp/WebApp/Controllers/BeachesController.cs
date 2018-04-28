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
    public class BeachesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Beaches
        public ActionResult Index()
        {
            var beaches = db.Beaches.Include(b => b.Customer);
            return View(beaches.ToList());
        }

        // GET: Beaches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beach beach = db.Beaches.Find(id);
            if (beach == null)
            {
                return HttpNotFound();
            }
            return View(beach);
        }

        // GET: Beaches/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beaches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,StartPoint,EndPoint,LastSeaPoint")] Beach beach)
        {
            if (ModelState.IsValid)
            {
                db.Beaches.Add(beach);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(beach);
        }

        // GET: Beaches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beach beach = db.Beaches.Find(id);
            if (beach == null)
            {
                return HttpNotFound();
            }
            return View(beach);
        }

        // POST: Beaches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,StartPoint,EndPoint,LastSeaPoint")] Beach beach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beach);
        }

        // GET: Beaches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beach beach = db.Beaches.Find(id);
            if (beach == null)
            {
                return HttpNotFound();
            }
            db.Beaches.Remove(beach);
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
