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
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly BlobServices _blobServices = new BlobServices();

        public string GenerateAccessCode(int id)
        {
            var rand = new Random(DateTime.Now.Millisecond);
            return rand.Next().ToString();
        }

        // GET: Customers
        public ActionResult Index()
        {
            return View(_db.Customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = _db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer, HttpPostedFileBase uploadFile)
        {
            if (ModelState.IsValid)
            {
                customer.JoinDate = DateTime.Now;
                customer.CustomerAccess = new CustomerAccess() { AccessCode = GenerateAccessCode(customer.Id) };

                if (uploadFile?.ContentLength > 0)
                {
                    var extension = Path.GetExtension(uploadFile.FileName);
                    if (extension?.ToLower() == ".png" || extension?.ToLower() == ".jpg" 
                        || extension?.ToLower() == ".jpeg")
                    {
                        var imageName = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                        imageName = Path.Combine(Server.MapPath("~/Images/") + imageName + extension);
                        uploadFile.SaveAs(imageName);

                        Upload(imageName, extension, ref customer);

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

                _db.Customers.Add(customer);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }
        
        public void Upload(string path, string extension, ref Customer customer)
        {
            var locPath = _blobServices.BlobUrl;
            var type = "customers-logos";
            var imageName = customer.Name + "-logo" + extension;
            customer.Logo = locPath + type + "/" + imageName;

            _blobServices.BlobImageUpload(imageName, path, type);

        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = _db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer, HttpPostedFileBase uploadFile)
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

                        Upload(imageName, extension, ref customer);

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
                
                _db.Entry(customer).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = _db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            _db.Customers.Remove(customer);
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
