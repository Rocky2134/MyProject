using MyProject.Helper;
using MyProject.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult IndexNew()
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
        public ActionResult Index()
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

        public ActionResult getPackageData(int userTypeID)
        {
            List<PackDetails> packs = new List<PackDetails>();
            var data = DBOperation.getPackageData(userTypeID);
            if (data.DATA != null)
            {
                var d = JsonConvert.SerializeObject(data.DATA);

                packs = JsonConvert.DeserializeObject<List<PackDetails>>(d);
            }

            return View(packs);
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