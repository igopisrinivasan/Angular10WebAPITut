using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace WebApplication1.Controllers
{
    public class EmployeeController : ApiController
    {
        public HttpResponseMessage Get()
        {
            string query = @" select EmployeeId, EmployeeName,Department, convert(varchar(10),DateofJoining,120)as DateOfJoining,
                                PhotoFilename
                                from dbo.Employee";

            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))


            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }

            return Request.CreateResponse(HttpStatusCode.OK, table);
        }

        public string Post(Employee dep)
        {
            try
            {
                string query = @" insert into dbo.Employee values (
                         '" + dep.EmployeeName + @"' 
                        '" + dep.Department + @"'
                        '" + dep.DateOfJoining + @"'
                        '" + dep.PhotoFileName + @"'
                        )";
                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }
                return "Added Successfully!!";
            }
            catch (Exception)
            {
                return "Failed to Add!!";
            }
        }

        public string Put(Employee dep)
        {

            try
            {
                string query = @"
                               update dbo.Employee set 
                               EmployeeName='" + dep.EmployeeName + @"' 
                               Department='" + dep.Department + @"' 
                               DateOfJoining='" + dep.DateOfJoining + @"' 
                               PhotoFileName='" + dep.PhotoFileName + @"' 
                               where EmployeeId = " + dep.EmployeeId + @"
                               ";
                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }
                return "Updated Successfully!!";
            }
            catch (Exception)
            {
                return "Failed to Update!!";
            }
        }

        public string Delete(int id)
        {
            try
            {
                string query = @" delete from dbo.Employee where EmployeeId = " + id + @" ";
                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }
                return "Deleted Successfully!!";
            }
            catch (Exception)
            {
                return "Failed to Delete!!";
            }
        }

        [Route("api/Employee/GetAllDepartmentNames")]
        [HttpGet]
        public HttpResponseMessage GetAllDeparmentNames()
        {
            string query = @" select DepartmentName from dbo.Employee ";
            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }
            return Request.CreateResponse(HttpStatusCode.OK,table);
        }
        
    }
}
