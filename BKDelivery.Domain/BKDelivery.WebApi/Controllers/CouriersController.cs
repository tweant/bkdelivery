using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BKDelivery.WebApi.Models;

namespace BKDelivery.WebApi.Controllers
{
    public class CouriersController : ApiController
    {
        private BKDeliveryWebApiContext db = new BKDeliveryWebApiContext();

        // GET: api/Couriers
        public IQueryable<Courier> GetCouriers()
        {
            return db.Couriers;
        }

        // GET: api/Couriers/5
        [ResponseType(typeof(Courier))]
        public async Task<IHttpActionResult> GetCourier(int id)
        {
            Courier courier = await db.Couriers.FindAsync(id);
            if (courier == null)
            {
                return NotFound();
            }

            return Ok(courier);
        }

        [Route("api/Couriers/search")]
        public IQueryable<Courier> GetCourierByEmail(string email)
        {
            return db.Couriers.Where(o => o.Email.Equals(email));
        }

        // PUT: api/Couriers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCourier(int id, Courier courier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != courier.CourierId)
            {
                return BadRequest();
            }

            db.Entry(courier).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourierExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Couriers
        [ResponseType(typeof(Courier))]
        public async Task<IHttpActionResult> PostCourier(Courier courier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Couriers.Add(courier);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = courier.CourierId }, courier);
        }

        // DELETE: api/Couriers/5
        [ResponseType(typeof(Courier))]
        public async Task<IHttpActionResult> DeleteCourier(int id)
        {
            Courier courier = await db.Couriers.FindAsync(id);
            if (courier == null)
            {
                return NotFound();
            }

            db.Couriers.Remove(courier);
            await db.SaveChangesAsync();

            return Ok(courier);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourierExists(int id)
        {
            return db.Couriers.Count(e => e.CourierId == id) > 0;
        }
    }
}