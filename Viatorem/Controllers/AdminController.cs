using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ViatoremModel;
using System.Data.Entity.Validation;

namespace Viatorem.Controllers
{
    public class AdminController : Controller
    {
        Entities dbConnection = new Entities();
        public IEnumerable<Location> Model { get; set; }


        /*Main page for Admin*/
        public ActionResult AdminIndex() {
            User currentUser = (User)Session["user"];
            if (currentUser == null || currentUser.Admin == false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }



        /*---------------------LOCATION-----------------------*/

        

        public ActionResult Location()
        {
            User currentUser = (User)Session["user"];
            if (currentUser == null || currentUser.Admin == false)
            {
                return RedirectToAction("Index", "Home");
            }
            IQueryable<Location> locations = dbConnection.Locations.Where(l => l.Deleted == false)
                                            .OrderBy(l=>l.Name);
            return View(locations);
        }
        public ActionResult AddLocation(string locationName)
        {
            Location location = new Location();


            IQueryable<Location> returnLocations = dbConnection.Locations.Where(l => l.Name.Equals(locationName));
            if (returnLocations.Count() == 0)
            {

                location.LocationID = System.Guid.NewGuid();
                location.Name = locationName;
                dbConnection.Locations.Add(location);

            }
            else if (returnLocations.Count() > 0)
            {
                var locationToAdd = dbConnection.Locations.FirstOrDefault(l => l.Name.Equals(locationName));
                if (locationToAdd != null)
                {
                    if (locationToAdd.Deleted == true)
                    {
                        locationToAdd.Deleted = false;
                    }
                    else locationToAdd.Deleted = false;
                }
            }

            

            dbConnection.SaveChanges();

            return RedirectToAction("Location");
        }

        public ActionResult DeleteLocation(string del)
        {

            var locGuid = Guid.Parse(del);
            //enought 1st, coz unique identifier!
            var locationToDelete = dbConnection.Locations.FirstOrDefault(l => l.LocationID == locGuid);
            if (locationToDelete != null)
            {
                locationToDelete.Deleted = true;
               //Cascade if delete is true
				List<Route> routesToDelete = dbConnection.Routes.Where(r => r.StartLocation == locGuid || r.EndLocation == locGuid).ToList<Route>();
				if(routesToDelete.Count()!=0){
					foreach(var ro in routesToDelete){
					ro.Deleted=true;
					//cascade if delete is true
					var schedulesToDelete = dbConnection.Schedules.Where(s => s.RouteID == ro.RouteID).ToList<Schedule>();
					if(schedulesToDelete.Count()!=0){
						foreach(var sch in schedulesToDelete){
							sch.Deleted=true;
							}
						}
					}
				}
            }
            dbConnection.SaveChanges();
            return RedirectToAction("Location");
        }



        /*---------------------ROUTE-----------------------*/

        public ActionResult Route()
        {
            User currentUser = (User)Session["user"];
            if (currentUser == null || currentUser.Admin == false)
            {
                return RedirectToAction("Index", "Home");
            }

            AdminViewModel adminView = new AdminViewModel();
            adminView.locations = dbConnection.Locations.Where(l => l.Deleted == false).OrderBy(l=>l.Name);
            adminView.routes = dbConnection.Routes.Where(r => r.startLocation.Deleted == false && r.endLocation.Deleted == false && r.Deleted == false);

            return View(adminView);
        }
        public ActionResult AddRoute(string start, string end)
        {
            Route route = new Route();

            if (start == end) {
                return RedirectToAction("Route");
            }

            IQueryable<Route> returnRoutes = dbConnection.Routes.Where(l => l.startLocation.Name.Equals(start) && l.endLocation.Name.Equals(end));
            if (returnRoutes.Count() == 0)
            {

                route.RouteID = System.Guid.NewGuid();
                route.Deleted = false;
                route.startLocation = dbConnection.Locations.Where(l => l.Name == start).FirstOrDefault();
                route.endLocation = dbConnection.Locations.Where(l => l.Name == end).FirstOrDefault();
                dbConnection.Routes.Add(route);


            }
            else if (returnRoutes.Count() > 0)
            {
                var routeToAdd = dbConnection.Routes.Where(r => r.startLocation.Name.Equals(start) && r.endLocation.Name.Equals(end)).FirstOrDefault();
                if (routeToAdd != null)
                {
                    if (routeToAdd.Deleted == true)
                        routeToAdd.Deleted = false;
                    else routeToAdd.Deleted = false;
                }
            }

            dbConnection.SaveChanges();


            return RedirectToAction("Route");
        }
        public ActionResult DeleteRoute(string routeId)
        {
            var guidRouteId = Guid.Parse(routeId);
            var routeToDelete = dbConnection.Routes.Where(r => r.RouteID == guidRouteId).FirstOrDefault(); //get that unique route!
            if (routeToDelete.Deleted == false)
            {
                routeToDelete.Deleted = true;
				//cascade if delete is true
				List<Schedule> schedulesToDelete = dbConnection.Routes.Where(s => s.RouteID == guidRouteId).ToList<Schedule>();
				if(schedulesToDelete.Count()!=0){
					foreach(var sch in schedulesToDelete){
						sch.Deleted = true;
					}
				}
            }
            dbConnection.SaveChanges();
            return RedirectToAction("Route");
        }



        /*---------------------BUS-----------------------*/

        public ActionResult Bus()
        {
            User currentUser = (User)Session["user"];
            if (currentUser == null || currentUser.Admin == false)
            {
                return RedirectToAction("Index", "Home");
            }
            IQueryable<Bus> buses = dbConnection.Buses.Where(b => b.Deleted == false);
            return View(buses);
        }
        public ActionResult AddBus(string busName, int numberOfSeats)
        {
            if (numberOfSeats % 4 != 0 || numberOfSeats > 68 || numberOfSeats < 12) {
                return RedirectToAction("Bus");
            }
            Bus newBus = new Bus();

            IQueryable<Bus> returnBuses = dbConnection.Buses.Where(b => b.BusName == busName);
            IQueryable<Bus> returnBusesDeleted = dbConnection.Buses.Where(b => b.BusName == busName && b.Deleted == true);
            if (returnBuses.Count() == 0)
            {
                newBus.BusName = busName;
                newBus.NumberOfSeats = numberOfSeats;
                newBus.BusID = System.Guid.NewGuid();
                dbConnection.Buses.Add(newBus);
            }
            else if (returnBusesDeleted.Count() > 0)
            {
                returnBusesDeleted.FirstOrDefault().NumberOfSeats = numberOfSeats;
                returnBusesDeleted.FirstOrDefault().Deleted = false;
				var busToAdd = dbConnection.Buses.Where(b => b.BusName.Equals(busName)).FirstOrDefault(); //maybe better check ID's
                if (busToAdd.Deleted == true)
                    busToAdd.Deleted = false;
                else busToAdd.Deleted = false;
            }

            dbConnection.SaveChanges();
            return RedirectToAction("Bus");
        }
        public ActionResult DeleteBus(string busId)
        {

            var busGuid = Guid.Parse(busId);
            var busToDelete = dbConnection.Buses.Where(b => b.BusID == busGuid).FirstOrDefault();
            if (busToDelete != null)
            {
                busToDelete.Deleted = true;
				var schedulesToDelete = dbConnection.Schedules.FirstOrDefault(s => s.BusID == busGuid);
				if (schedulesToDelete.Count() !=0)
				{
					foreach(var sch in schedulesToDelete)
					sch.Deleted = true;
				}
            }
            dbConnection.SaveChanges();
            return RedirectToAction("Bus");
        }

        /*---------------------SCHEDULE-----------------------*/
        public ActionResult Schedule()
        {
            User currentUser = (User)Session["user"];
            if (currentUser == null || currentUser.Admin == false)
            {
                return RedirectToAction("Index", "Home");
            }

            AdminViewModel viewModel = new AdminViewModel();

            viewModel.routes = dbConnection.Routes.Where(r => r.Deleted == false).ToList<Route>();
            viewModel.buses = dbConnection.Buses.Where(b => b.Deleted == false).ToList<Bus>();
            viewModel.schedules = dbConnection.Schedules.Where(s => s.Deleted == false).ToList<Schedule>();



            return View(viewModel);
        }
        public ActionResult AddSchedule(string busId, string routeId, DateTime date, DateTime departureTime, DateTime arrivalTime)
        {
            if (date == null || departureTime == null || arrivalTime == null) {
                return RedirectToAction("Schedule");
            }

            if (arrivalTime < departureTime) {
                ViewData["msg"] = "Please input the right time!";
                return RedirectToAction("Schedule");
            }

           
            departureTime = date.Date + departureTime.TimeOfDay;
            arrivalTime = date.Date + arrivalTime.TimeOfDay;

            var busGuid = Guid.Parse(busId);
            var routeGuid = Guid.Parse(routeId);

            bool addFlag = true;

            var scheduleToAdd = dbConnection.Schedules.FirstOrDefault(s => s.BusID == busGuid && s.RouteID == routeGuid &&
                                                                        s.DepartureTime == departureTime && s.ArrivalTime == arrivalTime &&
                                                                        s.Deleted == true);
            if (scheduleToAdd != null)
            {
                scheduleToAdd.Deleted = false;
            }


            Schedule newSchedule = new Schedule();
            List<Schedule> busSchedules = dbConnection.Schedules.Where(s => s.BusID == busGuid && s.Deleted == false).ToList<Schedule>();

            if (busSchedules.Count() != 0)
            {
                foreach (var sch in busSchedules)
                {
                
                    if ((departureTime < sch.DepartureTime && arrivalTime < sch.DepartureTime) || (departureTime > sch.ArrivalTime && departureTime > sch.ArrivalTime))
                    {
                        addFlag = true;

                    }
                    else
                    {
                        addFlag = false;
                        break;
                    }
                }

            }

            if (addFlag == true)
            {
                newSchedule.Route = dbConnection.Routes.Where(r => r.RouteID == routeGuid).FirstOrDefault();
                newSchedule.Bus = dbConnection.Buses.Where(b => b.BusID == busGuid).FirstOrDefault();



                newSchedule.Route.startLocation = dbConnection.Locations.Where(l => l.LocationID == newSchedule.Route.StartLocation).FirstOrDefault();
                newSchedule.Route.endLocation = dbConnection.Locations.Where(l => l.LocationID == newSchedule.Route.EndLocation).FirstOrDefault();


                newSchedule.ScheduleID = Guid.NewGuid();
                newSchedule.BusID = busGuid;
                newSchedule.RouteID = routeGuid;

                newSchedule.DepartureTime = departureTime;
                newSchedule.ArrivalTime = arrivalTime;
                newSchedule.Deleted = false;
                dbConnection.Schedules.Add(newSchedule);

            }
            else
            {
                TempData["msg"] = "Bus already in USE at this time!";
            }
            


            dbConnection.SaveChanges();
            return RedirectToAction("Schedule");

        }
        public ActionResult DeleteSchedule(string scheduleId)
        {
            var schGuid = Guid.Parse(scheduleId);
            Schedule scheduleToDelete = dbConnection.Schedules.Where(s => s.ScheduleID == schGuid).FirstOrDefault();

            scheduleToDelete.Deleted = true;
            dbConnection.SaveChanges();
            return RedirectToAction("Schedule");
        }

        /*---------------------USER-----------------------*/

        public ActionResult User() {

            User currentUser = (User)Session["user"];
            if (currentUser == null || currentUser.Admin == false)
            {
                return RedirectToAction("Index", "Home");
            }
            IEnumerable<User> users = dbConnection.Users.Where(u => u.Deleted == false).OrderBy(u=> u.Username);
            return View(users);
        }
        public ActionResult ChangeAdmin(string idOfUser)
        {
            string result = "";
            Guid userGuid = Guid.Parse(idOfUser);
            var userToChange = dbConnection.Users.FirstOrDefault(u => u.UserID == userGuid);
            if (userToChange != null) {
                userToChange.Admin = !userToChange.Admin;
                result = "Success";
            }
            else {
                result = "Error!";
                }
            dbConnection.SaveChanges();
            return Content(result);
                
        }
        public ActionResult DeleteUser(string userId)
        {
            var userGuid = Guid.Parse(userId);
            var userToDelete = dbConnection.Users.Where(b => b.UserID == userGuid).FirstOrDefault();
            if (userToDelete != null)
            {
                userToDelete.Deleted = true;
            }
            List<Seat> seatsToDelete = dbConnection.Seats.Where(s => s.UserID == userToDelete.UserID).ToList<Seat>();
            foreach (var seat in seatsToDelete) {
                dbConnection.Seats.Remove(seat);
            }
            dbConnection.SaveChanges();
            return RedirectToAction("User");
        }


    }
}