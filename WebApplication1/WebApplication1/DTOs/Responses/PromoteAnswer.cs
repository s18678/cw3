using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.DTOs.Responses
{
    public class PromoteAnswer
    {
        public int EnrId { get; set; }
        public int Semester { get; set; }
        public int IdStudies { get; set; }
        public DateTime StartDate { get; set; }
    }
}
