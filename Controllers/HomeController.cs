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
        //   CollectionItContext db = new CollectionItContext();
        public ActionResult Index()
        {
            IEnumerable<CollectionIt> collectionIts = db.CollectionIts;
            ViewBag.CollectionIts = collectionIts;
            return View();
        }

        [HttpGet]
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
                ApplicationUserId = "eqweqfwerqwe"
            });

            db.SaveChanges();

            //return View();
            return RedirectToAction("Collections");
        }

        [HttpGet]
        public ActionResult CreateItem(int id)
        {
            return View(new CreateItemViewModel() { collectionId = id});
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

        

        public ActionResult Collection(int id)
        {
            return View(db.CollectionIts.Find(id));
        }

        public ActionResult Collections()
        {
            var collections = db.CollectionIts.ToList();
            List<CreateCollectionViewModel> outputList = new List<CreateCollectionViewModel>();

            collections.ForEach(x => outputList.Add(new CreateCollectionViewModel() { Name = x.name, Description = x.description }));


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


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            db.CollectionIts.Remove(db.CollectionIts.FirstOrDefault(x => x.id == id));
            db.SaveChanges();
            return RedirectToAction("Collections");
        }
        [HttpPost]
        public ActionResult DeleteItem(int id)
        {
            Item i = new Item { id = id };
            db.Entry(i).State = System.Data.Entity.EntityState.Deleted;
            //db.Items.Remove(db.Items.FirstOrDefault(y => y.id == id));
            db.SaveChanges();
            return RedirectToAction("Collection");
        }
    }
}