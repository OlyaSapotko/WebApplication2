using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Entity;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {

        public ApplicationUserManager UserManager
        {
            get
            {
                return UserManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {

            }
        }

        ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index()
        {
            IEnumerable<CollectionIt> collectionIts = db.CollectionIts;
            ViewBag.CollectionIts = collectionIts;
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult CreateCollection()
        {
            return View();
        }

        [HttpPost]       
        public async Task<ActionResult> CreateCollection(CreateCollectionViewModel model)
        {
            db.CollectionIts.Add(new CollectionIt()
            {
                name = model.Name,
                description = model.Description,
                //ApplicationUserId = "qwert"
            });
            db.SaveChanges();           
            return RedirectToAction("Collections");
        }

        [HttpGet]
        [Authorize]
        public ActionResult CreateItem(int id)
        {
            return View(new CreateItemViewModel() { collectionId = id});
        }

        public ActionResult ChangeAdmin(string userId)
        {
           var myid = User.Identity.GetUserId();
            if (!String.IsNullOrEmpty(userId))
            {
                 myid = userId;
            }

            var user = db.Users.First(x => x.Id == myid);
            if (user.IsAdmin)
                user.IsAdmin = false;
            else
                user.IsAdmin = true;

            db.SaveChanges();
            return View("Index", "Home");
        }

        public ActionResult Admin()
        {
            var myid = User.Identity.GetUserId();
            var user = db.Users.First(x => x.Id == myid);
            if (user.IsAdmin)
                return View(db.Users.ToList());
            else
                return View("Error");
        }

        [HttpPost]
        public async Task<ActionResult> CreateItem(CreateItemViewModel model)
        {
            var item = new Item()
            {
                name = model.Name,
                description = model.Description,
                collectionItId = model.collectionId,
                CollectionIt = db.CollectionIts.FirstOrDefault(x => x.id == model.collectionId)
            };
            db.Items.Add(item);

            db.CollectionIts.FirstOrDefault(x => x.id == model.collectionId).Items.Add(item);

            db.SaveChanges();
            return RedirectToAction($"Collection/{model.collectionId}");
        }

        [HttpPost]
        [Authorize]
        public ActionResult LikeItem(int itemId)
        {
            var myId = User.Identity.GetUserId();
            if (IsUserLike(itemId, myId))
                RemoveLike(itemId, myId);
            else
                AddLike(itemId, myId);

            return Redirect($"Item/{itemId}");
        }

        private bool IsUserLike(int itemId, string userId)
           => db.Items.First(x => x.id == itemId).LikeItems.Count(x => x.ApplicationUserId == userId) > 0;

        public void AddLike(int itemId, string userId)
        {
            db.Items.First(x => x.id == itemId).LikeItems.Add(new LikeItem()
            {
                ItemId = itemId,
                ApplicationUserId = userId
            });
            db.SaveChanges();
        }

        public void RemoveLike(int itemId, string userId)
        {
            db.likeItems.Remove(db.likeItems.First(x => x.ItemId == itemId && x.ApplicationUserId == userId));
            //db.Items.First(x => x.id == itemId)
            //    .LikeItems
            //    .Remove(db.likeItems.First(x => x.ItemId == itemId 
            //&& x.ApplicationUserId == userId));
            db.SaveChanges();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Comment(int itemId, string text)
        {
            var myId = User.Identity.GetUserId();
            AddComment(itemId, myId, text);
            return Redirect($"Item/{itemId}");
        }

        public void AddComment(int itemId, string userId, string text)
        {
            db.Items.First(x => x.id == itemId).Comments.Add(new Comment() { ApplicationUserId = userId, text = text });
            db.SaveChanges();
          
        }

        public ActionResult Collection(int id)
        {
            var collection = db.CollectionIts.Find(id);
            collection.description = Markdig.Markdown.ToHtml(collection.description);
            return View(collection);
        }

        public ActionResult Item(int id)
        {
            var item = db.Items.Find(id);
            return View(db.Items.Find(id));
        }


        public ActionResult Collections()
        {
            var collections = db.CollectionIts.ToList();
            List<CreateCollectionViewModel> outputList = new List<CreateCollectionViewModel>();

            //collections.ForEach(x => outputList.Add(new CreateCollectionViewModel() { Name = x.name, Description = Markdig.Markdown.ToHtml(x.description) }));
            collections.ForEach(x =>x.description = Markdig.Markdown.ToHtml(x.description));

            //foreach (var item in collections)
            //{
            //    outputList.Add(new CreateCollectionViewModel()
            //    {
            //        Description = item.description,
            //        Name = item.name
            //    });
            //}

            return View(collections);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            db.CollectionIts.Remove(db.CollectionIts.FirstOrDefault(x => x.id == id));
            db.SaveChanges();
            return RedirectToAction("Collections");
        }
        [HttpPost]
        public ActionResult DeleteItem(int id, int collectionId)
        {
            Item i = new Item { id = id };
            db.Entry(i).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction($"Collection/{collectionId}");
        }
    }
}