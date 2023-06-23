using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Display(Name = "Autor")]
        public string Author { get; set; }

        [Display(Name = "Dostępna")]
        public bool Available { get; set; }
        [Display(Name = "Kategoria")]
        public int CategoryId { get; set; } 
        public Category? Category { get; set; }        
        public ICollection<Borrow>? Borrows { get; set; }
    }
}