using System.Collections;

namespace EnumComposer
{
    public class EnumNameConverter
    {
        private const string EMPTY = "empty";

        private static string[] keywords = new string[]  {
                "abstract","event","new","struct","as","explicit","null","switch","base","extern",
                "this","false","operator","throw","break","finally","out","true",
                "fixed","override","try","case","params","typeof","catch","for",
                "private","foreach","protected","checked","goto","public",
                "unchecked","class","if","readonly","unsafe","const","implicit","ref",
                "continue","in","return","using","virtual","default",
                "interface","sealed","volatile","delegate","internal","do","is",
                "sizeof","while","lock","stackalloc","else","static","enum",
                "namespace",
                "object","bool","byte","float","uint","char","ulong","ushort",
                "decimal","int","sbyte","short","double","long","string","void",
                "partial", "yield", "where"};

        private static Hashtable keywordsTable;

        public static string MakeValidDescription(string description)
        {
            if (description == null)
            {
                return description;
            }

            return description.Replace("\"", "\\\"");
        }

        public static string MakeValidIdentifier(string identifier)
        {
            if (identifier == null)
            {
                return EMPTY;
            }

            identifier = identifier.Trim();
            if (identifier.Length == 0)
            {
                return EMPTY;
            }

            identifier = SyntaxCompline(identifier);
            identifier = KeywordCompline(identifier);

            return identifier;
        }

        private static string KeywordCompline(string identifier)
        {
            if (keywordsTable == null)
            {
                FillKeywordTable();
            }

            if (keywordsTable.Contains(identifier))
            {
                identifier = "_" + identifier;
            }

            return identifier;
        }

        private static string SyntaxCompline(string identifier)
        {
            /* check 1st character */
            if (Is_identifier_start_character(identifier[0]) == false)
            {
                identifier = "_" + identifier;
            }

            string result = "" + identifier[0];

            /* check the rest characters */
            for (int i = 1; i < identifier.Length; i++)
            {
                if (Is_identifier_part_character(identifier[i]))
                {
                    result += identifier[i];
                }
                else
                {
                    /* we ignore spaces and replace other characters with '_' */
                    if (identifier[i] != ' ')
                    {
                        result += "_";
                    }
                }
            }

            return result;
        }

        private static bool Is_identifier_start_character(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_' || c == '@' || char.IsLetter(c);
        }

        private static bool Is_identifier_part_character(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_' || (c >= '0' && c <= '9') || char.IsLetter(c);
        }

        private static void FillKeywordTable()
        {
            lock (keywords)
            {
                if (keywordsTable == null)
                {
                    keywordsTable = new Hashtable();
                    foreach (string keyword in keywords)
                    {
                        keywordsTable.Add(keyword, keyword);
                    }
                }
            }
        }
    }
}