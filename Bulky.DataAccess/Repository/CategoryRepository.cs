﻿using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;


namespace Bulky.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context) //when we get this imple , we want to pass it all the base class
        //mean whatever DBcontext we get, we'll pass it to Repository 
        {
            _context = context;
        }

        //below Save() is now moved to UnitOfWork
        //public void Save()
        //{
        //    _context.SaveChanges();
        //}

        public void Update(Category obj)
        {
            _context.Categories.Update(obj);
            _context.SaveChanges();
        }
    }
}
