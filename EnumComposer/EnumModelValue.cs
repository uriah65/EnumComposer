using System;

namespace EnumComposer
{
    public class EnumModelValue
    {
        public bool IsInDB { get; set; }

        public string Description { get; set; }

        public string NameCs { get; set; }

        public bool IsActive { get; set; }

        public int Value { get; set; }

        public new string ToString()
        {
            if (IsInDB == false)
            {
                return "";
            }


            string description = "";
            string result = string.Format("{0} = {1},", NameCs, Value);
            if (IsActive == false)
            {
                result = @"//" + result;
            }

            if (Description != null)
            {
                description = string.Format("[Description(\"{0}\")]", EnumNameConverter.EscapeDescription(Description));
                if (IsActive == false)
                {
                    description = @"//" + description;
                }
                result = description + Environment.NewLine + result;
            }

            return result;
        }
    }
}