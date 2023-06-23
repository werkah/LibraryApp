using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Library.Models;

namespace Library.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext (DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        public DbSet<Library.Models.User> User { get; set; } = default!;
        public DbSet<Library.Models.Book> Book { get; set; } = default!;
        public DbSet<Library.Models.Borrow> Borrow { get; set; } = default!;
        public DbSet<Library.Models.Category> Category { get; set; } = default!;


    }
}
