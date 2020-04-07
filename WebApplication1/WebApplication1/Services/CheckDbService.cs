using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class CheckDbService : ICheckDbService
    {
        private const string constr = "Data Source=db-mssql;Initial Catalog=s18678;Integrated Security=True";
        public bool check(string instr)
        {
            using (var con = new SqlConnection(constr))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                try
                {

                    com.CommandText = "select IdStudy from studies where IndexNumber = @index";
                    com.Parameters.AddWithValue("index", instr);

                    var dr0 = com.ExecuteReader();
                    if (!dr0.Read())
                    {
                        return false;
                    }
                    return true;
                }
                catch (SqlException e) {
                    Console.WriteLine(e);
                    return false;
                }
            }
        }
    }
}
