﻿

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulky.Models.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]   
        public string Title { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name="List Price")]
        [Range(1,1000)] //price must be b/w 1-1000 dollars
        public double ListPrice { get; set; }


        [Required]
        [Display(Name = "Price for 1-50 books")]
        [Range(1, 1000)]  
        public double Price{ get; set; }


        [Required]
        [Display(Name = "Price for 50+ books")]
        [Range(1, 1000)] 
        public double Price50 { get; set; }


        [Required]
        [Display(Name = "Price for 100+ books")]
        [Range(1, 1000)]
        public double Price100 { get; set; }

        [ValidateNever]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }    

    }
}
