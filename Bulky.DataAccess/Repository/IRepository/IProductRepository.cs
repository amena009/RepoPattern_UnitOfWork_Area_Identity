using Bulky.Models.Models;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>//so now Category will access base func defined in IRepo
    {
        //along with base functionalities in IRepo, we'll have 2 more methods defined below
        void Update(Product obj);
        // void Save();
        //above line now moved to UnitOfWork
    }
}
