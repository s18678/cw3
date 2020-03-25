using System;

namespace WebApplication1.Models {
    public class Student {
        public int IdStudent { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNumber { get; set; }
        public DateTime BirthDate { get; set; }

        override
        public string ToString() {
            return $"{IdStudent}: {IndexNumber} {FirstName} {LastName}";
        }
    }
}