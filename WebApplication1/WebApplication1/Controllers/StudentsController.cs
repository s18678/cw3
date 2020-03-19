using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {

        private readonly IDbService _dbService;

        public StudentsController(IDbService _db) {
            _dbService = _db;
        }

        [HttpGet]
        public string GetStudents() {
            return "Nowak, Kowalski";
        }

        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            return Ok(_dbService.GetStudents());
        }

        [HttpGet ("{id:int}")]
        public IActionResult GetStudents([FromRoute] int id)
        {
            if (id == 1)
                return Ok("Nowak");
            else if (id == 2)
                return Ok("Kowalski");
            else
                return NotFound("Nie znaleziono");
        }

        [HttpPost]
        public IActionResult CreateStudent(Student std){
            std.IndexNumber = $"s{new Random().Next(1,20000)}";
            return Ok(std);
        }

        [HttpPut("{id:int}")]
        public IActionResult PutStudent([FromRoute] int id) {
            return Ok($"Aktualizacja ukonczona {id}");
        }
        [HttpDelete("{id:int}")]
        public IActionResult DelStudend([FromRoute] int id) {
            return Ok($"Usuwanie ukonczone {id}");
        }
    }
}