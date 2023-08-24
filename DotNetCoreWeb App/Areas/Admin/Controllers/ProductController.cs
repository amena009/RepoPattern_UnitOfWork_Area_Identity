using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyMVCWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
         
        //now whatever ProductRepository we have created is already doing ApplicationDbContext work
        //so insted of using ApplicationDbContext, we can directly use our repository IProductRepository

        //private readonly ApplicationDbContext _db;
        //private readonly IProductRepository _db;
        private readonly IUnitOfWork _db;  //coz now instead of IProductRepo, we are using UnitOfWork

        private readonly IWebHostEnvironment _webHostEnvironment;  //to access the root folder for storing our files in images folder in root folder

        //public ProductController(ApplicationDbContext db)
        //public ProductController(IProductRepository db)
        public ProductController(IUnitOfWork db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;  
        }

        public IActionResult Index()
        {
            //List<Product> allCategories = _db.GetAll().ToList(); //using IProductRepo
            List<Product> allCategories = _db.Product.GetAll(includeProperties:"Category").ToList(); //in UnitOfWork we have to mention which registered repo we are using
           
            return View(allCategories);
        }

        //public IActionResult Create()
        public IActionResult Upsert(int? id)   //update+insert means same page for update and create

        {
            //this is projection inEF
            //means how we are creating our model for dropdown
            IEnumerable<SelectListItem> categoryDropdownList = _db.Category.GetAll().Select(u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() });
            //ViewBag.categoryDropdownList = categoryDropdownList;
            ProductVM productVm = new()
            {
                dropdownList = categoryDropdownList,
                Product = new Product()
            };
            if(id ==null || id == 0)
            {
                //create
                return View(productVm);
            } else
            {
                //update
                productVm.Product = _db.Product.Get(u => u.Id == id);
                return View(productVm); 
            }
        }

        //public IActionResult Create(Product createObj)
        //{

        //bec now instead of model, we'll use ViewModel
        // public IActionResult Create(ProductVM createObj)
        [HttpPost]
        public IActionResult Upsert(ProductVM createObj, IFormFile? file)   //update+insert means same page for update and create
        {

            //custom validation-for ex, name and display order can't be same
            //this is server side validation
            //if (createObj.Title == createObj.Title.ToString())
            //{
            //    //here adding our custom error to the model, here name is the form-control key
            //    ModelState.AddModelError("name", "Display order and Name cannot be same");
            //}

            //here we are checking another validation,but giving any form-control
            //so if this error occurs, it'll not displayed under any form field, but will display under ErrorSummary

            //if (createObj.Name != null && createObj.Name.ToLower() == "test")
            //{
            //    //here adding our custom error to the model, here name is the form-control key
            //    ModelState.AddModelError("", "Display order and Name cannot be same");
            //}

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); //gives random file name along with original file extension
                    string productPath = Path.Combine(wwwRootPath, @"images\Product"); //to create path to reach upto product folder inside images
                    
                    //we'll check if image already there, means update, other wise store the new image
                    if(!string.IsNullOrEmpty(createObj.Product.ImageUrl)) //we'll check if image url is not empty, means some image already there,so image will replace old image
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, createObj.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath)) {
                            System.IO.File.Delete(oldImagePath); //to delete the old image
                        }
                    }

                    //below storing code will be same for both new and update image
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    createObj.Product.ImageUrl = @"\images\Product\" + fileName;
                }


                

                //_db.Categories.Add(createObj);
                //_db.Add(createObj);
               // _db.Product.Add(createObj.Product);

                //we'll check if it's create or update request by checking if id already exists
                if(createObj.Product.Id == 0)
                {
                     _db.Product.Add(createObj.Product);
                }
                else
                {
                     _db.Product.Update(createObj.Product);
                }

                //_db.SaveChanges();
                _db.Save();  //we'll not mention Product here coz save is in UnitOfWork, not in ProductRepo

                //temp data value is only available for the next render.
                //on page refresh the value vanishes.
                //mainly used to show notification.
                TempData["success"] = "Product created successfully.";
                return RedirectToAction("Index");

            } else
            {
                createObj.dropdownList = _db.Category.GetAll().Select(u => new SelectListItem { 
                    Text = u.Name, Value = u.Id.ToString() 
                });
                return View(createObj);

            }
        }


        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    //Find works on primary key
        //    //Product ProductById = _db.Categories.Find(id);

        //    //it work even on non-primary fields also, like name etc
        //    //it'll return the record if exist, or return the null value
        //    // Product? ProductById = _db.Categories.FirstOrDefault(a => a.Id == id);
        //    // Product? ProductById = _db.Get(a => a.Id == id);
        //    Product? ProductById = _db.Product.Get(a => a.Id == id); //UnitOfWork


        //    //similar to FirstOrDefault but once you get data you can apply more functionalities
        //    // Product? ProductById = _db.Categories.Where(a => a.Id == id).FirstOrDefault();
        //    if (ProductById == null)
        //    {
        //        return NotFound();
        //    }


        //    return View(ProductById);
        //}
        //[HttpPost]
        //public IActionResult Edit(Product createObj)
        //{
        //    //since we don't have any ID form-control,it'll create a new record on top of the existing record
        //    //to update existing record,we should pass id properly(such as id as hidden field)
        //    if (ModelState.IsValid)
        //    {
        //        // _db.Categories.Update(createObj);
        //        //_db.SaveChanges();

        //        // _db.Update(createObj);
        //        _db.Product.Update(createObj); //UnitOfWork

        //        _db.Save();
        //        TempData["success"] = "Product edited successfully.";

        //        return RedirectToAction("Index");

        //    }
        //    return View();
        //}

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //Find works on primary key
            //Product ProductById = _db.Categories.Find(id);

            //it work even on non-primary fields also, like name etc
            //it'll return the record if exist, or return the null value
            //Product? ProductById = _db.Get(a => a.Id == id);
            Product? ProductById = _db.Product.Get(a => a.Id == id); //UnitOfWork


            //similar to FirstOrDefault but once you get data you can apply more functionalities
            // Product? ProductById = _db.Categories.Where(a => a.Id == id).FirstOrDefault();
            if (ProductById == null)
            {
                return NotFound();
            }

            return View(ProductById);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            // Product? obj = _db.Get(u =>  u.Id == id);
            Product? obj = _db.Product.Get(u => u.Id == id); //UnitOfWork

            if (obj == null)
            {
                return NotFound();
            }

            if (obj == null)
            {
                return NotFound();
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath); //to delete the old image
            }

         

            //_db.Remove(obj);
            _db.Product.Remove(obj); //UnitOfWork

            _db.Save();
            TempData["success"] = "Product deleted successfully.";

            return RedirectToAction("Index");
        }


    }
}
