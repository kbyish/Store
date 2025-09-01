using CsvHelper.Configuration.Attributes;
using Microsoft.AspNetCore.Mvc;
using Store.Models;

namespace Store.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{


    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(ILogger<EmployeeController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetEmployee")]
    public Employee[] Get()
    {

        Employee[] employee =[
         new(){Id = 1, Name = "Kifah "},
         new(){Id = 2, Name = "Hussein "},
        ];


        return employee;

    }
}
