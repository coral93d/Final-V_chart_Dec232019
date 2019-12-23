using AspNetRoleBasedSecurity.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace AspNetRoleBasedSecurity.Controllers
{
    public class ChartController : Controller
    {
        private SqlConnection con;
        //To Handle connection related activities    
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["master"].ToString();
            con = new SqlConnection(constr);

        }

        private LineEntities _line = new LineEntities();

        LineEntities db = new LineEntities();
        OTCEntities otc = new OTCEntities();
        // GET: Chart
        public ActionResult DashBoard()
        {
            try
            {

                connection();
                SqlCommand com = new SqlCommand("ChartAllupdate", con);
                com.CommandType = CommandType.StoredProcedure;

                con.Open();
                int i = com.ExecuteNonQuery();
                con.Close();
                //////////////////Over all ----------CHART 1
                ViewBag.DataPoints1 = JsonConvert.SerializeObject(_line.MC_overall.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);
                ViewBag.DataPoints2 = JsonConvert.SerializeObject(_line.ER_overall.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);
                ViewBag.DataPoints3 = JsonConvert.SerializeObject(_line.PC_overall.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);
                ViewBag.DataPoints10 = JsonConvert.SerializeObject(_line.RQ_overall.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);
                ViewBag.DataPoints11 = JsonConvert.SerializeObject(_line.Ron_overall.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);



                ViewBag.DataPointsArea = JsonConvert.SerializeObject(_line.Line_overall.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);

                ///////// End CHART 1  ///////////////////////

                ViewBag.DataPoints4 = JsonConvert.SerializeObject(_line.Closes.Select(x => new { x.Label, x.Y }).ToArray(), _jsonSetting);
                ViewBag.DataPoints5 = JsonConvert.SerializeObject(_line.Opens.Select(x => new { x.Label, x.Y }).ToArray(), _jsonSetting);


                ViewBag.DataPoints6 = JsonConvert.SerializeObject(_line.Infra_by_track.Select(x => new { x.Name, x.Y }).ToArray(), _jsonSetting);

                ViewBag.DataPointspie = JsonConvert.SerializeObject(_line.Infra_by_track.Select(x => new { x.Name, x.Y }).ToArray(), _jsonSetting);

                
                /////////////////////////line open /close
                ViewBag.Line_open = JsonConvert.SerializeObject(_line.Line_Open.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);

                ViewBag.line_close = JsonConvert.SerializeObject(_line.Line_Close.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);

                ///////////////////////drill down 
                ViewBag.NewVsReturningVisitors = JsonConvert.SerializeObject(_line.Infra_by_track.Select(x => new { x.Name, x.Y }).ToArray(), _jsonSetting);

                ViewBag.NewVisitors = JsonConvert.SerializeObject(_line.Drill_mc.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);

                ViewBag.ReturningVisitors = JsonConvert.SerializeObject(_line.Drill_er.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);

               ViewBag.rq = JsonConvert.SerializeObject(_line.Drill_RQ.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);

                ViewBag.ron= JsonConvert.SerializeObject(_line.Drill_Ron.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);

                ViewBag.Pc = JsonConvert.SerializeObject(_line.Drill_pc.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);

                //admin

                ViewBag.admin = JsonConvert.SerializeObject(_line.Admin_Contribution.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);


                using (var context = new LineEntities())
                {
                    ViewBag.Total_Infra= context.Mains.SqlQuery("SELECT * FROM Main").Count();
                    ViewBag.Pending = context.Mains.SqlQuery("SELECT * FROM Main where Status='Pending'").Count();
                    ViewBag.Accepted = context.Mains.SqlQuery("SELECT * FROM Main where Status='Accepted'").Count();
                    ViewBag.Rejected = context.Mains.SqlQuery("SELECT * FROM Main where Status='Rejected'").Count();
                    ViewBag.Open = context.Mains.SqlQuery("SELECT * FROM Main where Status='Accepted' and Final_Status='0'").Count();
                    ViewBag.Close= context.Mains.SqlQuery("SELECT * FROM Main where Status='Accepted' and Final_Status='1'").Count();
                    ViewBag.Drill_count = context.Mains.SqlQuery("SELECT * FROM Main").Count();
                    ViewBag.Admin_count = context.Mains.SqlQuery("select * from Main where Status != 'Pending'").Count();
                }

                    return View(db.Mains.ToList());

            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return View("Error");
            }
            catch (System.Data.SqlClient.SqlException)
            {
                return View("Error");
            }

        }


        [HttpPost]
        public ActionResult DashBoard(string dropdownName)
        {
            try
            {
                ViewBag.dropdownName=dropdownName;

                if (dropdownName == "2010")
                {
                    return Redirect(Request.UrlReferrer.ToString());
                }

                connection();
                SqlCommand com = new SqlCommand("chartupdate", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Da", dropdownName);

                con.Open();
                int i = com.ExecuteNonQuery();
                con.Close();

                // Chart 1
                ViewBag.DataPoints1 = JsonConvert.SerializeObject(_line.mcs.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);
                ViewBag.DataPoints2 = JsonConvert.SerializeObject(_line.ers.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);
                ViewBag.DataPoints3 = JsonConvert.SerializeObject(_line.pcs.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);
                ViewBag.DataPoints10 = JsonConvert.SerializeObject(_line.RQs.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);
                ViewBag.DataPoints11 = JsonConvert.SerializeObject(_line.Rons.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);

                //Chart 4
                ViewBag.DataPoints4 = JsonConvert.SerializeObject(_line.Closes.Select(x => new { x.Label, x.Y }).ToArray(), _jsonSetting);
                ViewBag.DataPoints5 = JsonConvert.SerializeObject(_line.Opens.Select(x => new { x.Label, x.Y }).ToArray(), _jsonSetting);


                ViewBag.DataPoints6 = JsonConvert.SerializeObject(_line.Infra_by_track.Select(x => new { x.Name, x.Y }).ToArray(), _jsonSetting);
                ViewBag.DataPointspie = JsonConvert.SerializeObject(_line.Infra_by_track.Select(x => new { x.Name, x.Y }).ToArray(), _jsonSetting);
                ViewBag.DataPointsArea = JsonConvert.SerializeObject(_line.Total_Area.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);

                /////////////////////////line open /close--------------didnt use
                ViewBag.Line_open = JsonConvert.SerializeObject(_line.Line_Open.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);

                ViewBag.line_close = JsonConvert.SerializeObject(_line.Line_Close.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);

                ///////////////////////drill down 
                ViewBag.NewVsReturningVisitors = JsonConvert.SerializeObject(_line.Infra_by_track.Select(x => new { x.Name, x.Y }).ToArray(), _jsonSetting);

                ViewBag.NewVisitors = JsonConvert.SerializeObject(_line.Drill_mc.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);

                ViewBag.ReturningVisitors = JsonConvert.SerializeObject(_line.Drill_er.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);

                ViewBag.rq = JsonConvert.SerializeObject(_line.Drill_RQ.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);

                ViewBag.ron = JsonConvert.SerializeObject(_line.Drill_Ron.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);

                ViewBag.Pc = JsonConvert.SerializeObject(_line.Drill_pc.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);

                //admin
                if (dropdownName == "2019")
                {
                    ViewBag.admin = JsonConvert.SerializeObject(_line.Admin_Contribution19.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);
                }
                else
                    if (dropdownName == "2020")
                {
                    ViewBag.admin = JsonConvert.SerializeObject(_line.Admin_Contribution20.Select(x => new { x.X, x.Y }).ToArray(), _jsonSetting);
                }


                using (var context = new LineEntities())
                {
                    ViewBag.Total_Infra = context.Mains.SqlQuery("SELECT * FROM Main").Count();
                    ViewBag.Pending = context.Mains.SqlQuery("SELECT * FROM Main where Status='Pending'").Count();
                    ViewBag.Accepted = context.Mains.SqlQuery("SELECT * FROM Main where Status='Accepted'").Count();
                    ViewBag.Rejected = context.Mains.SqlQuery("SELECT * FROM Main where Status='Rejected'").Count();
                    ViewBag.Open = context.Mains.SqlQuery("SELECT * FROM Main where Status='Accepted' and Final_Status='0'").Count();
                    ViewBag.Close = context.Mains.SqlQuery("SELECT * FROM Main where Status='Accepted' and Final_Status='1'").Count();
                    ViewBag.Drill_count = context.Mains.SqlQuery("SELECT * FROM Main").Count();
                    ViewBag.Admin_count = context.Mains.SqlQuery("select * from Main where Status != 'Pending'").Count();
                }

                
                    
                

                return View(db.Mains.ToList());

            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return View("Error");
            }
            catch (System.Data.SqlClient.SqlException)
            {
                return View("Error");
            }

        }

        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

        

    }
}