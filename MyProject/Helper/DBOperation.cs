using Microsoft.Ajax.Utilities;
using MyProject.Modal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace MyProject.Helper
{
    public class DBOperation
    {
        private static SqlConnection con;
        private static SqlCommand cmd;
        private static SqlDataAdapter adpt;
        private static DataSet ds;
        private static DataTable dt;

        static DBOperation() => con = new SqlConnection(ConfigurationManager.AppSettings["DBCS"].ToString());
        public static SqlCommand AddParameters(SqlCommand cmd, SqlParameter[] param)
        {
            if (param != null)
            {
                foreach (SqlParameter sqlParameter in param)
                {
                    if (sqlParameter.Value == null || sqlParameter.Value.ToString() == "")
                        sqlParameter.Value = (object)DBNull.Value;
                    if (sqlParameter.SqlDbType != SqlDbType.VarBinary && sqlParameter.SqlDbType != SqlDbType.Image)
                        sqlParameter.Value = (object)HttpUtility.HtmlDecode(sqlParameter.Value.ToString());
                    cmd.Parameters.Add(sqlParameter);
                }
            }
            return cmd;
        }
        public static int ExecuteQuery(string spName, SqlParameter[] param)
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            cmd = new SqlCommand(spName, con);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cmd = AddParameters(cmd, param);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public static DataTable FillDataTable(string spName)
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            cmd = new SqlCommand(spName, con);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            dt = new DataTable();
            adpt = new SqlDataAdapter(cmd);
            adpt.Fill(dt);
            con.Close();
            return dt;
        }
        public static DataSet FillDataSet(string spName, SqlParameter[] param)
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            cmd = new SqlCommand(spName, con);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd = AddParameters(cmd, param);
            ds = new DataSet();
            adpt = new SqlDataAdapter(cmd);
            try
            {
                adpt.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public static DataTable FillDataTable(string spName, SqlParameter[] param)
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            cmd = new SqlCommand(spName, con);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd = AddParameters(cmd, param);
            dt = new DataTable();
            adpt = new SqlDataAdapter(cmd);
            try
            {
                adpt.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public static List<M_userType> getUserType(string userType = "UserType")
        {
            List<M_userType> userTypes = new List<M_userType>();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@selectionType", userType);

            DataSet ds = FillDataSet("[dbo].[USP_ADMIN_CustomFields_View]", param);
            if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    M_userType _UserType = new M_userType();
                    _UserType.DDLValue = Convert.ToInt32(item["DDLValue"]);
                    _UserType.DDLText = item["DDLText"].ToString();
                    userTypes.Add(_UserType);
                }
            }
            return userTypes;
        }

        public static Response saveUserDetails(TrailuserModal modal)
        {
            Response response = new Response();

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@sName", modal.sName);
            param[1] = new SqlParameter("@sPincode", modal.sPincode);
            param[2] = new SqlParameter("@sMobileNo", modal.sMobileNo);
            param[3] = new SqlParameter("@sEmailId", modal.sEmailId);
            param[4] = new SqlParameter("@sPassword", Encrypt(modal.sPassword));
            //param[5] = new SqlParameter("@iUsrTyp", modal.iUsrTyp);

            DataSet ds = FillDataSet("[dbo].[USP_SaveTrailuser]", param);
            if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
                response.status = Convert.ToInt32(ds.Tables[0].Rows[0]["StatusCode"]);
                response.message = ds.Tables[0].Rows[0]["Message"].ToString();
                response.userID = ds.Tables[0].Rows[0]["UserID"].ToString();
                response.mobileOTP = GetOTP();
                response.emailOTP = GetOTP();
                SendMail(new SendEmail { Message = "EmailOTP", RecieverDisplayName = modal.sName, RecieverEmailID = modal.sEmailId, emailOTP = response.emailOTP, mobileOTP = response.mobileOTP });
            }
            return response;
        }
        
        public static Response getPackageData(int userType)
        {
            Response response = new Response();

            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@userType", userType);
           
            DataSet ds = FillDataSet("[dbo].[USP_ADMIN_PackageDetails_select]", param);
            if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
                response.status = 1;
                response.message = "Packaged Details";
                response.DATA = ds.Tables[0];
            }
            return response;
        }

        public static Response PackageInsuranceDetails(int PackID)
        {
            Response api_Response = new Response();

            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@packID", PackID);

            DataSet ds = DBOperation.FillDataSet("[dbo].[USP_OPERATION_SelectInsuranceToBuy]", param);
            if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
                api_Response.status = 1;
                api_Response.message = "Packaged Details...";
                api_Response.DATA = ds.Tables[0];

            }
            return api_Response;
        }


        public static Response UpadteVerificationStatus(VerificationRequest modal)
        {
            Response response = new Response();

            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@usrID", modal.userID);
            param[1] = new SqlParameter("@type", modal.verificationType);

            DataSet ds = FillDataSet("[dbo].[USP_ADMIN_UpdateVerificationStatus_Update]", param);
            if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
                response.status = Convert.ToInt32(ds.Tables[0].Rows[0]["StatusCode"]);
                response.message = ds.Tables[0].Rows[0]["Message"].ToString();
                //SendMail(new SendEmail { Message = "EmailOTP", RecieverDisplayName = modal.sName, RecieverEmailID = modal.sEmailId,emailOTP= response.emailOTP,mobileOTP= response.mobileOTP });
            }
            return response;
        }

        public static string SendMail(SendEmail email, string partyID = "")
        {
            try
            {

                //mail.From = new MailAddress("");
                //smtp.Host = ""; //Or Your SMTP Server Address
                //smtp.Port = 25;
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new System.Net.NetworkCredential
                //("info@thelaundrypoint.in", "P@ravin@9322459888");




                //Server (Godaddy) Email Setting

                //var sendEmailSetting = new SendEmail
                //{
                //    Message = "Registration",
                //    SenderID = "accounts@thelaundryyard.com",
                //    SenderIdPassword = "P@ravin@9322459888",
                //    URLToBeSendLogin = "https://localhost:44393/Auth/Login",
                //    SenderDisplayName = "Smart Ensure",
                //    SMTPHost = "relay-hosting.secureserver.net",
                //    Port = 25
                //};

                //Server (Godaddy) Email Setting

                //Local Email Setting

                var sendEmailSetting = new SendEmail
                {
                    Message = "Registration",
                    SenderID = "vivekchoudhary424@gmail.com",
                    SenderIdPassword = "bqaogevzhmklzgux",
                    URLToBeSendLogin = "https://localhost:44393/Auth/Login",
                    SenderDisplayName = "vivek",
                    SMTPHost = "smtp.gmail.com",
                    Port = 587
                };

                //Local Email Setting


                var senderEmail = new MailAddress(sendEmailSetting.SenderID, sendEmailSetting.SenderDisplayName);
                var receiverEmail = new MailAddress(email.RecieverEmailID, email.RecieverDisplayName);
                var password = sendEmailSetting.SenderIdPassword;
                var sub = email.Subject;
                var text = "";
                if (email.Message == "Registration")
                {
                    sub = "New Registration";
                    text = $"<div>Hello Dear {email.RecieverDisplayName}, <br/><br/>Your Account has been created and under varification soon your account will be activated be patient<br/></div><br/>Click here to login :<a href='{sendEmailSetting.URLToBeSendLogin}'>Login Here</a> <br/><br/><table border=1 cellpadding=12 width=60%><tr><td>Username</td><td><b>{email.Username}</b></td></tr><tr><td>Password</td><td><b>{email.Password}</b></td></tr></table><br/><br/>Thanks,<br/> Team Smart Ensure";
                }
                if (email.Message == "EmailOTP")
                {
                    sub = "Email Verification";
                    text = $"<div>Hello Dear {email.RecieverDisplayName}, <br/><br/>Your varification code : {email.emailOTP}, Please Verify.....<br/></div><br/><br/>Thanks,<br/> Team Smart Ensure";
                }
                var body = text;
                var smtp = new SmtpClient
                {
                    Host = sendEmailSetting.SMTPHost,
                    Port = sendEmailSetting.Port,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = sub,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(mess);
                }
                return "Email Sent";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int GetOTP(int noOfDigits = 4)
        {
            Random rnd = new Random();
            Thread.Sleep(1500);
            return rnd.Next((int)Math.Pow(10, (noOfDigits - 1)), (int)Math.Pow(10, noOfDigits) - 1);
        }
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "KMDRE23870FDR3S";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "KMDRE23870FDR3S";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

    }
}