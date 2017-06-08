using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MuseumMVC.Models;

namespace MuseumMVC.Controllers
{
    public class AdminController : Controller
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
                using (DB_Entities db = new DB_Entities())
                {
                    var obj = db.Persons.FirstOrDefault(a => a.Navn.Equals(objUser.Navn) && a.Password.Equals(objUser.Password) && a.Ansat.AnsatID == a.PersonID);
                    if (obj != null)
                    {
                        Session["LoginAdmin"] = obj.PersonID.ToString();
                        Session["Username"] = obj.Navn;
                        return RedirectToAction("Index", "Admin");
                    }
                }
            }
            return View(objUser);
        }

        public ActionResult Timestamps(int id)
        {
            if (Session["LoginAdmin"] != null)
            {
            var indstemplings = db.Indstemplings.Where(i => i.FK_PersonID == id);
            return View(indstemplings.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        public ActionResult Inactive(Person model)
        {
            if (Session["LoginAdmin"] != null)
            {
                var persons = db.Persons.Include(p => p.Ansat).Include(p => p.Frivillig).Include(p => p.Tovholder);
                return View(persons.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Person
        public ActionResult Index(Person model)
        {
            if (Session["LoginAdmin"] != null)
            {
                    var persons = db.Persons.Include(p => p.Ansat).Include(p => p.Frivillig).Include(p => p.Tovholder);
                                return View(persons.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        // GET: Person/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["LoginAdmin"] != null)
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

        // GET: Person/Create
        public ActionResult Create()
        {
            if (Session["LoginAdmin"] != null)
            {
                ViewBag.PersonID = new SelectList(db.Ansats, "AnsatID", "AnsatID");
                ViewBag.PersonID = new SelectList(db.Frivilligs, "FrivilligID", "FrivilligID");
                ViewBag.PersonID = new SelectList(db.Tovholders, "TovholderID", "TovholderID");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        // POST: Person/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonID,Navn,Efternavn,Tlf,Email,Password,QrID,Status")] Person person)
        {
            if (Session["LoginAdmin"] != null)
            {
                    if (ModelState.IsValid)
                                {
                                    db.Persons.Add(person);
                                    db.SaveChanges();
                                    return RedirectToAction("Index");
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

        // GET: Person/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["LoginAdmin"] != null)
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

        // POST: Person/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonID,Navn,Efternavn,Tlf,Email,Password,QrID,Status")] Person person)
        {
            if (Session["LoginAdmin"] != null)
            {
                    if (ModelState.IsValid)
                                {
                                    db.Entry(person).State = EntityState.Modified;
                                    db.SaveChanges();
                                    return RedirectToAction("Index");
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