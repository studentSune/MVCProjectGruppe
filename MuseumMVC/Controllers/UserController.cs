using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MuseumMVC.Models;

namespace MuseumMVC.Controllers
{
    public class UserController : Controller
    {
        private DB_Entities db = new DB_Entities();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Person objUser)
        {
            if (ModelState.IsValid)
            {
                var obj = db.Persons.FirstOrDefault(a => a.Navn.Equals(objUser.Navn) && a.Password.Equals(objUser.Password) && a.Status == 1);
                if (obj != null)
                {
                        Session["Login"] = obj.PersonID.ToString();
                        Session["Username"] = obj.Navn;
                        return RedirectToAction("Index", "User" , new { id = obj.PersonID });
                }
                
            }
            return View(objUser);
        }

        public ActionResult Timestamps(int id)
        {
            if (Session["Login"] != null)
            {
                var indstemplings = db.Indstemplings.Where(i => i.FK_PersonID == id);
                return View(indstemplings.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        // GET: User/Details/5
        public ActionResult Index(int? id)
        {
            if (Session["Login"] != null)
            {
                if (id == null)
                            {
                                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                            }
                            Person person = db.Persons.Find(id);
                            if (person == null)
                            {
                                return HttpNotFound();
                            }
                            return View(person);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Login"] != null)
            {
                if (id == null)
                            {
                                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                            }
                            Person person = db.Persons.Find(id);
                            if (person == null)
                            {
                                return HttpNotFound();
                            }
                            ViewBag.PersonID = new SelectList(db.Ansats, "AnsatID", "AnsatID", person.PersonID);
                            ViewBag.PersonID = new SelectList(db.Frivilligs, "FrivilligID", "FrivilligID", person.PersonID);
                            ViewBag.PersonID = new SelectList(db.Tovholders, "TovholderID", "TovholderID", person.PersonID);
                            return View(person);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonID,Navn,Efternavn,Tlf,Email,Password,QrID,Status")] Person person)
        {
            if (Session["Login"] != null)
            {
                    if (ModelState.IsValid)
                                {
                                    db.Entry(person).State = EntityState.Modified;
                                    db.SaveChanges();
                                    return RedirectToAction("Index", new { id = person.PersonID});
                                }
                                ViewBag.PersonID = new SelectList(db.Ansats, "AnsatID", "AnsatID", person.PersonID);
                                ViewBag.PersonID = new SelectList(db.Frivilligs, "FrivilligID", "FrivilligID", person.PersonID);
                                ViewBag.PersonID = new SelectList(db.Tovholders, "TovholderID", "TovholderID", person.PersonID);
            
                                return View(person);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
