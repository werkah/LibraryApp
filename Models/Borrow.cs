using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Borrow
    {
        [Key]
        public int BorrowId { get; set; }

        [Display(Name = "Data wypożyczenia")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BorrowDate { get; set; }

        [Display(Name = "Data zwrotu")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BorrowDue { get; set; }

        [Display(Name = "Czy zwrócone")]
        public bool IsReturned { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public int UserId { get; set; }

        public User? User { get; set; }
    }
}
