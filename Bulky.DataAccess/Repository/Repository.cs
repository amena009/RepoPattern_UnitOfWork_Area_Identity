using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bulky.DataAccess.Repository
{
    //here we are making our class Repository generic by adding <T>
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet; // here we are creating generic DbSet(not just for Categories)
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>(); //here we are setting the current dbSet of type T(generic)
            //means _db.Categories is equal to dbSet
            //means instead of _db.Categpried.Add(), we can use _db.Add()


            //since our category was coming is null, but due to Foriegn key relation categoryId was there, so here 
            //we'll inclue category also
            //so include will populate the navigation properties based on the foriegn key relation
            _context.Products.Include(u => u.Category).Include(u => u.CategoryId);
        }
        public void Add(T entity)
        {
            _context.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            // Possible null reference return.
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault();
            //above statements is equal to dbSet.Categories.Where(u => u.id == id).FirstOrDefault();
        }

        //ex:- inlcludedProperties will be like this Category, CoverType
        //in getAll also we want to include the Category
        public IEnumerable<T> GetAll(string? includeProperties = null )
        {
            IQueryable<T> query = dbSet;
            if(!string.IsNullOrEmpty( includeProperties ))
            {
                foreach(var includeProp in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries) ) 
                { 
                  query = query.Include(includeProp); 
                }
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
           dbSet.Remove(entity);    
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
          dbSet.RemoveRange(entities);
        }
    }
}
