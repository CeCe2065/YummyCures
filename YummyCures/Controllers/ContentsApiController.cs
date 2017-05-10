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
using YummyCures.Models;

namespace YummyCures.Controllers
{
    public class ContentsApiController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ContentsApi
        public IQueryable<Content> GetContents()
        {
            return db.Contents;
        }

        // GET: api/ContentsApi/5
        [ResponseType(typeof(Content))]
        public IHttpActionResult GetContent(int id)
        {
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return NotFound();
            }

            return Ok(content);
        }

        // PUT: api/ContentsApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutContent(int id, Content content)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != content.ContentID)
            {
                return BadRequest();
            }

            db.Entry(content).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContentExists(id))
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

        // POST: api/ContentsApi
        [ResponseType(typeof(Content))]
        public IHttpActionResult PostContent(Content content)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Contents.Add(content);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = content.ContentID }, content);
        }

        // DELETE: api/ContentsApi/5
        [ResponseType(typeof(Content))]
        public IHttpActionResult DeleteContent(int id)
        {
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return NotFound();
            }

            db.Contents.Remove(content);
            db.SaveChanges();

            return Ok(content);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContentExists(int id)
        {
            return db.Contents.Count(e => e.ContentID == id) > 0;
        }
    }
}