﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using ZeroHunger01.Models;

namespace ZeroHunger01.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult RestaurantList()
        {
            var db = new ZHContext();
            var list = db.Restaurants.ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult AddRestaurant() {
            return View();
        }

        [HttpPost]
        public ActionResult AddRestaurant(Restaurant restaurant)
        {
            var db = new ZHContext();
            db.Restaurants.Add(restaurant);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
            
        public ActionResult Request(int id)
        {
            var db = new ZHContext();
            List<Restaurant> reslist = null;
            var restitem = db.Restaurants.Find(id);
            if(Session["request"] == null) {
            reslist= new List<Restaurant>();
            }
            else
            {
                reslist = (List < Restaurant > )Session["request"];
            }
            reslist.Add(restitem);
            Session["request"]=reslist;
            return RedirectToAction("RestaurantList");
        }

        public ActionResult Requests()
        {
            
            var reslist = (List<Restaurant>)Session["request"];
            return View(reslist);
        }

        public ActionResult AddtoReqTable() {
            
            var cart = (List<Restaurant>)Session["request"];
            Restaurant restaurant = new Restaurant();
            restaurant.MaxTime = DateTime.Now;
            //restaurant.OrderStatus = "Available";

            ZHContext db = new ZHContext();
            
            db.SaveChanges();
            foreach (var p in cart)
            {
                var od = new Request();

                od.rid = p.Id;
                od.Location = p.Location;
                od.MaxTime = p.MaxTime;
                od.Foodname = p.Foodname;
                od.Quantity = p.Quantity;
                od.OrderStatus = "Available";
                
                db.Requests.Add(od);
            }
            
            db.SaveChanges();
            Session["request"] = null;
            TempData["Msg"] = "Order Placed Successfully";
            return RedirectToAction("Index");
        }

        //Employee
        public ActionResult ShowEmployeeList(int id)
        {
            Session["Reqid"] = id;

            var db = new ZHContext();
            var list = db.Employees.ToList();
            return View(list);
        }

        public ActionResult AfterLoginEmployee()
        {
            var db = new ZHContext();
            var list = db.Employees.ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult AddEmployee() {
            return View();
        }

        [HttpPost]
        public ActionResult AddEmployee(Employee employee)
        {
            var db = new ZHContext();
            db.Employees.Add(employee);
            db.SaveChanges();
            return RedirectToAction("AfterLoginEmployee");
        }

        //RequestListShow
        public ActionResult ShowRequestList()
        {
            var db = new ZHContext();
            var list = db.Requests.ToList();
            //TempData["id"] = id;
            return View(list);
        }

        public ActionResult GetEmployee(int id) {
            
            var db = new ZHContext();
            var eid = (from item in db.Employees
                       where item.Id == id
                       select item).SingleOrDefault();
            eid.EmpStatus = "Assigned";
            

            var process = new Process();
            process.Rid =(int)Session["ReqId"];
            var req = (from item in db.Requests
                       where item.Id == process.Rid
                       select item).SingleOrDefault();
            process.Eid = id;
            process.Foodname = req.Foodname;
            process.Quantity = req.Quantity;
            process.EmpStatus = eid.EmpStatus;
            process.OrderStatus = req.OrderStatus;
            db.Processes.Add(process);
            db.SaveChanges();

            return RedirectToAction("Index");
            }


        public ActionResult ProcessEmployee() {
            var db = new ZHContext();
            var emp = (Employee)Session["emp"];
            var c = (from item in db.Processes
                     where item.Eid == emp.Id
                     select item).ToList();

            return View(c);
        }

        public ActionResult Delivered(int id)
        {
            var db = new ZHContext();
            var process = db.Processes.Find(id);
            process.EmpStatus = "";
            process.OrderStatus = "Delivered";
            db.SaveChanges();
            return RedirectToAction("ProcessEmployee");
        }


        [HttpGet]
        public ActionResult LoginAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginAdmin(AdminLogin adminlogin)
        {
            var db = new ZHContext();
            var c = (from item in db.Admins
                     where item.username==adminlogin.username
                     && item.password==adminlogin.password
                     select item).SingleOrDefault();
            
            if(c == null)
            {
                TempData["Message"] = "Wrong username and password";
                return View();
            }
            else
            {
                return RedirectToAction("ShowRequestList");
            }
        }


        //EmployeeLogin
        public ActionResult DoEmployeeLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoEmployeeLogin(EmployeeLogin employeeLogin)
        {
            var db = new ZHContext();
            var c = (from item in db.Employees
                     where item.Username == employeeLogin.Username
                     && item.Password == employeeLogin.Password
                     select item).SingleOrDefault();

            if (c == null)
            {
                TempData["Message"] = "Wrong username and password";
                return View();
            }
            else
            {
                Session["emp"] = c;
                return RedirectToAction("ProcessEmployee");
            }
        }

    }
}