using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using API.Map;
using API.Model;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HabilitiesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public HabilitiesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/<HabilitiesController>
        [HttpGet]
        [ActionName("GetAll")]
        public IEnumerable<Hability> GetAll()
        {
            IEnumerable<SearchHability> listHability = new List<SearchHability>();
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("testTailorItDB")))
            {
                connection.Open();

                listHability = connection.Query<SearchHability>("GetAllHabilities", commandType: CommandType.StoredProcedure);

            }

            return listHability.Select( x => new Hability()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }
    }
}
