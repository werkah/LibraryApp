using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name {get; set;}
        public ICollection<Book>? Books { get; set;} 
    }
}