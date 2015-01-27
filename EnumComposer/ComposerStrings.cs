#define NO_TRIVIA
using IEnumComposer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnumComposer
{
    public class ComposerStrings
    {
        private IEnumDbReader _dbReader;
        private IEnumLog _log;

        public List<EnumModel> EnumModels { get; set; }

        private string SourceText { get; set; }

        public ComposerStrings(IEnumDbReader dbReader, IEnumLog log = null)
        {
            _dbReader = dbReader;
            if (log == null)
            {
                _log = new DedbugLog();
            }
            else
            {
                _log = log;
            }
        }

        public void Compose(string sourceText)
        {
            /* This test reads a C# source file and parse it through Roslyn compiler.
            *  Enumeration model get filled for all enumerations found in the source file, and SQL statements get extracted
            */
            SourceText = sourceText;
            EnumModels = new List<EnumModel>();
            ParseSourceEnumerations(SourceText);
            UpdateModelsFromBD();
        }

        private void ParseSourceEnumerations(string source)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
            SyntaxNode syntaxRoot = tree.GetRoot();
            foreach (EnumDeclarationSyntax enumeration in syntaxRoot.DescendantNodes().OfType<EnumDeclarationSyntax>())
            {
                EnumModel model = new EnumModel();
                model.Name = enumeration.Identifier.ToString();

                /* get attributes on the enumeration declaration*/
                SyntaxList<AttributeListSyntax> attributesList = enumeration.AttributeLists;
                foreach (AttributeListSyntax attibutes in attributesList)
                {
                    foreach (AttributeSyntax attribute in attibutes.Attributes)
                    {
                        if (attribute.Name.ToString() == "EnumSqlSelect")
                        {
                            /* extract SqlSelect statement from EnumSqlSelectAttribute */
                            if (attribute.ArgumentList.Arguments.Count != 1)
                            {
                                throw new ApplicationException(string.Format("Invalid number of arguments {0} (expected 1), in EnumSqlSelectAttribute for {1} enumeration.",
                                    attribute.ArgumentList.Arguments.Count, model.Name));
                            }
                            model.SqlSelect = GetAttributeValue(attribute.ArgumentList.Arguments[0]);
                        }
                        if (attribute.Name.ToString() == "EnumSqlCnn")
                        {
                            if (attribute.ArgumentList.Arguments.Count != 2)
                            {
                                throw new ApplicationException(string.Format("Invalid number of arguments {0} (expected 2), in EnumSqlCnnAttribute for {1} enumeration.",
                                    attribute.ArgumentList.Arguments.Count, model.Name));
                            }
                            model.SqlServer = GetAttributeValue(attribute.ArgumentList.Arguments[0]);
                            model.SqlDatabase = GetAttributeValue(attribute.ArgumentList.Arguments[1]);
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(model.SqlSelect))
                {
                    continue;
                }

                EnumModels.Add(model);

                model.SpanStart = enumeration.OpenBraceToken.SpanStart + 1;
                model.SpanEnd = enumeration.CloseBraceToken.SpanStart;


#if TRIVIA
                {
                    int spanEnd = model.SpanEnd;
                    var triviaList = enumeration.CloseBraceToken.LeadingTrivia;
                    foreach (SyntaxTrivia trivia in triviaList)
                    {
                        if (trivia.SpanStart < spanEnd)
                        {
                            spanEnd = trivia.SpanStart;
                        }
                    }
                    model.SpanEnd = spanEnd;
                }
#endif

                /* loop all enumeration values*/
                foreach (EnumMemberDeclarationSyntax syntax in enumeration.Members)
                {
                    if (syntax.EqualsValue == null)
                    {
                        /* if enumeration option has no value we skip it, as if not exist*/
                        continue;
                    }
                    EnumModelValue value = new EnumModelValue();
                    model.Values.Add(value);
                    value.NameCs = syntax.Identifier.ToString();
                    value.IsActive = true;
                    string svalue = syntax.EqualsValue.Value.ToString();
                    value.Value = int.Parse(svalue);

#if TRIVIA
                    if (model.LeadingTrivia == null)
                    {
                        string triviaS = "";
                        var triviaList = syntax.GetLeadingTrivia();
                        foreach (SyntaxTrivia trivia in triviaList)
                        {
                            triviaS += trivia.ToFullString();
                        }
                        model.LeadingTrivia = triviaS;
                    }
#endif
                }
            }
        }

        private void UpdateModelsFromBD()
        {
            /* we need to keep SpanStart order, to honor connection EnumSqlCnnAttribute */
            foreach (EnumModel model in EnumModels.OrderBy(e => e.SpanStart))
            {
                UpdateModelFromBD(model);
            }
        }

        private void UpdateModelFromBD(EnumModel model)
        {
            if (_dbReader != null)
            {
                _log.WriteLine("'{0}' enumeration. Reading database.", model.Name);
                _dbReader.ReadEnumeration(model);
            }
        }

        public string GetResultFile()
        {
            StringBuilder result = new StringBuilder(1024);
            int ixLastInsert = SourceText.Length;
            string insertValue = "";
            foreach (var model in EnumModels.OrderByDescending(e => e.SpanEnd))
            {
                if (model.SpanEnd > ixLastInsert)
                {
                    throw new ApplicationException("Invalid enumeration order for '" + model.Name + "'.");
                }

                // insert in-between section
                insertValue = SourceText.Substring(model.SpanEnd, ixLastInsert - model.SpanEnd);
                result.Insert(0, insertValue, 1);

                // insert enumeration
                insertValue = model.ToCSharp();
                result.Insert(0, insertValue, 1);

                ixLastInsert = model.SpanStart;
            }

            insertValue = SourceText.Substring(0, ixLastInsert);
            result.Insert(0, insertValue, 1);

            return result.ToString();
        }

        private string GetAttributeValue(AttributeArgumentSyntax argument)
        {
            string str = argument.Expression.ToString();
            /* remove quotes */
            if (str.StartsWith("@"))
            {
                str = str.Substring(2, str.Length - 3);
            }
            else
            {
                str = str.Substring(1, str.Length - 2); 
            }
            return str;
        }
    }
}