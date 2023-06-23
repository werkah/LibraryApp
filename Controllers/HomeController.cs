using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Library.Models;
using Library.Data;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Library.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly LibraryContext _context;

    public HomeController(ILogger<HomeController> logger, LibraryContext context)
    {
        _logger = logger;
        _context = context;
    }
private string HashString(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
public IActionResult Index()
{
    string isLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
    bool isAdminCreated = _context.User.Any(u => u.Username == "admin");

    if (!isAdminCreated) //tworzenie admina
    {
        User adminUser = new User
        {
            Username = "admin",
            Password = HashString("admin")
        };

        _context.User.Add(adminUser);
        _context.SaveChanges();
        TempData["Message"] = "Admin user created. Please log in with the username 'admin' and password 'admin'.";
    }

    if (isLoggedIn == "true")
    {
        bool isAdmin = HttpContext.Session.GetString("Username") == "admin";

        if (isAdmin)
        {
            TempData["Message"] = "Admin logged in";
            return RedirectToAction("AdminLoggedIn");
        }
        else
        {
            return RedirectToAction("LoggedIn");
        }
    }
    else
    {
        ViewData["IsLoggedIn"] = false;
        return View();
    }
}
    public IActionResult AdminLoggedIn()
    {
            return View();
    }

    public IActionResult LoggedIn()
    {
            return View();
    }

    public IActionResult Login()
{
    bool isLoggedIn = HttpContext.Session.GetString("IsLoggedIn") == "true";
    bool isAdmin = HttpContext.Session.GetString("Username") == "admin"; 

    if (!isLoggedIn) 
    {
        return RedirectToAction("Login", "Account");
    }
    else if (isAdmin)
    {
        TempData["Message"] = "Admin is already logged in.";
        return RedirectToAction("AdminLoggedIn", "Home");
    }
    else
    {
        TempData["Message"] = "User is already logged in.";
        return RedirectToAction("LoggedIn", "Home");
    }
}

    

    public IActionResult Logout()
    {
        HttpContext.Session.SetString("IsLoggedIn", "false");
        HttpContext.Session.SetString("UserId", "");
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
