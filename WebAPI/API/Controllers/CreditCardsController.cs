using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using API.Models;

namespace API.Controllers
{
    public class CreditCardsController : ApiController
    {
        private AWEntities db = new AWEntities();

        // GET: api/CreditCards
        public IQueryable<CreditCard> GetCreditCard()
        {
            return db.CreditCard.Take(100);
        }

        // GET: api/CreditCards/5
        [ResponseType(typeof(CreditCard))]
        public IHttpActionResult GetCreditCard(int id)
        {
            CreditCard creditCard = db.CreditCard.Find(id);
            if (creditCard == null)
            {
                return NotFound();
            }

            return Ok(creditCard);
        }

        // PUT: api/CreditCards/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCreditCard(int id, CreditCard creditCard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != creditCard.CreditCardID)
            {
                return BadRequest();
            }

            db.Entry(creditCard).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CreditCardExists(id))
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

        // POST: api/CreditCards
        [ResponseType(typeof(CreditCard))]
        public IHttpActionResult PostCreditCard(CreditCard creditCard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CreditCard.Add(creditCard);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = creditCard.CreditCardID }, creditCard);
        }

        // DELETE: api/CreditCards/5
        [ResponseType(typeof(CreditCard))]
        public IHttpActionResult DeleteCreditCard(int id)
        {
            CreditCard creditCard = db.CreditCard.Find(id);
            if (creditCard == null)
            {
                return NotFound();
            }

            db.CreditCard.Remove(creditCard);
            db.SaveChanges();

            return Ok(creditCard);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CreditCardExists(int id)
        {
            return db.CreditCard.Count(e => e.CreditCardID == id) > 0;
        }
    }
}