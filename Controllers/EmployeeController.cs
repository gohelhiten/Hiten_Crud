using Microsoft.AspNetCore.Mvc;
using MVCAPP.Models;
using MVCAPP.Repo;

namespace MVCAPP.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly DBRepo _dBRepo;

        public EmployeeController(ILogger<EmployeeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _dBRepo = new DBRepo(_configuration); // Use shared DBRepo instance
        }

        public IActionResult Index()
        {
            //var employees = _dBRepo.Employees();
            return View();
        }

        public IActionResult EmployeeIndex()
        {
            var employees = _dBRepo.Employees();
            return View(employees);
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var employees = _dBRepo.Employees();
            return Json(new { data = employees });
        }

        [HttpGet]
        public IActionResult Create() => View(new Employee());

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                bool success = employee.EmployeeID == 0 ?
                    _dBRepo.InsertEmployee(employee) :
                    _dBRepo.UpdateEmployee(employee) > 0;

                TempData[success ? "Success" : "Error"] = success ?
                    "Employee saved successfully!" :
                    "Failed to save employee.";

                return RedirectToAction("Index");
            }
            return View("Create", employee);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _dBRepo.GetEmployeeById(id);
            return employee == null ? NotFound() : View("Create", employee);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            bool success = _dBRepo.DeleteEmployee(id) > 0;
            TempData[success ? "Success" : "Error"] = success ?
                "Employee deleted successfully!" :
                "Failed to delete employee.";
            return RedirectToAction("EmployeeIndex");
        }
    }

    //public class EmployeeController : Controller
    //{
    //    private readonly ApplicationContext _dbcontext;
    //    public EmployeeController(ApplicationContext dbcontext)
    //    {
    //        _dbcontext = dbcontext;
    //    }
    //    public IActionResult Index()
    //    {
    //        return View();
    //    }

    //    [HttpGet]
    //    public IActionResult GetList()
    //    {
    //        var employees = _dbcontext.Employee.ToList();
    //        return Json(new { data = employees });
    //    }
    //}
    
}
