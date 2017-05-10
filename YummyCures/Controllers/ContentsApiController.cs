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
using Microsoft.AspNet.Identity;

namespace YummyCures.Controllers

{
    [Authorize]
    public class ContentsApiController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ContentsApi
        public IQueryable<Content> GetContents()
        {
            string userID = User.Identity.GetUserId();
            return db.Contents.Where(p => p.UserID == userID);
        }

        // GET: api/ContentsApi/5
        [ResponseType(typeof(Content))]
        public IHttpActionResult GetContent(int id)
        {
            string userID = User.Identity.GetUserId();
            Content content = db.Contents.Where(p => p.UserID == userID && p.ContentID == id).FirstOrDefault();


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

            //Going to get the original and move all the properties over from the project that is passed in.

            string userID = User.Identity.GetUserId();
            Content originalContent = db.Contents.Where(p => p.UserID == userID && p.ContentID == id).FirstOrDefault();

            if (originalContent == null)
            {
                return NotFound();
            }

            //Move over all the properties that need to be set.

            originalContent.ContentTypeID = content.ContentTypeID;
            originalContent.ContentBody = content.ContentBody;
            originalContent.Title = content.Title;
            originalContent.PreviewUrl = content.PreviewUrl;


            db.Entry(originalContent).State = EntityState.Modified;

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
            content.UserID = User.Identity.GetUserId();

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
            string userID = User.Identity.GetUserId();
            Content content = db.Contents.Where(p => p.UserID == userID && p.ContentID == id).FirstOrDefault();

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
            string userID = User.Identity.GetUserId();
            return db.Contents.Count(p => p.UserID == userID && p.ContentID == id) > 0;

        }
    }
}