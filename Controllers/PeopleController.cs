using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace WebApi_Core.Controllers
{
    //people
    [ApiController]
    [Route("[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly ILogger<PeopleController> _logger;
        private IList<People> _peoples = new List<People>()
        {
            new People() { Fam = "petrov", Im = "petr", Ot = "Petrovich", Age = 12, Id = 1 },
            new People() { Fam = "petrov", Im = "petr2", Ot = "Petrovich", Age = 12, Id = 2 },
            new People() { Fam = "semenov", Im = "semen", Ot = "Petrovich", Age = 12, Id = 3 },
            new People() { Fam = "ivanov", Im = "ivan", Ot = "Petrovich", Age = 12, Id = 4 },
            new People() { Fam = "sidorov", Im = "isidr", Ot = "Petrovich", Age = 18, Id = 5 },
            new People() { Fam = "makarevich", Im = "makar", Ot = "Petrovich", Age = 20, Id = 6 }
        };

        public PeopleController(ILogger<PeopleController> logger)
        {
            _logger = logger;
        }

        //people
        [HttpGet]
        public ActionResult<IEnumerable<People>> Get()
        {
            return _peoples.AsEnumerable<People>().ToList<People>();
        }

        //people/20
        [HttpGet("{id:int}")]
        /*
         * [HttpGet]
         * [Route("{id:int}")]
         * 
         */
        public ActionResult<People> GetById(int id)
        {
            var p = _peoples.Where(p => p.Id == id).FirstOrDefault();
            if (p == null)
                return NotFound();
            return p;
        }

        //people/petrov
        [HttpGet]
        [Route("{fam}")]
        public ActionResult<IEnumerable<People>> GetByFam(string fam)
        {
            var p = _peoples.Where(p => p.Fam == fam).AsEnumerable<People>().ToList<People>();
            if (p.Count == 0)
                return NotFound();
            return p;
        }

        //people/notpetrov
        //people/notpetrov?age=20
        [HttpGet("notpetrov")]
        public ActionResult<IEnumerable<People>> GetList(int? age)
        {
            var p = _peoples.Where(p => p.Fam != "petrov" && (age != null ? p.Age == age : p.Age > 0)).AsEnumerable<People>().ToList<People>();
            if (p.Count == 0)
                return NotFound();
            return p;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<People> AddPeople(People item)
        {
            if (item == null || item?.Id == 0)
                return BadRequest();
            _peoples.Add(item);
            return CreatedAtAction(nameof(GetByFam), new { fam = item.Fam }, item);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<People> DeletePeople(long id)
        {
            var headers = this.Request.Headers;
            if (!headers.TryGetValue("login", out StringValues login))
                return Unauthorized();

            var p = _peoples.Where(p => p.Id == id).FirstOrDefault();
            if (p == null)
                return NotFound();
            _peoples.Remove(p);
            return p;
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PutPeople(long id, People item)
        {
            if (id != item.Id)
                return BadRequest();
            
            try
            {
                var p = _peoples.Where(p => p.Id == id).FirstOrDefault();
                if (p == null)
                    return NotFound();
            }
            catch (Exception ex)
            {
                throw;
            }

            return NoContent();
        }
    }
}
