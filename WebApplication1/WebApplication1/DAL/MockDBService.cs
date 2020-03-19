using System;
using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class MockDBService : IDbService{
        private static IEnumerable<Student> _students;

        static MockDBService() {
            _students = new List<Student>
        {
            new Student{IdStudent = 1, FirstName = "Jan", LastName = "Kowalski"},
            new Student{IdStudent = 2, FirstName = "Anna", LastName = "Malewski"},
            new Student{IdStudent = 3, FirstName = "Andrzej", LastName = "Andrzejewicz"}
        };
        }


        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }

       
    }
}