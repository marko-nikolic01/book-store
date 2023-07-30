using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        [Range(1, 1000, ErrorMessage = "List Price must be betwen 1-1000")]
        [Display(Name = "List Price")]
        public double ListPrice { get; set; }
        [Required]
        [Range(1, 1000, ErrorMessage = "Price for 1-49 must be betwen 1-1000")]
        [Display(Name = "Price for 1-49")]
        public double Price { get; set; }
        [Required]
        [Range(1, 1000, ErrorMessage = "Price for 50-99 must be betwen 1-1000")]
        [Display(Name = "Price for 50-99")]
        public double Price50 { get; set; }
        [Required]
        [Range(1, 1000, ErrorMessage = "Price for 100+ must be betwen 1-1000")]
        [Display(Name = "Price for 100+")]
        public double Price100 { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public string ImageUrl { get; set; }
    }
}
