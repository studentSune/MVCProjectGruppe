using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MuseumMVC.Models;

namespace MuseumMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TjekIndTjekUd()
        {
            return View();
        }

        [HttpPost]
        public string manuelTjekIndTjekUd()
        {
            string userName = Request.Params["userName"].Trim();
            string password = Request.Params["password"].Trim();

            using (DB_Entities db = new DB_Entities())
            {
                Person isUserAndPasswordValid = db.Persons.Where(_c => _c.Navn == userName && _c.Password == password).FirstOrDefault();

                if (isUserAndPasswordValid == null)
                {
                    return "{\"msg\": \"Bruger eksisterer ikke eller Password er ikke korrekt\", \"code\": 400}";
                }

                var id = isUserAndPasswordValid.PersonID;

                Indstempling data = new Indstempling();
                data.FK_PersonID = id;
                data.Tidspunkt = DateTime.Now;

                //get data from DB
                List<Indstempling> hasBeenCheckedIn = db.Indstemplings.Where(_c => _c.FK_PersonID == id).ToList()
                // filter data on datetime date
                    .Where(_c => _c.Tidspunkt.Date == DateTime.Now.Date).ToList();

                if (hasBeenCheckedIn.Count == 1 || hasBeenCheckedIn.Count == 3)
                {
                    //set data for checkout
                    data.TidStatus = "Udstempling";
                }
                else if (hasBeenCheckedIn.Count == 0 || hasBeenCheckedIn.Count == 2)
                {
                    //set data for checkin
                    data.TidStatus = "Indstempling";
                }
                else
                {
                    //user trying to check more than the limit pr. day - throw error
                    return "{\"msg\": \"Din grænse er noget for Tjek ind for idag\", \"code\": 400}";
                }

                string status = data.TidStatus;

                //add data to table
                db.Indstemplings.Add(data);

                //commit changes to db
                db.SaveChanges();

                //return JSON to frontend
                return "{\"msg\": \"" + status + "\", \"code\": 200}";
            }
        }

        [HttpPost]
        public string validateChekIn(int id)
        {
            using (DB_Entities db = new DB_Entities())
            {
                Person doesUserExist = db.Persons.Where(_c => _c.PersonID == id).FirstOrDefault();

                if (doesUserExist == null)
                {
                    return "{\"msg\": \"Bruger eksisterer ikke\", \"code\": 400}";
                }

                //instantiate new object
                Indstempling data = new Indstempling();
                data.FK_PersonID = id;
                data.Tidspunkt = DateTime.Now;

                //get data from DB
                List<Indstempling> hasBeenCheckedIn = db.Indstemplings.Where(_c => _c.FK_PersonID == id).ToList()
                // filter data on datetime date
                    .Where(_c => _c.Tidspunkt.Date == DateTime.Now.Date).ToList();

                if (hasBeenCheckedIn.Count == 1 || hasBeenCheckedIn.Count == 3)
                {
                    //set data for checkout
                    data.TidStatus = "Udstempling";
                }
                else if (hasBeenCheckedIn.Count == 0 || hasBeenCheckedIn.Count == 2)
                {
                    //set data for checkin
                    data.TidStatus = "Indstempling";
                }
                else
                {
                    //user trying to check more than the limit pr. day - throw error
                    return "{\"msg\": \"Din grænse er noget for Tjek ind for idag\", \"code\": 400}";
                }

                string status = data.TidStatus;

                //add data to table
                db.Indstemplings.Add(data);

                //commit changes to db
                db.SaveChanges();

                //return JSON to frontend
                return "{\"msg\": \"" + status + "\", \"code\": 200}";
            }
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            Response.AddHeader("Cache-Control", "no-cache, no-store,must-revalidate");
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Expires", "0");
            Session.Abandon();

            Session.Clear();
            Response.Cookies.Clear();
            Session.RemoveAll();

            Session["Login"] = null;
            return View("Index");
        }
    }
}