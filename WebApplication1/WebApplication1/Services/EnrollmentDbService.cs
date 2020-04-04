using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;
using WebApplication1.Models;
using WebApplication1.DTOs.Requests;
using System.Globalization;
using WebApplication1.DTOs.Responses;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace WebApplication1.Services
{
    public class EnrollmentDbService : IEnrollmentDbService
    {
        private const string constr = "Data Source=db-mssql;Initial Catalog=s18678;Integrated Security=True";

        public string message { get; set; }
        public int code { get; set; }

        public EnrollStudentsResponse enroll(EnrollStudentRequest s)
        {
            EnrollStudentsResponse ret = new EnrollStudentsResponse();

            CultureInfo provider = CultureInfo.InvariantCulture;
            s.Bdate = DateTime.ParseExact(s.BirthDate.Replace('.', '-'), "dd-MM-yyyy", provider);


            using (var con = new SqlConnection(constr))
            using (var com = new SqlCommand())
            {

                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();
                com.Transaction = tran;
                try
                {
                    com.CommandText = "select IdStudy from studies where name = @name";
                    com.Parameters.AddWithValue("name", s.Studies);

                    var dr0 = com.ExecuteReader();
                    if (!dr0.Read())
                    {
                        tran.Rollback();
                        message = "Studia nie istnieja";
                        code = 400;
                        return null;
                    }

                    int IdStudy = (int)dr0["IdStudy"];

                    com.CommandText = "select * from enrollment where idStudy = @id and semester = 1";
                    com.Parameters.AddWithValue("id", IdStudy);

                    dr0.Close();
                    int idEnr = 0;
                    Boolean addEnr = false;

                    var dr1 = com.ExecuteReader();
                    if (dr1.Read())
                    {
                        idEnr = (int)dr1["IdEnrollment"];
                        ret.IdEnrollment = idEnr;
                        ret.IdStudy = (int)dr1["IdStudy"];
                        ret.Semester = (int)dr1["Semester"];
                        ret.StartDate = (DateTime)dr1["StartDate"];
                        dr1.Close();
                    }
                    else
                    {
                        addEnr = true;
                        dr1.Close();
                        com.CommandText = "select Max(IdEnrollment) AS MID from enrollment ";
                        var dr2 = com.ExecuteReader();
                        dr2.Read();
                        idEnr = ((int)dr2["MID"]) + 1;
                        dr2.Close();
                    }
                    com.Parameters.Clear();
                    com.CommandText = "Select IndexNumber from Student where IndexNumber = @idS";
                    com.Parameters.AddWithValue("idS", s.IndexNumber);
                    var dr3 = com.ExecuteReader();
                    if (dr3.Read())
                    {
                        message = "Student juz istnieje";
                        code = 400;
                        dr3.Close();
                        tran.Rollback();
                        return null;
                    }
                    dr3.Close();



                    if (addEnr)
                    {
                        com.Parameters.Clear();
                        com.CommandText = "Insert into Enrollment (IdEnrollment, Semester, IdStudy, StartDate) VALUES (@IdEnr, 1, @IdS, @sdate)";
                        com.Parameters.AddWithValue("IdEnr", idEnr);
                        com.Parameters.AddWithValue("IdS", IdStudy);
                        com.Parameters.AddWithValue("sdate", DateTime.Today);
                        com.ExecuteNonQuery();
                        com.CommandText = "select * from enrollment where IdEnrollmet = (Select Max(IdEnrollment) from enrollment)";
                        var dr4 = com.ExecuteReader();
                        dr4.Read();
                        ret.IdEnrollment = idEnr;
                        ret.IdStudy = (int)dr4["IdStudy"];
                        ret.Semester = (int)dr4["Semester"];
                        ret.StartDate = (DateTime)dr4["StartDate"];
                        dr4.Close();
                    }

                    com.Parameters.Clear();

                    com.CommandText = "Insert into Student (IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) values (@index, @name, @sname, @bdate, @Idenr);";
                    com.Parameters.AddWithValue("index", s.IndexNumber);
                    com.Parameters.AddWithValue("name", s.FirstName);
                    com.Parameters.AddWithValue("sname", s.LastName);
                    com.Parameters.AddWithValue("bdate", s.Bdate);
                    com.Parameters.AddWithValue("Idenr", idEnr);

                    com.ExecuteNonQuery();

                    tran.Commit();
                    return ret;
                }
                catch (SqlException e)
                {
                    tran.Rollback();
                    message = e.Message;
                    code = 400;
                    return null;
                }

            }
        }

        public PromoteAnswer promote(PromoteRequest r)
        {
            using (var con = new SqlConnection(constr))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();
                com.Transaction = tran;
                try
                {
                    int id = 0;

                    com.CommandText = "Promote";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Semester", r.Semester);
                    com.Parameters.AddWithValue("@Sname", r.Studies);
                    com.Parameters.AddWithValue("@EnrId", SqlDbType.Int);
                    com.Parameters["@EnrId"].Direction = ParameterDirection.Output;

                    com.ExecuteNonQuery();

                    id = (int)com.Parameters["@EnrId"].Value;

                    if (id == 0)
                    {
                        code = 404;
                        return null;
                    }
                    
                    com.Parameters.Clear();
                    com.CommandType = CommandType.Text;
                    com.CommandText = "Select * From Enrollment WHERE IdEnrollment = @Id";
                    com.Parameters.AddWithValue("Id", id);
                    var dr = com.ExecuteReader();
                    PromoteAnswer ans = new PromoteAnswer();
                    ans.EnrId = id;
                    ans.IdStudies = (int)dr["IdStudy"];
                    ans.Semester = (int)dr["Semester"];
                    ans.StartDate = (DateTime)dr["StartDate"];
                    return ans;

                }
                catch (SqlException e)
                {
                    tran.Rollback();
                    code = 400;
                    message = e.Message;
                    return null;
                }
            }
        }
    }
}
