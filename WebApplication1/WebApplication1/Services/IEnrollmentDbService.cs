using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DTOs.Requests;
using WebApplication1.DTOs.Responses;

namespace WebApplication1.Services
{
    public interface IEnrollmentDbService
    {
        string message { get; set; }
        int code { get; set; }

        PromoteAnswer promote(PromoteRequest r);
        EnrollStudentsResponse enroll(EnrollStudentRequest s);
    }
}
