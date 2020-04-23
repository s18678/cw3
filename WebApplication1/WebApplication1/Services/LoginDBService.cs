using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DTOs.Requests;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class LoginDBService : ILoginDBService
    {
        private const string constr = "Data Source=db-mssql;Initial Catalog=s18678;Integrated Security=True";

        public Student login(LoginRequest request, string key) {

            using (var con = new SqlConnection(constr))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                try
                {
                    Student st = new Student();

                    com.CommandText = "Select  FirstName,  convert(varchar(100),DecryptByPassPhrase('@key', Password)) as Password From Student WHERE IndexNumber = @indexnumber";
                    com.Parameters.AddWithValue("indexnumber", request.login);
                    com.Parameters.AddWithValue("key", key);
                    var dr0 = com.ExecuteReader();
                    if (!dr0.Read())
                    {
                        return null;
                    }
                    st.FirstName = (string)dr0["FirstName"];
                    st.IndexNumber = request.login;
                    string password = (string)dr0["Password"];
                    if (password.Equals(request.password)) {
                        return st;
                    }

                }
                catch (Exception e) { return null; }
                
            }
            return null;
        }
    }
}
