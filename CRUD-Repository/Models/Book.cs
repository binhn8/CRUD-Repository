using System;
using System.ComponentModel.DataAnnotations;

namespace CRUD_Repository.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Title")]
        public string Title { get; set; }
        public DateTime Published { get; set; }
        public Author? Author { get; set; }
        public Category? Category { get; set; }
       
        [Display(Name = "Author")]
        public int AuthorId { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
    }
}
