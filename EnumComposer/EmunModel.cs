﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumComposer
{
    public class EnumModel //: IEnumModel
    {
        //private EnumNameConverter _converter;

        public string SqlServer { get; set; }

        public string SqlDatabase { get; set; }

        public string SqlSelect { get; set; }

        public string Name { get; set; }

        public int SpanStart { get; set; }

        public int SpanEnd { get; set; }

        public string LeadingTrivia { get; set; }

        public List<EnumModelValue> Values { get; set; }

        public EnumModel()
        {
            //_converter = new EnumNameConverter();
            Values = new List<EnumModelValue>();
        }

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

        public void FillFromDb(int value, string name, string description = null)
        {
            string nameCs = EnumNameConverter.MakeValidIdentifier(name);

            EnumModelValue modelValue = Values.SingleOrDefault(e => e.Value == value);
            if (modelValue == null)
            {
                /* Was found in DB but has not been discovered in code. Must have been added in Db. */
                modelValue = new EnumModelValue();
                modelValue.Value = value;
                modelValue.NameCs = nameCs;
                modelValue.Description = description;
                modelValue.IsActive = false;
                Values.Add(modelValue);
            }
            else
            {
                /* Was found both in DB and in code.*/
                //modelValue.Name = name;
                modelValue.NameCs = EnumNameConverter.MakeValidIdentifier(name);  /* Refresh CS name. */
                modelValue.Description = description;
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
            if (LeadingTrivia == null)
            {
                LeadingTrivia = "";
            }

            string result = Environment.NewLine;
            foreach (EnumModelValue value in Values.OrderBy(e => e.Value))
            {
                if (value.IsInDB)
                {
                    /* only need to output if there is a reference in DB */

#if DEBUG
                    result += string.Format("{2}\t\t{1}{0}", Environment.NewLine, value.ToString(), LeadingTrivia);
#else
                    result += string.Format("\t\t{1}{0}", Environment.NewLine, value.ToString());
#endif
                }
            }

            return result;
        }
    }
}