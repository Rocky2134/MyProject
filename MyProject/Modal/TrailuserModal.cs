using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Modal
{
    public class TrailuserModal
    {
        public int iPk_USRFRTRLID { get; set; } 
        public string sName { get; set; } 
        public string sPincode { get; set; } 
        public string sMobileNo { get; set; } 
        public string sEmailId { get; set; } 
        public string sPassword { get; set; } 
        public int iUsrTyp { get; set; } 
        public string dtDateOfReg { get; set; } 
        public int iIsactive { get; set; } 
    }

    public class Response
    {
        public string message { get; set; }
        public string userID { get; set; }
        public object DATA { get; set; }
        public int status { get; set; }
        public int mobileOTP { get; set; }
        public int emailOTP { get; set; }
    }
    public class VerificationRequest
    {
        public string userID { get; set; }
        public string verificationType { get; set; }
       
    }
}