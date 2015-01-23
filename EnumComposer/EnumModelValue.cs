namespace EnumComposer
{
    public class EnumModelValue
    {
        public bool IsInDB { get; set; }

        public string Name { get; set; }

        public string NameCs { get; set; }

        public bool IsActive { get; set; }

        public int Value { get; set; }

        public new string ToString()
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

            return result;
        }
    }
}