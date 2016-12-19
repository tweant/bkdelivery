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
    public class OrdersController : ApiController
    {
        private BKDeliveryWebApiContext db = new BKDeliveryWebApiContext();

        // GET: api/Orders
        public IQueryable<Order> GetOrders()
        {
            return db.Orders;
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> GetOrder(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [Route("api/Orders/search")]
        public IQueryable<Order> GetOrdersByName(int orderId, int clientId, int courierId)
        {
            if (orderId != 0 && clientId != 0 && courierId != 0)
            {
                return db.Orders.Where(x => x.OrderId == orderId && x.ClientId == clientId && x.CourierId == courierId);
            }
            else if (orderId != 0 && clientId != 0)
            {
                return db.Orders.Where(x => x.OrderId == orderId && x.ClientId == clientId);
            }
            else if (clientId != 0 && courierId != 0)
            {
                return db.Orders.Where(x => x.ClientId == clientId && x.CourierId == courierId);
            }
            else if (orderId != 0 && courierId != 0)
            {
                return db.Orders.Where(x => x.OrderId == orderId && x.CourierId == courierId);
            }
            else if (orderId != 0)
            {
                return db.Orders.Where(x => x.OrderId == orderId);
            }
            else if (clientId != 0)
            {
                return db.Orders.Where(x => x.ClientId == clientId);
            }
            else if (courierId != 0)
            {
                return db.Orders.Where(x => x.CourierId == courierId);
            }
            else
            {
                return db.Orders;
            }
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.OrderId)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Orders.Add(order);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = order.OrderId }, order);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> DeleteOrder(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            await db.SaveChangesAsync();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.OrderId == id) > 0;
        }
    }
}