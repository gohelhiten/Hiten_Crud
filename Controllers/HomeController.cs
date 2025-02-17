using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVCAPP.Models;
using MVCAPP.Repo;

namespace MVCAPP.Controllers
{
    //[ApiController]
   // [Route("Home")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            //DBRepo dBRepo = new DBRepo(_configuration);
            //var empLst = dBRepo.Employees();

            TempData["Mykey"] = "Learning Temp Data";
            var emp = new Employee
            {
                FirstName = "A",
                LastName = "B",
                Email = "abc@abc.com",
                PhoneNumber = "011121212",
                HireDate = DateOnly.FromDateTime(Convert.ToDateTime("11-10-2024")),
                JobTitle = "Devloper",
                Department = "IT",
                Salary = 10000
            };

            return View(emp);
        }

        public IActionResult Privacy()
        {
            ViewData["MyKey"] = "Let me Show View Data";
            return View();
        }

        [Route("Home/About")]
        [Route("Home/About/{id?}")]
        public IActionResult AboutUs(int id)
        {
            ViewBag.Readtempdata = id; //TempData.Peek("Mykey");

            return View();
        }
        //Route()
        public IActionResult tools()
        {
            DBRepo dBRepo = new DBRepo(_configuration);
            var empLst = dBRepo.Employees();

            return View(empLst);
        }

        public IActionResult Create()
        {
            return RedirectToAction("EmployeeIndex", "Employee");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
