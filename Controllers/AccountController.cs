using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;
using System.Security.Cryptography;
using System.Text;

namespace Library.Controllers
{
    public class AccountController : Controller
    {
        private readonly LibraryContext _context;

        public AccountController(LibraryContext context)
        {
            _context = context;
        }
        private string HashString(string input) //hashowanie sha 256
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input); //tworzenie tab bajtow
                byte[] hashBytes = sha256.ComputeHash(inputBytes); //obliczenie hash
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes) 
                {
                    sb.Append(b.ToString("x2")); //na szesnastkowy
                }
                return sb.ToString(); //zwrot skrotu
            }
        }

        private bool VerifyHash(string input, string hashedInput)
        {
            string hashedInputToVerify = HashString(input); //obliczenie skrotu
            return StringComparer.OrdinalIgnoreCase.Compare(hashedInputToVerify, hashedInput) == 0; //porownanie z danym
        }

        // GET: Account
        public async Task<IActionResult> Index()
        {
              return _context.User != null ? 
                          View(await _context.User.ToListAsync()) :
                          Problem("Entity set 'LibraryContext.User'  is null.");
        }

        // GET: Account/ShowBorrows
        public async Task<IActionResult> ShowBorrows()
        {
              return View();
        }

        // GET: Account/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            var userBorrows = await _context.Borrow.Include(o => o.User).Where(o => o.User.Username.Contains(SearchPhrase)).ToListAsync();
            return View(userBorrows);
        }

        // GET: Account/ShowSearchBooksResults
        public async Task<IActionResult> ShowSearchBooksResults()
        {
            var books = await _context.Book.Include(b => b.Category).ToListAsync(); //pobranie ksiazek z kategoriami
            var categoriesWithBooks = books.GroupBy(b => b.CategoryId) //grupowanie po kategoriach
                                   .Select(g => new { CategoryId = g.Key, BookCount = g.Count() }) //grupa z cat i liczba
                                   .OrderByDescending(c => c.BookCount) //malejaco
                                   .ToList();

            var mostPopularCategory = categoriesWithBooks.FirstOrDefault(); //pierwsza

            ViewBag.MostPopularCategory = mostPopularCategory;

            return View(books);
        }

        // GET: Account/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        // GET: Login/Create
        public IActionResult Create()
        {
            return View();
        }

// POST: Account/Create
// To protect from overposting attacks, enable the specific properties you want to bind to.
// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("Username,Password")] User user)
{
    bool isAdmin = HttpContext.Session.GetString("UserId") == HttpContext.Session.GetString("Admin");

    if (isAdmin)
    {
        if (ModelState.IsValid)
        {
            User existingUser = _context.User.FirstOrDefault(u => u.Username == user.Username);
            if (existingUser != null)
            {
                TempData["Message"] = "User already exists!";
                return View(user);
            }

            user.Password = HashString(user.Password);
            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
    else
    {
        TempData["Message"] = "You don't have permission to add new users."; //jak nie admin to nie dodaje
        return RedirectToAction("Index", "Home");
    }

    return View(user);
}

// GET: Account/Login
public IActionResult Login()
{
    return View();
}

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
public IActionResult Login([Bind("Username, Password")] User user)
{
    if (ModelState.IsValid)
    {
        User existingUser = _context.User.FirstOrDefault(u => u.Username == user.Username);
        if (existingUser == null) //jak nie istnieje
        {
            TempData["Message"] = "User not found!";
            return View(user);
        }

        if (VerifyHash(user.Password, existingUser.Password)) //jak poprawne haslo
        {
            HttpContext.Session.SetString("IsLoggedIn", "true");
            HttpContext.Session.SetString("Username", existingUser.Username);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            TempData["Message"] = "Invalid password!";
            return View(user);
        }
    }

    return View(user);
}

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Password")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'LibraryContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.User?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
