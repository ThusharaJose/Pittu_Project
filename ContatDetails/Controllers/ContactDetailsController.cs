using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContatDetails.Controllers
{
    public class ContactDetailsController : Controller
    {
        private readonly ContactDetailsEntities db;
     
        public ContactDetailsController(ContactDetailsEntities db)
        {
            this.db = db;
        }
        public ActionResult Index()
        {
            return View(db.Contacts.ToList());
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(db.Contacts.Where(x=>x.Id ==id).FirstOrDefault());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Create(Contact contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Contacts.Add(contact);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {

                Contact contact = db.Contacts.Find(id);
                if (contact == null)
                {
                    return HttpNotFound();
                }
                return View(db.Contacts.Where(x => x.Id == id).FirstOrDefault());
            }
            catch
            {
                return View();
            }
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contact contact)
        {
            try
            {  
                if (ModelState.IsValid)
                {
                    db.Entry(contact).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(db.Contacts.Where(x=>x.Id==id).FirstOrDefault());
            }
        }

       [HttpGet]
        public ActionResult Delete(int id)
        {
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(db.Contacts.Where(x => x.Id == id).FirstOrDefault());
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Contact contact)
        {
            try
            {
                Contact contacts = db.Contacts.Find(id);
                db.Contacts.Remove(contacts);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
