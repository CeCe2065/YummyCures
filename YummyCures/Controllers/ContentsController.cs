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

            viewModel.Videos = db.Contents.Where(p => p.UserID == userID && p.ContentTypeID == 1).Include(p => p.ContentType).OrderBy(p => p.ContentCreatedDate).Take(4).ToList();
            viewModel.Recipes = db.Contents.Where(p => p.UserID == userID && p.ContentTypeID == 3).Include(p => p.ContentType).OrderBy(p => p.ContentCreatedDate).Take(4).ToList();
            viewModel.Articles = db.Contents.Where(p => p.UserID == userID && p.ContentTypeID == 2).Include(p => p.ContentType).OrderBy(p => p.ContentCreatedDate).Take(4).ToList();
            return View(viewModel);
        }
        // GET: Contents/Search?Term=searchterm
        public ActionResult Search(SearchViewModel search)
        {
            ContentIndexViewModel viewModel = new ContentIndexViewModel();

            var contents = db.Contents.Include(c => c.ContentType).Include(u => u.User);
            string userID = User.Identity.GetUserId();

            //This tells MVC to return the search view but pass in data that only matches our search term.
            viewModel.Videos = db.Contents.Where(p => p.UserID == userID && p.ContentTypeID == 1 && p.ContentBody.Contains(search.Term)).Include(p => p.ContentType).OrderBy(p => p.ContentCreatedDate).Take(4).ToList();
            viewModel.Recipes = db.Contents.Where(p => p.UserID == userID && p.ContentTypeID == 3 && p.ContentBody.Contains(search.Term)).Include(p => p.ContentType).OrderBy(p => p.ContentCreatedDate).Take(4).ToList();
            viewModel.Articles = db.Contents.Where(p => p.UserID == userID && p.ContentTypeID == 2 && p.ContentBody.Contains(search.Term)).Include(p => p.ContentType).OrderBy(p => p.ContentCreatedDate).Take(4).ToList();
            return View(viewModel);
        }


        // GET: Contents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Include(c => c.ContentType).Where(c => c.ContentID == id).FirstOrDefault();
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
            Content content = db.Contents.Include(p => p.Tags).Where(p => p.UserID == userID && p.ContentID == id).FirstOrDefault();

            if (content == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContentTypeID = new SelectList(db.ContentTypes, "ContentTypeID", "ContentTypeDescription", content.ContentTypeID);


            EditContentViewModel viewModel = new EditContentViewModel();
            viewModel.Content = content;
            viewModel.AllTags = (from t in db.Tags
                                 select new SelectListItem()
                                 {
                                     Value = t.TagID.ToString(),
                                     Text = t.Description
                                 }).ToList();

            return View(viewModel);

        }

        // POST: Contents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditContentViewModel model) //Originally this prarmeter was named project. But project conflicted with the project property on the view model so I had to change it. WOW!
        {
            //Going to get the original and move all the properties over from the project that is passed in.
            string userID = User.Identity.GetUserId();
            Content originalContent = db.Contents.Include(p => p.Tags).Where(p => p.UserID == userID && p.ContentID == model.Content.ContentID).FirstOrDefault();
            if (originalContent == null)
            {
                return HttpNotFound();
            }

            //Move over all the properties that need to be set.
            originalContent.ContentTypeID = model.Content.ContentTypeID;
            originalContent.ContentBody = model.Content.ContentBody;
            originalContent.Title = model.Content.Title;
            originalContent.PreviewUrl = model.Content.PreviewUrl;
            originalContent.ThumbNailUrl = model.Content.ThumbNailUrl;

            originalContent.Description = model.Content.Description;

            //Add the tags that were selected.
            foreach (int TagID in model.SelectedTags)
            {
                if (!originalContent.Tags.Any(p => p.TagID == TagID))
                {
                    var tag = db.Tags.Find(TagID);
                    originalContent.Tags.Add(tag);
                }
            }

            //remove tags that were unchecked
            var deletedTags = from t in originalContent.Tags
                              where !model.SelectedTags.Contains(t.TagID)
                              select t;
            originalContent.Tags.RemoveAll(t => deletedTags.Contains(t));

            if (ModelState.IsValid)
            {
                db.Entry(originalContent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContentTypeID = new SelectList(db.ContentTypes, "ContentTypeID", "ContentTypeDescription", model.Content.ContentTypeID);


            model.AllTags = (from t in db.Tags
                             select new SelectListItem()
                             {
                                 Value = t.TagID.ToString(),
                                 Text = t.Description
                             }).ToList();

            return View(model);



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
