using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DTOs.Requests;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface ILoginDBService
    {
        Student login(LoginRequest request, string key);
    }
}
