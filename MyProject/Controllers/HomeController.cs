using MyProject.Helper;
using MyProject.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Controllers
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
        public ActionResult Test()
        {
            var data = DBOperation.getUserType();
            ViewBag.UserType = data;
            return View();
        }
        public ActionResult Verification()
        {
            
            return View();
        }
        public ActionResult FormBuilderpage()
        {

            return View();
        }
        public ActionResult AddTemplate(string name, string template)
        {
            return RedirectToAction("FormBuilderpage");
        }

        public JsonResult GetUserType()
        {
            var data = DBOperation.getUserType();

            return new JsonResult
            {
                Data = data,
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult saveDetails(TrailuserModal trailuser)
        {
            var data = DBOperation.saveUserDetails(trailuser);
            return new JsonResult
            {
                Data = data,
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        
        public JsonResult updateStatus(VerificationRequest verification)
        {
            var data = DBOperation.UpadteVerificationStatus(verification);
            return new JsonResult
            {
                Data = data,
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }
}