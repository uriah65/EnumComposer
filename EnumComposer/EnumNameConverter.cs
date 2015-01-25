﻿using System;

namespace EnumComposer
{
    public class EnumNameConverter
    {
        public string Convert(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "empty";
            }

            string name2 = "";
            foreach (char ch in name.ToCharArray())
            {
                if (ch == ' ')
                {
                    continue;
                }

                if (char.IsDigit(ch) || char.IsLetter(ch) || ch == '_')
                {
                    name2 += ch;
                }
                else
                {
                    name2 += "_";
                }
            }
            name = name2;

            if (char.IsDigit(name[0]))
            {
                name = "_" + name;
            }
            else
            {
            }

            return name;
        }

        //todo: not nice mixing static and instance
        public static string EscapeDescription(string description)
        {
            if (description == null)
            {
                return description;
            }

            return description.Replace("\"", "\\\"");
        }

    }
}