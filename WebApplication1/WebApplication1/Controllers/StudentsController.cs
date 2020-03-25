using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private const string constr = "Data Source=db-mssql;Initial Catalog=s18678;Integrated Security=True";

        private readonly IStudentsDAL ISD;

        public StudentsController(IStudentsDAL _st) {
            ISD = _st;
        }



        [HttpGet]
        public IActionResult GetStudents()
        {
            IEnumerable<Student> list = ISD.GetStudents(constr);
            return Ok(list);
        }
        
        [HttpGet ("{iNum}")]
        public IActionResult GetStudents([FromRoute] string iNum)
        {
            Regex rx = new Regex(@"s[0-9]+");
            if (!rx.IsMatch(iNum))
                return BadRequest();

            using (SqlConnection con = new SqlConnection(constr))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from student where indexnumber=@index";

                com.Parameters.AddWithValue("index", iNum);

                con.Open();
                SqlDataReader rd = com.ExecuteReader();
                
                if (rd.Read())
                {
                    var st = new Student();
                    st.IndexNumber = rd["IndexNumber"].ToString();
                    st.FirstName = rd["FirstName"].ToString();
                    st.LastName = rd["LastName"].ToString();
                    st.BirthDate = DateTime.Parse(rd["BirthDate"].ToString());
                    return Ok(st);
                }
                return NotFound();
            }
        }
        
       
        [HttpGet("cw4/{iNum}")]
        public IActionResult GetStudents2([FromQuery]string iNum)
        {

            Regex rx = new Regex(@"s[0-9]+");
            if (!rx.IsMatch(iNum))
                return BadRequest();

            using (SqlConnection con = new SqlConnection(constr))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = ("select studies.name from student " +
                    "inner join enrollment on student.idenrollment = enrollment.idenrollment " +
                    "inner join studies on studies.idstudy = enrollment.idstudy" +
                    " where student.indexnumber=@index");


                com.Parameters.AddWithValue("index", iNum);

                con.Open();
                SqlDataReader rd = com.ExecuteReader();

                if (rd.Read())
                {    
                    String name = rd["name"].ToString();
                    return Ok(name);
                }else
                    return NotFound();
            }
        }

                /*
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
                */
            }
}