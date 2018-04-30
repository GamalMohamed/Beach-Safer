using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class DeviceUsersController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly BlobServices _blobServices = new BlobServices();

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
        public ActionResult Create(DeviceUser deviceUser, HttpPostedFileBase uploadFile)
        {
            if (ModelState.IsValid)
            {
                if (uploadFile?.ContentLength > 0)
                {
                    var extension = Path.GetExtension(uploadFile.FileName);
                    if (extension?.ToLower() == ".png" || extension?.ToLower() == ".jpg"
                        || extension?.ToLower() == ".jpeg")
                    {
                        var imageName = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                        imageName = Path.Combine(Server.MapPath("~/Images/") + imageName + extension);
                        uploadFile.SaveAs(imageName);

                        Upload(imageName, extension, ref deviceUser);

                        // Delete file from server after finishing 
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        System.IO.File.Delete(imageName);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "File type not supported for upload. Available formats: .pdf, .png, .jpg, .jpeg";
                        return View("Error");
                    }

                }

                _db.DeviceUsers.Add(deviceUser);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deviceUser);
        }

        public void Upload(string path, string extension, ref DeviceUser deviceUser)
        {
            var locPath = _blobServices.BlobUrl;
            var type = "deviceusers-profilepics";
            var imageName = deviceUser.Name + "-profilepic"+ extension;
            deviceUser.ProfilePic = locPath + type + "/" + imageName;

            _blobServices.BlobImageUpload(imageName, path, type);

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
        public ActionResult Edit(DeviceUser deviceUser, HttpPostedFileBase uploadFile)
        {
            if (ModelState.IsValid)
            {
                if (uploadFile?.ContentLength > 0)
                {
                    var extension = Path.GetExtension(uploadFile.FileName);
                    if (extension?.ToLower() == ".png" || extension?.ToLower() == ".jpg"
                        || extension?.ToLower() == ".jpeg")
                    {
                        var imageName = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                        imageName = Path.Combine(Server.MapPath("~/Images/") + imageName + extension);
                        uploadFile.SaveAs(imageName);

                        Upload(imageName, extension, ref deviceUser);

                        // Delete file from server after finishing 
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        System.IO.File.Delete(imageName);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "File type not supported for upload. Available formats: .pdf, .png, .jpg, .jpeg";
                        return View("Error");
                    }

                }

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
