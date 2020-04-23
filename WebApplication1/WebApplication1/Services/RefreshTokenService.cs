using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private const string constr = "Data Source=db-mssql;Initial Catalog=s18678;Integrated Security=True";

        public void addRefToken(string login, String g)
        {

            using (var con = new SqlConnection(constr))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                try
                { //wywoływane tylko po sprawdzeniu loginu i hasła, więc nie trzeba sprawdzać istnienia loginu
                    com.CommandText = "UPDATE STUDENT SET Refkey = @key, RefExp = @exp WHERE IndexNumber = @login";
                    com.Parameters.AddWithValue("key", g);
                    com.Parameters.AddWithValue("exp", DateTime.Now.AddDays(1));
                    com.Parameters.AddWithValue("login", login);
                    com.ExecuteNonQuery();
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
        }

        public string checkRefToken(string g)
        {
            using (var con = new SqlConnection(constr))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                try
                {
                    com.CommandText = "SELECT IndexNumber, RefToken, RefExp FROM Student WHERE Reftoken = @Token";
                    com.Parameters.AddWithValue("Token", g);
                    var dr0 = com.ExecuteReader();
                    if (!dr0.Read()) { return null; }

                    string login = (string)dr0["IndexNumber"];
                    DateTime exp = (DateTime)dr0["RefExp"];
                    if (DateTime.Compare(exp, DateTime.Now) < 0)
                        return null;
                    return login;

                }
                catch (Exception e) { Console.WriteLine(e.Message); return null; }
            }
        }
    }
}
