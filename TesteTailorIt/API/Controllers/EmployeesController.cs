using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using API.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Dapper;
using API.Map;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmployeesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/<EmployeesController>
        [HttpPost]
        [ActionName("GetDomains")]
        public Domains GetDomains([FromBody] IEnumerable<Employee> listEmployees)
        {
            List<string> listAges = new List<string>();
            List<DateTime> listDates = listEmployees.Select(x => x.BirthDate).ToList();
            foreach (var item in listDates)
            {
                TimeSpan ts = DateTime.Today.Subtract(item);
                int year = (DateTime.MinValue.AddDays(ts.Days).Year - 1);
                listAges.Add(year.ToString());
            }

            return new Domains()
            {
                Age = listAges.Distinct().ToList(),
                Gender = listEmployees.Select(x => x.Gender.ToString()).ToList()
            };
        }

        // GET: api/<EmployeesController>
        [HttpGet]
        [ActionName("GetAll")]
        public IEnumerable<Employee> GetAll()
        {
            IEnumerable<SearchEmployee> listEmployee = new List<SearchEmployee>();
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("testTailorItDB")))
            {
                connection.Open();

                listEmployee = connection.Query<SearchEmployee>("GetAllEmployees", commandType: CommandType.StoredProcedure);

            }
            return listEmployee.Select(x => new Employee()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                BirthDate = x.BirthDate,
                Age = x.Age,
                Email = x.Email,
                Gender = x.Gender,
                Habilities = new Hability()
                {
                    Id = x.IdHability,
                    Name = x.NameHability
                }
            }).ToList();
        }

        [HttpPost]
        [ActionName("FilterEmployees")]
        public IEnumerable<Employee> GetByFilter([FromBody] Filter filter)
        {
            IEnumerable<SearchEmployee> listEmployee = new List<SearchEmployee>();
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("testTailorItDB")))
            {
                connection.Open();

                listEmployee = connection.Query<SearchEmployee>("GetEmployeesByFilter", 
                    param: new {
                        age = filter.Age,
                        gender = filter.Gender
                    },
                    commandType: CommandType.StoredProcedure);

            }
            return listEmployee.Select(x => new Employee()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                BirthDate = x.BirthDate,
                Age = x.Age,
                Email = x.Email,
                Gender = x.Gender,
                Habilities = new Hability()
                {
                    Id = x.IdHability,
                    Name = x.NameHability
                }
            }).ToList();
        }

        // GET api/<EmployeesController>/5
        [HttpGet("{id}")]
        [ActionName("Get")]
        public Employee Get(int id)
        {
            SearchEmployee employee = new SearchEmployee();
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("testTailorItDB")))
            {
                connection.Open();

                employee = connection.QuerySingle<SearchEmployee>("GetEmployeesById", param: new { Id = id}, commandType: CommandType.StoredProcedure);

            }
            return new Employee()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                BirthDate = employee.BirthDate,
                Age = employee.Age,
                Email = employee.Email,
                Gender = employee.Gender,
                Habilities = new Hability()
                {
                    Id = employee.IdHability,
                    Name = employee.NameHability
                }
            };
        }

        // POST api/<EmployeesController>
        [HttpPost]
        [ActionName("Save")]
        public void SaveEmployee([FromBody] Employee employee)
        {
            //var a = 1;
            if (employee.Id == 0)
                InsertEmploye(employee);
            else
                UpdateEmploye(employee);
        }

        public void InsertEmploye(Employee employee)
        {
            TimeSpan ts = DateTime.Today.Subtract(employee.BirthDate);
            int age = DateTime.MinValue.AddDays(ts.Days).Year - 1;

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("testTailorItDB")))
            {
                connection.Open();

                DynamicParameters _params = new DynamicParameters(new
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    BirthDate = employee.BirthDate,
                    Age = age,
                    Email = employee.Email,
                    Gender = employee.Gender
                });

                int newId = connection.QuerySingle<int>("InsertEmployee", param: _params, commandType: CommandType.StoredProcedure);

                //foreach (var h in employee.Habilities)
                //{
                    connection.Execute("InsertEmployeeHabilities", param: new
                    {
                        IdEmployee = newId,
                        IdHability = employee.Habilities.Id
                    }, commandType: CommandType.StoredProcedure);
                //}                

            }
            
        }

        public void UpdateEmploye(Employee employee)
        {
            TimeSpan ts = DateTime.Today.Subtract(employee.BirthDate);
            int age = DateTime.MinValue.AddDays(ts.Days).Year - 1;

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("testTailorItDB")))
            {
                connection.Open();

                DynamicParameters _params = new DynamicParameters(new
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    BirthDate = employee.BirthDate,
                    Age = age,
                    Email = employee.Email,
                    Gender = employee.Gender,
                    IdHability = employee.Habilities.Id
                });

                connection.Execute("UpdateEmployee", param: _params, commandType: CommandType.StoredProcedure);               

            }

        }

        // DELETE api/<EmployeesController>/5
        [HttpDelete("{id}")]
        [ActionName("Delete")]
        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("testTailorItDB")))
            {
                connection.Open();

                connection.Execute("DeleteEmployee", param: new { Id = id }, commandType: CommandType.StoredProcedure);

            }
        }
    }
}
