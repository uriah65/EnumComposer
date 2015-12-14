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

        public string ToCsCode(string leadingTrivia)
        {
            if (IsInDB == false)
            {
                return "";
            }

            string result = string.Format("{0} = {1},", NameCs, Value);
            if (IsActive == false)
            {
                result = @"//" + result;
            }
            result = leadingTrivia + result;

            if (Description != null)
            {
                string description = string.Format("[Description(\"{0}\")]", EnumNameConverter.MakeValidDescription(Description));
                if (IsActive == false)
                {
                    description = @"//" + description;
                }
                result = leadingTrivia + description + Environment.NewLine + result;
            }

            return result;
        }
    }
}