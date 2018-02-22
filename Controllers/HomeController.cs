using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using belt_exam.Models;
using System.Linq;

namespace belt_exam.Controllers
{
    public class HomeController : Controller
    {
        private BeltContext _context;

        public HomeController ([FromServices] BeltContext context) {
            _context = context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route ("register")]
        public IActionResult Registration (RegisterViews ruser) {
            if (ModelState.IsValid) {
                List<User> isuser = _context.users.Where (useri => useri.Email == ruser.Email).ToList ();
                if (isuser.Count () > 0) {
                    string message = "There is already another user with this email!Please use other email!";
                    ViewBag.message = message;
                    return View ("Index");
                }

                User neWuser = new User () {
                    FirstName = ruser.FirstName,
                    LastName = ruser.LastName,
                    Email = ruser.Email,
                    Password = ruser.Password,
                };
                _context.users.Add (neWuser);
                _context.SaveChanges ();
                List<User> users = _context.users.ToList ();
                HttpContext.Session.SetInt32 ("Session", users[users.Count () - 1].UserId);
                return RedirectToAction ("Home");
            }
            return View("Index");
        }


        [HttpPost]
        [Route ("login")]
        public IActionResult LoginProcess (string email, string password) {
            List<User> user = _context.users.Where (u => u.Email == email).ToList ();
            if (user.Count > 0) {
                if (user[0].Password == password) {
                    HttpContext.Session.SetInt32 ("Session", user[0].UserId);
                    return RedirectToAction ("Home");
                } else {
                    string error = "Password you entered does not match what we have in our records!";
                    ViewBag.err = error;
                    return View ("Index");
                }
            } else {
                ViewBag.err = "We could not find your email in our database!";
            }
            return View ("Index");
        }

        [HttpGet]
        [Route("home")]
        public IActionResult Home()
        {
            if(HttpContext.Session.GetInt32("Session")==null){
                return RedirectToAction("Index");
            }
            int uid=Convert.ToInt32(HttpContext.Session.GetInt32("Session"));
            ViewBag.session=_context.users.SingleOrDefault(u=>u.UserId==uid).FirstName;
            List<Activity> Activities = _context.activities
                .Include (pa => pa.Joins)
                .ThenInclude (j => j.User)
                .ToList ();
            Activities.Reverse();
            ViewBag.activities=Activities;
            ViewBag.sessionid=uid;
            return View();
        }
        
        [HttpGet]
        [Route("new")]
        public IActionResult New()
        {
            if(HttpContext.Session.GetInt32("Session")==null){
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [Route("addActivity")]
        public IActionResult AddActivity(ActivityViews act,string min){
            if(ModelState.IsValid){
            Activity activity=new Activity(){
                UserId=Convert.ToInt32(HttpContext.Session.GetInt32("Session")),
                Title=act.Title,
                Description=act.Description,
                DateTime=act.DateTime,
                Duration=act.Duration+" "+min
            };
            _context.activities.Add(activity);
            _context.SaveChanges();
            List<Activity> activities=_context.activities.ToList();
            int uid=activities[activities.Count()-1].ActivityId;
            Join join=new Join(){
                UserId=Convert.ToInt32(HttpContext.Session.GetInt32("Session")),
                ActivityId=uid,
            };
            _context.joins.Add(join);
            _context.SaveChanges();
            return RedirectToAction("Home");
            }
            return View("New");
        }

        [HttpGet]
        [Route ("logout")]
        public IActionResult Logout () {
            HttpContext.Session.Clear ();
            return RedirectToAction ("Index");
        }

        [HttpGet]
        [Route("activity/{aid}")]
        public IActionResult ActivityPage(int aid){
            if(HttpContext.Session.GetInt32("Session")==null){
                return RedirectToAction("Index");
            }
            ViewBag.activity = _context.activities
                .Include (p => p.Joins)
                .ThenInclude (s => s.User)
                .SingleOrDefault(a=>a.ActivityId==aid);
            ViewBag.sessionid=Convert.ToInt32(HttpContext.Session.GetInt32("Session"));
            return View();
        }

        [HttpGet]
        [Route("join/{aid}")]
        public IActionResult Join(int aid){
            int uid=Convert.ToInt32(HttpContext.Session.GetInt32("Session"));
            Join join=new Join(){
                ActivityId=aid,
                UserId=uid,
            };
            _context.joins.Add(join);
            _context.SaveChanges();
            return RedirectToAction("Home");
        }
        [HttpGet]
        [Route("unjoin/{aid}")]
        public IActionResult Unjoin(int aid){
            int uid=Convert.ToInt32(HttpContext.Session.GetInt32("Session"));
            Join thisJoin=_context.joins.SingleOrDefault(p=>p.ActivityId==aid && p.UserId==uid);
            _context.joins.Remove(thisJoin);
            _context.SaveChanges();
            return RedirectToAction("Home");
        }

        [HttpGet]
        [Route("delete/{aid}")]
        public IActionResult Delete(int aid){
            Activity act=_context.activities.SingleOrDefault(a=>a.ActivityId==aid);
            _context.activities.Remove(act);
            List<Join> joins=_context.joins.Where(j=>j.ActivityId==aid).ToList();
            foreach(var j in joins)
            {
                _context.joins.Remove(j);
            }
            _context.SaveChanges();
            return RedirectToAction("Home");
            
        }
    }
}
