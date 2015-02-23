using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSPTestModelAppLib.Folder2.Folder3
{
    class Enum3Config
    {

        [EnumSqlCnnAttribute("(local)","AdventureWorks2014")]
        [EnumSqlSelect("SELECT ContactTypeID, Name FROM Person.ContactType")]
        public enum ContactTypeEnum
        {
            //AccountingManager = 1,
            //AssistantSalesAgent = 2,
            //AssistantSalesRepresentative = 3,
            //CoordinatorForeignMarkets = 4,
            //ExportAdministrator = 5,
            //InternationalMarketingManager = 6,
            //MarketingAssistant = 7,
            //MarketingManager = 8,
            //MarketingRepresentative = 9,
            //OrderAdministrator = 10,
            //Owner = 11,
            //Owner_MarketingAssistant = 12,
            //ProductManager = 13,
            //PurchasingAgent = 14,
            //PurchasingManager = 15,
            //RegionalAccountRepresentative = 16,
            //SalesAgent = 17,
            //SalesAssociate = 18,
            //SalesManager = 19,
            //SalesRepresentative = 20,
        }

        class B
        {
            [EnumSqlSelect("SELECT ContactTypeID, Name, Name FROM Person.ContactType")]
            public enum ContactType2Enum
            {
                //[Description("Accounting Manager")]
                //AccountingManager = 1,
                //[Description("Assistant Sales Agent")]
                //AssistantSalesAgent = 2,
                //[Description("Assistant Sales Representative")]
                //AssistantSalesRepresentative = 3,
                //[Description("Coordinator Foreign Markets")]
                //CoordinatorForeignMarkets = 4,
                //[Description("Export Administrator")]
                //ExportAdministrator = 5,
                //[Description("International Marketing Manager")]
                //InternationalMarketingManager = 6,
                //[Description("Marketing Assistant")]
                //MarketingAssistant = 7,
                //[Description("Marketing Manager")]
                //MarketingManager = 8,
                //[Description("Marketing Representative")]
                //MarketingRepresentative = 9,
                //[Description("Order Administrator")]
                //OrderAdministrator = 10,
                //[Description("Owner")]
                //Owner = 11,
                //[Description("Owner/Marketing Assistant")]
                //Owner_MarketingAssistant = 12,
                //[Description("Product Manager")]
                //ProductManager = 13,
                //[Description("Purchasing Agent")]
                //PurchasingAgent = 14,
                //[Description("Purchasing Manager")]
                //PurchasingManager = 15,
                //[Description("Regional Account Representative")]
                //RegionalAccountRepresentative = 16,
                //[Description("Sales Agent")]
                //SalesAgent = 17,
                //[Description("Sales Associate")]
                //SalesAssociate = 18,
                //[Description("Sales Manager")]
                //SalesManager = 19,
                //[Description("Sales Representative")]
                //SalesRepresentative = 20,
            }
        }
    }

    [EnumSqlSelect("SELECT AddressTypeId, Name FROM Person.AddressType")]
    public enum AddressTypeEnum
    {
        //Billing = 1,
        //Home = 2,
        //MainOffice = 3,
        //Primary = 4,
        //Shipping = 5,
        //Archive = 6,
    }
}

