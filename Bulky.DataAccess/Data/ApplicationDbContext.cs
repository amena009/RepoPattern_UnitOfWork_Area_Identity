
using Bulky.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Bulky.DataAccess
{
    //basic config for setting up EntityFramework
    // public class ApplicationDbContext : DbContext

    //since now we'll use Indentity , so we'll write IdentityDbContext
    //when we add identity, it'll all user tables also required in db
    //we need to add migration , and all tables will be added to DB
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>    
    {
        //here we'll pass our ConnectionString to DbContext
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)  //here whatever config we'll do to options , options then will be passed on to base class of DbContext 
        {
            
        }

        //1.whenever we have to create a table, we have to create a DbSet
        //2.here Category is entity which is our class, we want category model table in out Db
        //3.Categories will be our actual table name in Db
        //4.the below single line will create the table in DB.
        //5.after writing below line,now we've to apply migration(by giving "add-migration Anymsg" in NugetManager Console)
        //6.running the step 5 command will only create the Db in VS, no migrations happened. means no such table created in Db
        //7.we'll run update-database command to actually make the migration happen
        //8.you can see the migration history in dbo_EFMigrationHistory
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        //now Category table is created in DB, we need to fill it with data.
        //so we'll use Seed functionality to fill our table with data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //keys of indentity table are mapped on model creating
            //and if this particular method is not called, we'll get an error msg
            base.OnModelCreating(modelBuilder);



         modelBuilder.Entity<Category>().HasData(
                    new Category { Id = 1, Name = "Action", DisplayOrder = 1},
                    new Category { Id = 2, Name = "Sci-Fi", DisplayOrder = 2},
                    new Category { Id = 3, Name = "History", DisplayOrder = 3},
                    new Category { Id = 4, Name = "Fiction", DisplayOrder = 4}
            );

            modelBuilder.Entity<Product>().HasData(
              new Product
              {
                  Id = 1,
                  Title = "Fortune of Time",
                  Author = "Billy Spark",
                  Description = "Billy Sparks book. it's nice.",
                  ISBN = "SWD9999001",
                  ListPrice = 99,
                  Price = 90,
                  Price50 = 85,
                  Price100 = 80,
                  CategoryId = 1,
                  ImageUrl = ""
              },
                   new Product
                   {
                       Id = 2,
                       Title = "Dark Skies",
                       Author = "Nancy Hoover",
                       Description = "Nancy Hoover's book. it's nice.",
                       ISBN = "CAW777777701",
                       ListPrice = 40,
                       Price = 30,
                       Price50 = 25,
                       Price100 = 20,
                       CategoryId = 6,
                       ImageUrl = ""
                   },
                   new Product
                   {
                       Id = 3,
                       Title = "Vanish in the Sunset",
                       Author = "Julian Button",
                       Description = "Julian Button's book. it's nice.",
                       ISBN = "RITO5555501",
                       ListPrice = 55,
                       Price = 50,
                       Price50 = 40,
                       Price100 = 35,
                       CategoryId = 2,
                       ImageUrl = ""
                   },
                   new Product
                   {
                       Id = 4,
                       Title = "Cotton Candy",
                       Author = "Abby Muscles",
                       Description = "Abby Muscles's book. It's good.",                       
                       ISBN = "WS3333333301",
                       ListPrice = 70,
                       Price = 65,
                       Price50 = 60,
                       Price100 = 55,
                       CategoryId = 4,
                       ImageUrl = ""
                   },
                   new Product
                   {
                       Id = 5,
                       Title = "Rock in the Ocean",
                       Author = "Ron Parker",
                       Description = "Ron Parker's book. It's nice.",
                       ISBN = "SOTJ1111111101",
                       ListPrice = 30,
                       Price = 27,
                       Price50 = 25,
                       Price100 = 20,
                       CategoryId = 2,
                       ImageUrl = ""
                   },
                   new Product
                   {
                       Id = 6,
                       Title = "Leaves and Wonders",
                       Author = "Laura Phantom",
                       Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                       ISBN = "FOT000000001",
                       ListPrice = 25,
                       Price = 23,
                       Price50 = 22,
                       Price100 = 20,
                       CategoryId = 1,
                       ImageUrl = ""
                   }
              );

        }


    }
}
