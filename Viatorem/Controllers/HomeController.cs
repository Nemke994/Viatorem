using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ViatoremModel;

namespace Viatorem.Controllers
{
    public class HomeController : Controller
    {
        Entities dbConnection = new Entities();

        public ActionResult Index()
        {
            
            User currentUser = (User)Session["user"];
            if (currentUser != null && currentUser.Admin) {
                return RedirectToAction("AdminIndex", "Admin");
            }
            IQueryable<Location> locations = dbConnection.Locations.OrderBy(l=> l.Name);
            IQueryable<Seat> seats = dbConnection.Seats;
            HomeViewModel locationViewer = new HomeViewModel();
            locationViewer.seats = seats;
            locationViewer.locations = locations;
            return View(locationViewer);
        }
        public PartialViewResult Schedules(string startLocation, string endLocation, System.DateTime date)
        {
            HomeViewModel scheduleViewer = new HomeViewModel();
            scheduleViewer.seats = dbConnection.Seats;


            
            
            scheduleViewer.schedules = dbConnection.Schedules.Where(s => s.Route.startLocation.Name == startLocation &&
                                       s.Route.endLocation.Name == endLocation && DbFunctions.TruncateTime(s.DepartureTime) == date && s.Deleted == false);

            
            scheduleViewer.locations = dbConnection.Locations;

            return PartialView("_Schedules",scheduleViewer);
            
        }
        public ActionResult Reservate(string[] reservedId)
        {
            
            User user = (User)Session["user"];

            
            for (int i = 0; i < reservedId.Length; i++) {


                string seatID = reservedId[i];
                var seatToCheck = dbConnection.Seats.Where(s => s.SeatLoc ==seatID).FirstOrDefault();
                if (seatToCheck != null)
                    continue;
                Seat newSeat = new Seat();
                newSeat.SeatID = Guid.NewGuid();
                newSeat.UserID = user.UserID;
              
                string scheduleId = reservedId[i].Substring(0,reservedId[i].Length-2);

                newSeat.SeatLoc = scheduleId + reservedId[i].Substring(reservedId[i].Length - 2);
                Guid scheduleGuid = Guid.Parse(scheduleId);
                DateTime scheduleTime = dbConnection.Schedules.FirstOrDefault(s => s.ScheduleID == scheduleGuid).DepartureTime;
                if (scheduleTime < DateTime.Today) {
                    return RedirectToAction("Index", "Home");
                }
                newSeat.BusID = dbConnection.Schedules.FirstOrDefault(s => s.ScheduleID == scheduleGuid).BusID;
               newSeat.ScheduleID = Guid.Parse(scheduleId);
                dbConnection.Seats.Add(newSeat);
            }

            dbConnection.SaveChanges();
            
            

            string result = "Seat successfully reserved!";
            return Content(result);
        }

        public ActionResult Signup(string username, string pass,string confirmPass, string email) {

            ViewData["user_alert"] = "";


            
            User newUser = new User();
            

            if (username != null && pass != null && email != null) {

                var existingUser = dbConnection.Users.FirstOrDefault(u => u.Username == username);
                if (existingUser == null)
                {
                    if (pass != confirmPass) {
                        ViewData["user_alert"] = "Passwords must be the same!";
                        return View();
                    }
                    newUser.UserID = Guid.NewGuid();

                    if (pass.Length < 6 || pass.Length > 32)
                    {
                        ViewData["user_alert"] = "Password must be larger than 6 or less than 32 characters!";
                        return View();
                    }
                    if (username.Length < 6 || username.Length > 32)
                    {
                        ViewData["user_alert"] = "Username must be larger than 6 or less than 32 characters!";
                        return View();
                    }
                    newUser.Username = username;
                    newUser.Password = pass;
                    newUser.Email = email;
                    newUser.Admin = false;
                    newUser.Deleted = false;
                    dbConnection.Users.Add(newUser);
                    dbConnection.SaveChanges();
                    return RedirectToAction("Index");
					/* Maybe use regular expressions?
					Regex r=new Regex("[^A-Z0-9.$]$");
					//use regular expreesions for user name (at least 6 characters), pass (at least 6 characters and 1 number or special symbol), email (at least one "@" and ".com")...
					.
					.
					.
					if(!r.IsMatch(pass)){
						ViewData["user_alert"]="Pass is not safe!"
					}
					*/
                }
                else {
                    ViewData["user_alert"] = "This user already exists!";
                }
				//edit next, revive only if email and password's are equals
                var userToAdd = dbConnection.Users.Where(u => u.Username == username && u.Password == pass && u.Email == email).FirstOrDefault();
                if (userToAdd != null) {
                    userToAdd.Deleted = false;
                }
                dbConnection.SaveChanges();
                
            }
            
            return View();
        }

        public ActionResult Login(string username, string pass)
        {
            
            var userLogin = dbConnection.Users.FirstOrDefault(u => u.Username == username && u.Password == pass && u.Deleted == false);
            if (userLogin != null)
            {
                Session["user"] = userLogin;
                if (userLogin.Admin == true)
                {
                    return RedirectToAction("AdminIndex", "Admin");
                }
                else
                {
                    return RedirectToAction("Index","Home");
                }
            }
            else if(username != null && pass != null) {
                TempData["user_err"] = "Username or Password incorrect!";
            }
            return View();
        }

        public ActionResult Signout() {

            User user = (User)Session["user"];
            if (user == null)
            {
                return RedirectToAction("Login","Home");
            }
            Session["user"] = null;
            return RedirectToAction("Index","Home");
        }


        public ActionResult About() {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}