using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumComposer
{
    public class EnumModel //: IEnumModel
    {
        public string SqlServer { get; set; }
        public string SqlDatabase { get; set; }
        public string SqlSelect { get; set; }

        private EnumNameConverter _converter;
        private string _name;
        private string _nameCs;

        

        public EnumModel()
        {
            _converter = new EnumNameConverter();
            Values = new List<EnumModelValue>();
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value.Trim(); // EXW
                _nameCs = _converter.Convert(_name);
            }
        }

        public string NameSc
        {
            get
            {
                return _nameCs;
            }
        }

        public int SpanStart { get; set; }

        public int SpanEnd { get; set; }


        public List<EnumModelValue> Values { get; set; }

        public void FillFromCode(int value, string nameCs, bool isActive)
        {
            AssertNoDuplicates(value, nameCs);

            EnumModelValue modelValue = new EnumModelValue();
            modelValue.Value = value;
            modelValue.NameCs = nameCs;
            modelValue.IsActive = isActive;
            modelValue.IsInDB = false; /* Assume */
            Values.Add(modelValue);
        }

        public void FillFromDb(int value, string name)
        {
            string nameCs = _converter.Convert(name);

            EnumModelValue modelValue = Values.SingleOrDefault(e => e.Value == value);
            if (modelValue == null)
            {
                /* Was found in DB but has not been discovered in code. Must have been added in Db. */
                modelValue = new EnumModelValue();
                modelValue.Value = value;
                modelValue.NameCs = nameCs;
                modelValue.IsActive = false;
                Values.Add(modelValue);
            }
            else
            {
                /* Was found both in DB and in code.*/
                modelValue.Name = name;
                modelValue.NameCs = _converter.Convert(name);  /* Refresh CS name. */
            }
            modelValue.IsInDB = true;
        }

        private void AssertNoDuplicates(int value, string nameCs)
        {
            bool hasValue = Values.Any(e => e.Value == value);
            if (hasValue)
            {
                throw new ArgumentException();
            }

            bool hasNameCs = Values.Any(e => e.NameCs == nameCs);
            if (hasNameCs)
            {
                throw new ArgumentException();
            }
        }

        public string ToCSharp()
        {
            string result = "";
            result += string.Format("[EnumSqlSelect(\"{1}\")]{0}", Environment.NewLine, SqlSelect);
            result += string.Format("public enum {1}{0}", Environment.NewLine, _nameCs);
            result += string.Format("{{{0}", Environment.NewLine);
            foreach (EnumModelValue value in Values.OrderBy(e => e.Value))
            {
                if (value.IsInDB)
                {
                    /* only need to output if there is a reference in DB */
                    result += string.Format("\t\t{1}{0}", Environment.NewLine, value.ToString());
                }
            }
            result += string.Format(@"}}");

            return result;
        }
    }
}