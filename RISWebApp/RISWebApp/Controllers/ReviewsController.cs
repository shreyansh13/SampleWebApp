using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RISWebApp.Models;

namespace RISWebApp.Controllers
{
    public class ReviewsController : Controller
    {
        private string NormalizeString (string aString)
        {
            string output = aString.Replace(" ", "");
            return output;
        }

        private ProductDatabaseEntities db = new ProductDatabaseEntities();        
        private void GetListofCategories()
        {            
            var query = db.ProductTables.ToList();
            HashSet<string> Categories = new HashSet<string>();
            Hashtable CategoryHash = new Hashtable();
            foreach (ProductTable product in query)
            {
                string catString = product.Categories;
                string[] splits = catString.Split(',');
                foreach(string cat in splits)
                {
                    string normalizedCat = NormalizeString(cat);
                    if (CategoryHash.Contains(normalizedCat))
                    {
                        CategoryProductModel catProducts = (CategoryProductModel)CategoryHash[normalizedCat];
                        catProducts.mCategoryName = normalizedCat;
                        catProducts.mProduct.Add(product);
                        CategoryHash[normalizedCat] = catProducts;
                    }
                    else
                    {
                        CategoryProductModel catProducts = new CategoryProductModel();
                        catProducts.mCategoryName = normalizedCat;
                        catProducts.mProduct = new List<ProductTable>();
                        catProducts.mProduct.Add(product);
                        CategoryHash.Add(normalizedCat, catProducts);
                    }                    
                }
            }
            ViewBag.ProductCategories = CategoryHash.Values.Cast<CategoryProductModel>().ToList();
        }

        // GET: Reviews        
        public ActionResult Index()
        {
            var query = db.ProductTables.Where(x => x.IsTagged != null && x.IsTagged == "1").OrderByDescending(x => x.Id);
            GetListofCategories();   
            return View(query.ToList());
        }

        public ActionResult SeeAllData()
        {
            GetListofCategories();
            return View(db.ProductTables.ToList());
        }

        // GET: Reviews/Details/5
        public ActionResult Product(int? id)
        {
            GetListofCategories();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductTable productTable = db.ProductTables.Find(id);
            if (productTable == null)
            {
                return HttpNotFound();
            }
            return View(productTable);
        }

        public ActionResult ProductReviewByCategories(string aCategory)
        {
            GetListofCategories();
            ViewBag.Title = aCategory;
            if (aCategory == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            
            var query = db.ProductTables.Where(x => x.Categories != null && x.Categories.Contains(aCategory) == true).OrderByDescending(x => x.Id);

            if (query == null)
            {
                return HttpNotFound();
            }
            return View(query.ToList());
        }

        // GET: Reviews/Create
        [ValidateInput(false)]
        public ActionResult Create()
        {
            GetListofCategories();
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,UrlTitle,Title,Categories,Tags,Html,IsTagged")] ProductTable productTable)
        {
            GetListofCategories();
            if (ModelState.IsValid)
            {
                db.ProductTables.Add(productTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productTable);
        }

        // GET: Reviews/Edit/5
        [ValidateInput(false)]
        public ActionResult Edit(int? id)
        {
            GetListofCategories();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductTable productTable = db.ProductTables.Find(id);
            if (productTable == null)
            {
                return HttpNotFound();
            }            
            return View(productTable);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,UrlTitle,Title,Categories,Tags,Html,IsTagged")] ProductTable productTable)
        {
            GetListofCategories();            
            if (ModelState.IsValid)
            {
                // remove the current product    
                ProductTable oldProductTable = db.ProductTables.Find(productTable.Id);
                db.ProductTables.Remove(oldProductTable);
                db.SaveChanges();
                db.ProductTables.Add(productTable);
                db.SaveChanges();
                
                //db.Entry(productTable).State = EntityState.Modified;                
                
                return RedirectToAction("SeeAllData");                
            }
            return View(productTable);            
        }

        // GET: Reviews/Delete/5
        [ValidateInput(false)]
        public ActionResult Delete(int? id)
        {
            GetListofCategories();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductTable productTable = db.ProductTables.Find(id);
            if (productTable == null)
            {
                return HttpNotFound();
            }
            return View(productTable);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult DeleteConfirmed(int id)
        {
            GetListofCategories();
            ProductTable productTable = db.ProductTables.Find(id);
            db.ProductTables.Remove(productTable);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [ValidateInput(false)]
        protected override void Dispose(bool disposing)
        {
            GetListofCategories();
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
