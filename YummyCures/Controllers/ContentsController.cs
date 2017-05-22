using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YummyCures.Models;
using Microsoft.AspNet.Identity;

namespace YummyCures.Controllers
{
    [Authorize]
    public class ContentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Contents
        public ActionResult Index()

        {
            //added the following line
            ContentIndexViewModel viewModel = new ContentIndexViewModel();

            var contents = db.Contents.Include(c => c.ContentType).Include(u => u.User);
            string userID = User.Identity.GetUserId();

            //added 4 lines below

            viewModel.Videos = db.Contents.Where(p => p.UserID == userID && p.ContentTypeID == 1).Include(p => p.ContentType).OrderBy(p => p.ContentCreatedDate).Take(3).ToList();
            viewModel.Recipes = db.Contents.Where(p => p.UserID == userID && p.ContentTypeID == 3).Include(p => p.ContentType).OrderBy(p => p.ContentCreatedDate).Take(3).ToList();
            viewModel.Articles = db.Contents.Where(p => p.UserID == userID && p.ContentTypeID == 2).Include(p => p.ContentType).OrderBy(p => p.ContentCreatedDate).Take(3).ToList();
            return View(viewModel);
        }


        // GET: Contents/Search?Term=searchterm
        //public ActionResult Search(SearchViewModel search)
        //{
        //string userID = User.Identity.GetUserId();

        //This tells MVC to return the index view but pass in data that only matches our search term.
        // return View("Index", db.Contents.Where(p => p.UserID == userID && (p.Description.Contains(search.Term) || string.IsNullOrEmpty(search.Term))).ToList());
        // }


        // GET: Contents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Include( c => c.ContentType ).Where( c => c.ContentID == id).FirstOrDefault();
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // GET: Contents/Create
        public ActionResult Create()
        {
            ViewBag.ContentTypeID = new SelectList(db.ContentTypes, "ContentTypeID", "ContentTypeDescription");
            return View();
        }

        // POST: Contents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContentID,ContentCreatedDate,UserID,ContentTypeID,Title,ContentBody,PreviewUrl,ThumbNailUrl")] Content content)
        {

            content.UserID = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                db.Contents.Add(content);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContentTypeID = new SelectList(db.ContentTypes, "ContentTypeID", "ContentTypeDescription", content.ContentTypeID);
            return View(content);
        }

        // GET: Contents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string userID = User.Identity.GetUserId();
            Content content = db.Contents.Where(p => p.UserID == userID && p.ContentID == id).FirstOrDefault();

            if (content == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContentTypeID = new SelectList(db.ContentTypes, "ContentTypeID", "ContentTypeDescription", content.ContentTypeID);
            return View(content);
        }

        // POST: Contents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContentID,ContentCreatedDate,UserID,ContentTypeID,Title,ContentBody,PreviewUrl,ThumbNailUrl")] Content content)
        {
            //Going to get the original and move all the properties over from the project that is passed in.
            string userID = User.Identity.GetUserId();
            Content originalContent = db.Contents.Where(p => p.UserID == userID && p.ContentID == content.ContentID).FirstOrDefault();

            if (originalContent == null)
            {
                return HttpNotFound();
            }

            //Move over all the properties that need to be set.
            originalContent.ContentTypeID = content.ContentTypeID;
            originalContent.ContentBody = content.ContentBody;
            originalContent.Title = content.Title;
            originalContent.PreviewUrl = content.PreviewUrl;
            originalContent.ThumbNailUrl = content.ThumbNailUrl;

            //content.UserID = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Entry(originalContent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContentTypeID = new SelectList(db.ContentTypes, "ContentTypeID", "ContentTypeDescription", content.ContentTypeID);
            return View(content);
        }

        // GET: Contents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string userID = User.Identity.GetUserId();
            Content content = db.Contents.Where(p => p.UserID == userID && p.ContentID == id).FirstOrDefault();

            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // POST: Contents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string userID = User.Identity.GetUserId();
            Content content = db.Contents.Where(p => p.UserID == userID && p.ContentID == id).FirstOrDefault();

            db.Contents.Remove(content);
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
