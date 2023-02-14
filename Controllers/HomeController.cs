 using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using UserRegistration.Models;

namespace UserRegistration.Controllers
{
    // Conventions I am using:
    // Prefacing parameter values (passed into methods) to distinguish between class fields and improve readability
    public class HomeController : Controller
    {
        User_DatabaseEntities userDatabase = new User_DatabaseEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View(userDatabase.tblUsers.ToList());
        }

        public ActionResult Create(tblUser pUser)
        {
            if (ModelState.IsValid)
            {
                // Check if user is in database by searching for the input email
                List<tblUser> usersWithEmail = userDatabase.tblUsers.Where(user => user.Email == pUser.Email).ToList();
                bool foundUser = usersWithEmail.Count > 0 ? true : false;
                if (!foundUser)
                {
                    userDatabase.tblUsers.Add(pUser);
                    userDatabase.SaveChanges();
                }
            }
            return View(pUser);
        }

        public ActionResult Edit(int? pID)
        {
            if (pID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser retrieveUser = userDatabase.tblUsers.Where(user => user.UserID == pID).FirstOrDefault();
            if (retrieveUser == null)
            {
                return HttpNotFound();
            }
            return View(retrieveUser);
        }

        [HttpPost]
        public ActionResult Edit(tblUser pUser, int pID)
        {
            tblUser retrieveUser = userDatabase.tblUsers.Where(user => user.UserID == pID).FirstOrDefault();
            retrieveUser.UpdateUserInfo(pUser);
            userDatabase.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? pID)
        {
            if (pID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser retrieveUser = userDatabase.tblUsers.Where(user => user.UserID == pID).FirstOrDefault();
            if (retrieveUser == null)
            {
                return HttpNotFound();
            }
            return View(retrieveUser);
        }

        [HttpPost]
        public ActionResult Delete(int pID)
        {
            tblUser removeUser = userDatabase.tblUsers.Where(user => user.UserID == pID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                userDatabase.tblUsers.Remove(removeUser);
                userDatabase.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}