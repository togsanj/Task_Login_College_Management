using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Task_Login_College_Management.Models;

namespace Task_Login_College_Management.Controllers
{
    public class HomeController : Controller
    {

        //private string connectionString;
        private object connectionObject;

        string connectionString = "Data Source=ROHIT-U6JU28T5\\SQLEXPRESS;Initial Catalog=Task_Login_College_Management;Integrated Security=True;Encrypt=False";
        [Authorize]
        public ActionResult AddCollege()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Index(string UserIDAttribute, string PasswordAttribute)
        {
            DataTable dt = new DataTable();
            string ConnectionString = "Data Source=ROHIT-U6JU28T5\\SQLEXPRESS;Initial Catalog=Task_Login_College_Management;Integrated Security=True;Encrypt=False";

            string query = "select UserID, Password from Admin_Login where UserID=@UserID AND Password=@Password";

            SqlConnection connectionObject = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(query, connectionObject);
            cmd.Parameters.AddWithValue("@UserID", UserIDAttribute);
            cmd.Parameters.AddWithValue("Password", PasswordAttribute);
            connectionObject.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                FormsAuthentication.SetAuthCookie(UserIDAttribute, false);
                return RedirectToAction("AddCollege");
            }
            else
            {
                return Content("<script>alert('Invalid Credentials'); location.href='/admin/index'</script>");
            }

        }
        [Authorize]
        [HttpPost]
        public ActionResult AddCollege(About req)
        {
            DataTable dt = new DataTable();
            string ConnectionString = "Data Source=ROHIT-U6JU28T5\\SQLEXPRESS;Initial Catalog=Task_Login_College_Management;Integrated Security=True;Encrypt=False";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query = "INSERT INTO College(CollegeName, UniversityName, CollegeCode, CollegeAddress)  VALUES (@CollegeName, @UniversityName, @CollegeCode, @CollegeAddress)";


                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CollegeName", req.CollegeName1Attribute);
                cmd.Parameters.AddWithValue("@UniversityName", req.UniversityName1Attribute);
                cmd.Parameters.AddWithValue("@CollegeCode", req.CollegeCode1Attribute);
                cmd.Parameters.AddWithValue("@CollegeAddress", req.CollegeAddress1Attribute);

                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    con.Close();
                    if (res > 0)
                    {
                        return Content("<script>alert(`College Added`); location.herf='/admin/ListOfColleges'</script>");
                    }
                    else
                    {
                        return Content("<script>alert(`College not Added`); location.herf='/admin/AddColleges'</script>");
                    }

                }
            }


        public ActionResult ListOfColleges()
        {
            string connectionString = "Data Source=ROHIT-U6JU28T5\\SQLEXPRESS;Initial Catalog=Task_Login_College_Management;Integrated Security=True;Encrypt=False";

            DataTable dt = new DataTable();
            using (SqlConnection CON = new SqlConnection(connectionString))
            {
                string query = "SELECT  Id, CollegeName, UniversityName, CollegeCode, CollegeAddress from College";
                SqlCommand cmd = new SqlCommand(query, CON);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

            }
            return View(dt);
        }
        public ActionResult GetElemetById(int? ID) 
        {
            DataTable dt = new DataTable();
            using (SqlConnection CON = new SqlConnection(connectionString))
            {

                string query = "SELECT Id, CollegeName, UniversityName, CollegeCode, CollegeAddress from College WHERE Id=@Id";

                SqlCommand cmd = new SqlCommand(query, CON);
                cmd.Parameters.AddWithValue("@Id", ID);
                CON.Open(); 
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

            }
                return View(dt.Rows[0]);
        }

        [HttpPost]
        public ActionResult GetElemetById(int? IDAttribute, About EditData) 
        {
            DataTable dt = new DataTable();
            using (SqlConnection CON = new SqlConnection(connectionString))
            {
                string query = "UPDATE College SET CollegeName=@CollegeName, UniversityName=@UniversityName, CollegeCode=@CollegeCode, CollegeAddress=@CollegeAddress WHERE Id=@Id";

                SqlCommand cmd = new SqlCommand(query, CON);
                cmd.Parameters.AddWithValue("@Id", IDAttribute);
                cmd.Parameters.AddWithValue("@CollegeName", EditData.CollegeName1Attribute);
                cmd.Parameters.AddWithValue("@UniversityName", EditData.UniversityName1Attribute);
                cmd.Parameters.AddWithValue("@CollegeCode", EditData.CollegeCode1Attribute);
                cmd.Parameters.AddWithValue("@CollegeAddress", EditData.CollegeAddress1Attribute);
                CON.Open(); 
                
                int res = cmd.ExecuteNonQuery();
                if(res>0)
                {
                    return Content("<script>alert('Data Update success');location.href='/Home/ListOfColleges'</script>");
                }
                else
                {
                    return Content("<script>alert('Data Update Failed');location.href='/Home/GetElemetById?ID=" + IDAttribute  + "'</script>");
                }
            }
        }

        public ActionResult DeleteElemetById(int? ID)
        {
            //DataTable dt = new DataTable();
            using (SqlConnection CON = new SqlConnection(connectionString))
            {

                string query = "DELETE from College WHERE Id=@Id";

                SqlCommand cmd = new SqlCommand(query, CON);
                cmd.Parameters.AddWithValue("@Id", ID);
                CON.Open();
                cmd.ExecuteNonQuery();
                //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //adapter.Fill(dt);

            }
            return RedirectToAction("ListOfColleges");
        }
    }
}