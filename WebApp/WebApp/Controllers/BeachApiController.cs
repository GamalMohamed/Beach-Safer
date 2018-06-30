using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class BeachApiController : ApiController
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: api/BeachApi/5
        public IHttpActionResult GetBeach(string id)
        {
            var beachAccess = _db.BeachAccesses.FirstOrDefault(e => e.AccessCode == id);
            if (beachAccess == null)
            {
                return NotFound();
            }
            var beach = _db.Beaches.Find(beachAccess.Beach.Id);
            if (beach == null)
            {
                return NotFound();
            }

            var beachVm = new BeachViewModel()
            {
                BeachId = beach.Id,
                Name = beach.Name,
                CustomerLogo = beach.Customer.Logo,
                CustomerName = beach.Customer.Name,
                StartPoint = beach.StartPoint,
                EndPoint = beach.EndPoint,
                LastSeaPoint = beach.LastSeaPoint
            };


            return Ok(beachVm);
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
