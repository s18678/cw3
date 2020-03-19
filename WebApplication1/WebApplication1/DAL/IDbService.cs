using System;
using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public interface IDbService
    {
        IEnumerable<Student> GetStudents();
    }
}