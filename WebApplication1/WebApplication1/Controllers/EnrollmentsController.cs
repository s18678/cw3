using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;
using WebApplication1.Models;
using WebApplication1.DTOs.Requests;
using System.Globalization;
using WebApplication1.DTOs.Responses;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private IEnrollmentDbService ienr;
        public EnrollmentsController(IEnrollmentDbService service) {
            ienr = service;
        }


        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult EnrollStudent(EnrollStudentRequest s) {
           EnrollStudentsResponse ret = ienr.enroll(s);
            if (ret == null) 
                return BadRequest(ienr.message);
            else return Created("", ret);
        }


        [HttpPost("promotions")]
        [Authorize(Roles = "employee")]
        public IActionResult promote(PromoteRequest r)
        {
            PromoteAnswer ret = ienr.promote(r);
            if (ret == null)
            {
                if (ienr.code == 404)
                    return NotFound();
                return BadRequest(ienr.message);
            }
            else
                return Created("", ret);
        }
    }
}
