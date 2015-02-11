﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestComposer
{
    public static class ConstantsPR
    {
        public static bool HasNoAdventureWorks
        {
            get { return Environment.MachineName.ToLower() != "dellstudioxps"; }
        }

        public static void AssertSpaceEqual(string expected, string actual, string message)
        {
            actual = StripSpace(actual);
            expected = StripSpace(expected);

            for (int i = 0; i < actual.Length; i++)
            {
                if (actual[i] != expected[i])
                {
                    string difference = actual.Substring(i);
                }
            }


            Assert.AreEqual(expected, actual, message);
        }

        public static string StripSpace(string str)
        {
            if (str == null)
            {
                return null;
            }

            str = str.Replace(" ", "");
            str = str.Replace(Environment.NewLine, "");
            str = str.Replace("\t", "");
            return str;
        }

        public static string RemoveTextBetween(this string text, string fromPart, string toPart)
        {
            int ix1 = text.IndexOf(fromPart) + fromPart.Length;
            int ix2 = text.IndexOf(toPart, ix1);

            string result = text.Remove(ix1, ix2 - ix1);
            return result;
        }
    }
}