using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using coreAPI.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace coreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;

        }

        [HttpGet]

        public JsonResult Get()
        {
            string query = @" select EmployeeId, EmployeeName,Department, convert(varchar(10),DateofJoining,120)as DateOfJoining,
                                PhotoFileName
                                from dbo.Employee";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using SqlCommand myCommand = new SqlCommand(query, myCon);
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]

        public JsonResult Post(Employee emp)
        {

            try
            {
                string query = @"
                        insert into dbo.Employee values 
                        (
                        '" + emp.EmployeeName + @"', 
                        '" + emp.Department + @"',
                        '" + emp.DateOfJoining + @"',
                        '" + emp.PhotoFileName + @"'
                        )
                           ";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using SqlCommand myCommand = new SqlCommand(query, myCon);
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }

                return new JsonResult("Added Successfully!!");
            }
            catch (Exception)
            {
                return new JsonResult("Failed to Add!!");
            }
        }


        [HttpPut]

        public JsonResult Put(Employee emp)
        {

            try
            {
                string query = @"
                               update dbo.Employee set 
                               EmployeeName='" + emp.EmployeeName + @"' ,
                               Department='" + emp.Department + @"' ,
                               DateOfJoining='" + emp.DateOfJoining + @"' ,
                               PhotoFileName='" + emp.PhotoFileName + @"' 
                               where EmployeeId = " + emp.EmployeeId + @"
                               ";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using SqlCommand myCommand = new SqlCommand(query, myCon);
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }

                return new JsonResult("Added Successfully!!");

            }
            catch (Exception)
            {
                return new JsonResult("Failed to Add!!");
            }
        }

        [HttpDelete("{id}")]

        public JsonResult Delete(int id)
        {

            try
            {
                string query = @" delete from dbo.Employee where EmployeeId = " + id + @" ";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using SqlCommand myCommand = new SqlCommand(query, myCon);
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }

                return new JsonResult("Added Successfully!!");
            }
            catch (Exception)
            {
                return new JsonResult("Failed to Delete!!");
            }
        }

        [Route("SaveFile")]
        [HttpPost]

        public JsonResult SaveFile()
        {
            try
            {

                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");

            }
        }
    }
}
