using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;

namespace Bulky.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context) : base(context) //when we get this imple , we want to pass it all the base class
                                                                            //mean whatever DBcontext we get, we'll pass it to Repository 
    {
        _context = context;
    }

    //below Save() is now moved to UnitOfWork
    //public void Save()
    //{
    //    _context.SaveChanges();
    //}

    public void Update(Product obj)
    {
      
        //_context.Products.Update(obj);
        
        //instead of directly updating the whole of object, we'll fetch the obj by id and then see if image needs to be updated or not
        var objFromDB = _context.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDB != null) {
                objFromDB.Title = obj.Title; 
                objFromDB.ISBN = obj.ISBN;
                objFromDB.Price = obj.Price;
                objFromDB.Price50 = obj.Price50;
                objFromDB.ListPrice = obj.ListPrice;
                objFromDB.Price100 = obj.Price100;
                objFromDB.Description = obj.Description;
                objFromDB.CategoryId = obj.CategoryId;
                objFromDB.Author = obj.Author;
                if(obj.ImageUrl != null)
                {
                    objFromDB.ImageUrl = obj.ImageUrl;
                }
            }
            _context.SaveChanges();
    }
}
}
