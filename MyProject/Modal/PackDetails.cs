using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Modal
{
    public class PackDetails
    {
        public int iPk_PackId { get; set; }
        public string sPackName { get; set; }
        public decimal dPackFeaAmt { get; set; }
        public decimal dPackFunAmt { get; set; }
        public decimal totalAmount { get; set; }
        public int iNumReg { get; set; }
        public int iValidityvalue { get; set; }
        public int iValidityId { get; set; }
        public int iTypId { get; set; }
        public string iTypIdname
        {
            get
            {
                switch (iTypId)
                {
                    case 1:
                        return "Single";
                    case 2:
                        return "Multiple";
                    default:
                        return "";
                }
            }
        }
        public string iValidityIdName
        {
            get
            {
                switch (iValidityId)
                {
                    case 1:
                        return "Day";
                    case 2:
                        return "Month";
                    case 3:
                        return "Year";

                    default:
                        return "";
                }
            }
        }
        public int IPk_CovLicId { get; set; }
        public int iqty { get; set; }
        public string sMenuName { get; set; }
        public string iFk_LiceId { get; set; }
        public decimal Total_displayvalue { get; set; }

    }
    public class PackInsuranceDetails
    {
        public decimal packageCost { get; set; }
        public int iMnuId { get; set; }
        public string sMenuName { get; set; }
        public int IPk_CovLicId { get; set; }
        public int iqty { get; set; }
    }
    public class GlobalClass
    {
        public int id { get; set; }
        public string backColor { get; set; }
        public string Color { get; set; }
        public string imageURL { get; set; }


        public static List<GlobalClass> GetFeatureList()
        {
            List<GlobalClass> Lst = new List<GlobalClass>();

            Lst.Add(new GlobalClass
            {
                id = 1,
                backColor = "lightsteelblue",
                imageURL= "/Content/images/pricing-bg-blue@2x.png" //"/Content/images/pricing-bg-blue@2x.png"
            });

            Lst.Add(new GlobalClass
            {
                id = 2,
                backColor = "blanchedalmond",
                imageURL = "/Content/images/pricing-bg-orange@2x.png" // "Content/images/pricing-bg-orange@2x.png"
            });

            Lst.Add(new GlobalClass
            {
                id = 3,
                backColor = "gainsboro",
                imageURL = "/Content/images/pricing-bg-default@2x.png"// "Content/images/pricing-bg-default@2x.png"
            });

            return Lst;
        }
    }
   
}