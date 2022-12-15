using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Helper
{
    public class SendEmail
    {
        public string RecieverEmailID { get; set; }
        public string RecieverDisplayName { get; set; }
        public string SenderID { get; set; }
        public string SenderDisplayName { get; set; }
        public string SenderIdPassword { get; set; }
        public string URLToBeSend { get; set; }
        public string URLToBeSendLogin { get; set; }
        public string SMTPHost { get; set; }
        public int Port { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string RoleName { get; set; }
        public int mobileOTP { get; set; }
        public int emailOTP { get; set; }
    }


}