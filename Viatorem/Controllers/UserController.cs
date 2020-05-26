using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViatoremModel;

namespace Viatorem.Controllers
{
    public class UserController : Controller
    {
        Entities dbConnection = new Entities();
        // GET: User
        public ActionResult MyTickets(string sortSel)
        {
            User currentUser = (User)Session["user"];
            if (currentUser == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (currentUser.Admin == true) {
                return RedirectToAction("AdminIndex", "Admin");
            }
            TicketViewModel ticketViewModel = new TicketViewModel();
          

            List<Seat> tickets = dbConnection.Seats.Where(s => s.UserID == currentUser.UserID).ToList<Seat>();
            if (sortSel != null &&sortSel.Equals("date"))
            {
                tickets = dbConnection.Seats.Where(s => s.UserID == currentUser.UserID).OrderBy(s => s.Schedule.DepartureTime).ToList<Seat>();
            }
            else if(sortSel != null && sortSel.Equals("route"))
            {
                tickets = dbConnection.Seats.Where(s => s.UserID == currentUser.UserID).OrderBy(s => s.Schedule.Route.startLocation.Name).ToList<Seat>();
            }
            ticketViewModel.seats = tickets;
            ticketViewModel.user = currentUser;
            return View(ticketViewModel);
        }
        public ActionResult MyProfile()
        {
            User currentUser = (User)Session["user"];
            if (currentUser == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (currentUser.Admin == true)
            {
                return RedirectToAction("AdminIndex", "Admin");
            }
            return View();
        }

        public ActionResult Edit(string username,string oldPass, string pass, string email) {

            var currentUser = (User)Session["user"];
            var usertoEdit = dbConnection.Users.FirstOrDefault(u => u.UserID == currentUser.UserID);



            if (username != null || pass != null || email != null)
            {
                if (oldPass == usertoEdit.Password)
                {
					//edit validation
                    usertoEdit.Username = username; //dont allow to edit name?
                    usertoEdit.Password = pass;
                    usertoEdit.Email = email;
                }
               
            }

            currentUser = usertoEdit;
            Session["user"] = currentUser;
            dbConnection.SaveChanges();
            
            return RedirectToAction("MyProfile");
        }
 

        
        
    }
}