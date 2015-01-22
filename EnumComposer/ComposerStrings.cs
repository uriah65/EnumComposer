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

        public List<EnumModel> EnumModels { get; set; }

        private string SourceText { get; set; }

        public ComposerStrings(IEnumDbReader dbReader)
        {
            _dbReader = dbReader;
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
            CompilationUnitSyntax root = (CompilationUnitSyntax)tree.GetRoot();

            /* get name space */
            NamespaceDeclarationSyntax ns = root.Members.Single(m => m is NamespaceDeclarationSyntax) as NamespaceDeclarationSyntax;

            /* loop all root enumerations */
            foreach (EnumDeclarationSyntax enumeration in ns.Members.Where(n => n is EnumDeclarationSyntax))
            {
                EnumModel model = new EnumModel();
                EnumModels.Add(model);
                model.Name = enumeration.Identifier.ToString();
                model.SpanStart = enumeration.Span.Start;
                model.SpanEnd = enumeration.Span.End;

                //SyntaxTokenList mds = enumeration.Modifiers;
                //foreach (SyntaxToken md in mds)
                //{
                //    //SyntaxToken md1 = md[0];
                //    var trivias = md.LeadingTrivia;
                //    foreach (SyntaxTrivia trivia in trivias)
                //    {
                //        string strivia = trivia.ToString();
                //        if (strivia.StartsWith(ConstantsPR.ENUM_COMMENT_START))
                //        {
                //            strivia = strivia.Substring(ConstantsPR.ENUM_COMMENT_START.Length);
                //            model.SqlRequest.Parse(strivia);
                //        }
                //    }
                //}

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
                }

                /* get attributes on the enumeration declaration*/
                SyntaxList<AttributeListSyntax> attributesList = enumeration.AttributeLists;
                foreach (AttributeListSyntax attibutes in attributesList)
                {
                    foreach (AttributeSyntax attribute in attibutes.Attributes)
                    {
                        if (attribute.Name.ToString() == "EnumSqlSelect")
                        {
                            /* extract SqlSelect statement from EnumSqlSelectAttribute */
                            model.SqlSelect = GetAttributeValue(attribute.ArgumentList.Arguments[0]);
                        }
                        if (attribute.Name.ToString() == "EnumSqlCnn")
                        {
                            model.SqlServer = GetAttributeValue(attribute.ArgumentList.Arguments[0]);
                            model.SqlDatabase = GetAttributeValue(attribute.ArgumentList.Arguments[1]);
                        }
                    }
                }
            }
        }

        private void UpdateModelsFromBD()
        {
            foreach (EnumModel model in EnumModels)
            {
                UpdateModelFromBD(model);
            }
        }

        private void UpdateModelFromBD(EnumModel model)
        {
            if (_dbReader != null)
            {
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
            str = str.Substring(1, str.Length - 2); /* remove quotes */
            return str;
        }
    }
}