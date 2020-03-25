using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class SqlServerDbDAL : IStudentsDAL
    {
        public IEnumerable<Student> GetStudents(String constr)
        {
            var list = new List<Student>();

            using (SqlConnection con = new SqlConnection(constr))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from student";

                con.Open();
                SqlDataReader sqr = com.ExecuteReader();
                while (sqr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = sqr["IndexNumber"].ToString();
                    st.FirstName = sqr["FirstName"].ToString();
                    st.LastName = sqr["LastName"].ToString();
                    list.Add(st);
                }
            }
            return list;
        }
        
    }
}
