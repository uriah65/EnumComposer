using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSPTestModelAppLib.Folder2.Folder3
{
    class Enum3Config
    {


        [EnumSqlSelect("SELECT ContactTypeID, Name FROM Person.ContactType")]
        public enum ContactTypeEnum
        {
        }

        class B
        {
            [EnumSqlSelect("SELECT ContactTypeID, Name, Name FROM Person.ContactType")]
            public enum ContactType2Enum
            {

            }
        }
    }

    [EnumSqlSelect("SELECT AddressTypeId, Name FROM Person.AddressType")]
    public enum AddressTypeEnum
    {
    }
}

