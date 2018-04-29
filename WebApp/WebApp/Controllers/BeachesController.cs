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
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Beaches
        public ActionResult Index()
        {
            return View(_db.Beaches.ToList());
        }

        // GET: Beaches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beach beach = _db.Beaches.Find(id);
            if (beach == null)
            {
                return HttpNotFound();
            }
            return View(beach);
        }

        // GET: Beaches/Create
        public ActionResult Create()
        {
            ViewBag.CustomersList = new SelectList(_db.Customers.ToList(), "Id", "Name");
            return View();
        }

        // POST: Beaches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Beach beach)
        {
            if (ModelState.IsValid)
            {
                _db.Beaches.Add(beach);
                _db.SaveChanges();
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
            Beach beach = _db.Beaches.Find(id);
            if (beach == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomersList = new SelectList(_db.Customers.ToList(), "Id", "Name");
            return View(beach);
        }

        // POST: Beaches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Beach beach)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(beach).State = EntityState.Modified;
                _db.SaveChanges();
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
            Beach beach = _db.Beaches.Find(id);
            if (beach == null)
            {
                return HttpNotFound();
            }
            _db.Beaches.Remove(beach);
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
