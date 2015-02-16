using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSPTestModelAppLib;

namespace VSPTestModelApplication
{
    class Enum
    {

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

        [EnumSqlCnn("#CONFIG", @"Access")]
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

        [EnumSqlCnn("#CONFIG", @"Text")]
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

    }
}
