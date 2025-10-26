using Microsoft.AspNetCore.Mvc;
using RareCrewCSharp.Models;
using RareCrewCSharp.Services;
using System.Diagnostics;

namespace RareCrewCSharp.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmployeeService _employeeService;
        private readonly PieChartGenerator _pieChartGenerator;

       public HomeController(EmployeeService employeeService, PieChartGenerator pieChartGenerator)
        {
            _employeeService = employeeService;
            _pieChartGenerator = pieChartGenerator;
        }   

        public async Task<IActionResult> Index()
        {
            List<Employee> employees= await _employeeService.GetEmployees();

            string chartPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/EmployeeChart.png");
            _pieChartGenerator.GenerateChart(employees, chartPath);
            return View(employees);
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
}