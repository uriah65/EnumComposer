using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSPTestModelAppLib;

namespace VSPTestModelApplication
{
    class EnumExplicite
    {

        [EnumSqlCnn("#OleDb", @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\temp\database.accdb;Persist Security Info=False")]
        [EnumSqlSelect("SELECT id, name, notes FROM T_Colors")]
        public enum AccessEnum
        {
            //[Description("The color of tomato")]
            //Red = 1,
            //[Description("Sky color")]
            //Blue = 2,
            //[Description("Grass is green")]
            //Green = 3,
        }

        [EnumSqlCnn("#ODBC", @"Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=c:\temp\;Extensions=asc,csv,tab,txt;Persist Security Info=False")]
        [EnumSqlSelect(@"SELECT * FROM [data.csv]")]
        public enum OdbcEnum
        {
            //[Description("#F0F8FF	 	Shades	Mix")]
            //AliceBlue = 1,
            //[Description("#FAEBD7	 	Shades	Mix")]
            //AntiqueWhite = 2,
            //[Description("#00FFFF	 	Shades	Mix")]
            //Aqua = 3,
            //[Description("#7FFFD4	 	Shades	Mix")]
            //Aquamarine = 4,
            //[Description("#F0FFFF	 	Shades	Mix")]
            //Azure = 5,
            //[Description("#F5F5DC	 	Shades	Mix")]
            //Beige = 6,
            //[Description("#FFE4C4	 	Shades	Mix")]
            //Bisque = 7,
            //[Description("#000000	 	Shades	Mix")]
            //Black = 8,
            //[Description("#FFEBCD	 	Shades	Mix")]
            //BlanchedAlmond = 9,
            //[Description("#0000FF	 	Shades	Mix")]
            //Blue = 10,
            //[Description("#8A2BE2")]
            //BlueViolet = 11,
        }

        [EnumSqlCnn("(local)", "AdventureWorks2014")]
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
