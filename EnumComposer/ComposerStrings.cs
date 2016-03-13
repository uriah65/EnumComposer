#define TRIVIA

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

        public List<ModelSql> SqlModels { get; private set; }
        public List<EnumModel> EnumModels { get; set; }
        
        private string SourceText { get; set; }

        public ComposerStrings(IEnumDbReader dbReader, IEnumLog log = null)
        {
            _dbReader = dbReader;
            _log = log ?? new DedbugLog();
            SqlModels = new List<ModelSql>();
            EnumModels = new List<EnumModel>();
        }

        public void Compose(string sourceText)
        {
            /* Method reads a C# source file and parse it through Roslyn compiler.
            *  Enumeration model get filled for all enumerations found in the source file, and SQL statements get extracted
            */
            SourceText = sourceText;
            //ParseSourceEnumerations(SourceText);
            Scan(SourceText);

            if (EnumModels == null || EnumModels.Count == 0)
            {
                _log.WriteLine("no enumerations found.");
            }
            else
            {
                _log.WriteLine("" + EnumModels.Count() + " enumeration(s) found.");
            }

            UpdateModelsFromBD();
        }

        public void Scan(string sourceCode)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);
            SyntaxNode syntaxRoot = tree.GetRoot();

            ExtractSqlAttributes(syntaxRoot);

            ExtractActionAttributes(tree, syntaxRoot);

            ExtractDictionaryAttributes(tree, syntaxRoot);
        }

        private void ExtractSqlAttributes(SyntaxNode syntaxRoot)
        {
            foreach (AttributeSyntax attribute in syntaxRoot.DescendantNodes().OfType<AttributeSyntax>())
            {
                string attributeName = attribute.Name.ToString();
                if (attributeName == "EnumSqlCnn" || attributeName == "EnumSqlCnnAttribute")
                {
                    if (attribute.ArgumentList.Arguments.Count != 2)
                    {
                        throw new ApplicationException(string.Format("Invalid number of arguments {0} != (expected 2), in EnumSqlCnnAttribute {1}.",
                            attribute.ArgumentList.Arguments.Count, attribute.ToString()));
                    }

                    ModelSql sql = new ModelSql();
                    SqlModels.Add(sql);
                    sql.SqlProvider = GetAttributeValue(attribute.ArgumentList.Arguments[0]);
                    sql.SqlDatasource = GetAttributeValue(attribute.ArgumentList.Arguments[1]);
                    sql.Endpoint = attribute.FullSpan.End;
                }
            }
        }

        private void ExtractActionAttributes(SyntaxTree tree, SyntaxNode syntaxRoot)
        {
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
                        string attributeName = attribute.Name.ToString();
                        if (attributeName == "EnumSqlSelect" || attributeName == "EnumSqlSelectAttribute")
                        {
                            /* extract SqlSelect statement from EnumSqlSelectAttribute */
                            if (attribute.ArgumentList.Arguments.Count != 1)
                            {
                                throw new ApplicationException(string.Format("Invalid number of arguments {0} (expected 1), in EnumSqlSelectAttribute for {1} enumeration.",
                                    attribute.ArgumentList.Arguments.Count, model.Name));
                            }
                            model.SqlSelect = GetAttributeValue(attribute.ArgumentList.Arguments[0]);
                        }

                        ModelSql sql = FindModelSql(enumeration.OpenBraceToken.FullSpan.Start);
                        if (sql == null)
                        {
                            throw new ApplicationException($"EnumSqlCnn attribute was not found for the enumeration {model.Name}.");
                        }

                        model.SqlProvider = sql.SqlProvider;
                        model.SqlDatasource = sql.SqlDatasource;
                    }
                }

                if (string.IsNullOrWhiteSpace(model.SqlSelect))
                {
                    continue;
                }

                EnumModels.Add(model);

                model.SpanStart = enumeration.OpenBraceToken.SpanStart + 1;
                model.SpanEnd = enumeration.CloseBraceToken.SpanStart;
                var lineSpan = tree.GetLineSpan(enumeration.OpenBraceToken.Span);
                model.OpenBraceCharacterPosition = lineSpan.StartLinePosition.Character;

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
            }
        }

        private void ExtractDictionaryAttributes(SyntaxTree tree, SyntaxNode syntaxRoot)
        {
            foreach (FieldDeclarationSyntax field in syntaxRoot.DescendantNodes().OfType<FieldDeclarationSyntax>())
            {
                //EnumModel model = new EnumModel();
                //model.Name = field.n. Identifier.ToString();

                ///* get attributes on the enumeration declaration*/
                SyntaxList<AttributeListSyntax> attributesList = field.AttributeLists;
                foreach (AttributeListSyntax attibutes in attributesList)
                {
                    foreach (AttributeSyntax attribute in attibutes.Attributes)
                    {
                        string attributeName = attribute.Name.ToString();
                        if (attributeName == "EnumDictionarySqlSelect" || attributeName == "EnumDictionarySqlSelectAttribute")
                        {

                            ArgumentListSyntax args = field.DescendantNodes().OfType<ArgumentListSyntax>().FirstOrDefault();
                            int ix1= args.CloseParenToken.FullSpan.End;
                            int ix2 = args.OpenParenToken.FullSpan.Start;

                            //            /* extract SqlSelect statement from EnumSqlSelectAttribute */
                            //            if (attribute.ArgumentList.Arguments.Count != 1)
                            //            {
                            //                throw new ApplicationException(string.Format("Invalid number of arguments {0} (expected 1), in EnumSqlSelectAttribute for {1} enumeration.",
                            //                    attribute.ArgumentList.Arguments.Count, model.Name));
                            //            }
                            //            model.SqlSelect = GetAttributeValue(attribute.ArgumentList.Arguments[0]);
                        }

                //        ModelSql sql = FindModelSql(enumeration.FullSpan.Start);
                //        if (sql == null)
                //        {
                //            throw new ApplicationException($"EnumSqlCnn attribute was not found for the enumeration {model.Name}.");
                //        }

                //        model.SqlProvider = sql.SqlProvider;
                //        model.SqlDatasource = sql.SqlDatasource;
                    }
                }

                //if (string.IsNullOrWhiteSpace(model.SqlSelect))
                //{
                //    continue;
                //}

                //EnumModels.Add(model);

                //model.SpanStart = enumeration.OpenBraceToken.SpanStart + 1;
                //model.SpanEnd = enumeration.CloseBraceToken.SpanStart;
                //var lineSpan = tree.GetLineSpan(enumeration.OpenBraceToken.Span);
                //model.OpenBraceCharacterPosition = lineSpan.StartLinePosition.Character;

                ///* loop all enumeration values*/
                //foreach (EnumMemberDeclarationSyntax syntax in enumeration.Members)
                //{
                //    if (syntax.EqualsValue == null)
                //    {
                //        /* if enumeration option has no value we skip it, as if not exist*/
                //        continue;
                //    }
                //    EnumModelValue value = new EnumModelValue();
                //    model.Values.Add(value);
                //    value.NameCs = syntax.Identifier.ToString();
                //    value.IsActive = true;
                //    string svalue = syntax.EqualsValue.Value.ToString();
                //    value.Value = int.Parse(svalue);
                //}
            }
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
                        string attributeName = attribute.Name.ToString();
                        if (attributeName == "EnumSqlSelect" || attributeName == "EnumSqlSelectAttribute")
                        {
                            /* extract SqlSelect statement from EnumSqlSelectAttribute */
                            if (attribute.ArgumentList.Arguments.Count != 1)
                            {
                                throw new ApplicationException(string.Format("Invalid number of arguments {0} (expected 1), in EnumSqlSelectAttribute for {1} enumeration.",
                                    attribute.ArgumentList.Arguments.Count, model.Name));
                            }
                            model.SqlSelect = GetAttributeValue(attribute.ArgumentList.Arguments[0]);
                        }
                        if (attributeName == "EnumSqlCnn" || attributeName == "EnumSqlCnnAttribute")
                        {
                            if (attribute.ArgumentList.Arguments.Count != 2)
                            {
                                throw new ApplicationException(string.Format("Invalid number of arguments {0} (expected 2), in EnumSqlCnnAttribute for {1} enumeration.",
                                    attribute.ArgumentList.Arguments.Count, model.Name));
                            }
                            model.SqlProvider = GetAttributeValue(attribute.ArgumentList.Arguments[0]);
                            model.SqlDatasource = GetAttributeValue(attribute.ArgumentList.Arguments[1]);
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
                var lineSpan = tree.GetLineSpan(enumeration.OpenBraceToken.Span);
                model.OpenBraceCharacterPosition = lineSpan.StartLinePosition.Character;


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

//#if TRIVIA && DEBUG
//                    if (model.LeadingTrivia == null)
//                    {
//                        string triviaS = "";
//                        var triviaList = syntax.GetLeadingTrivia();
//                        foreach (SyntaxTrivia trivia in triviaList)
//                        {
//                            triviaS += trivia.ToFullString();
//                        }
//                        model.LeadingTrivia = triviaS;
//                    }
//#endif
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
                //_log.WriteLine("updating '{0}' enumeration.", model.Name);
                _dbReader.ReadEnumeration(model);
            }
        }

        public string GetResultFile()
        {
            StringBuilder result = new StringBuilder(1024);
            int ixLastInsert = SourceText.Length;
            string insertString = "";
            foreach (var model in EnumModels.OrderByDescending(e => e.SpanEnd))
            {
                if (model.SpanEnd > ixLastInsert)
                {
                    throw new ApplicationException("Invalid enumeration order for '" + model.Name + "'.");
                }

                // insert in-between section
                insertString = SourceText.Substring(model.SpanEnd, ixLastInsert - model.SpanEnd);
                result.Insert(0, insertString, 1);

                // insert enumeration
                insertString = model.ToCSharp();
                result.Insert(0, insertString, 1);

                ixLastInsert = model.SpanStart;
            }

            insertString = SourceText.Substring(0, ixLastInsert);
            result.Insert(0, insertString, 1);

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

        private ModelSql FindModelSql(int startPoint)
        {
            for (int i = SqlModels.Count - 1; i >= 0; i--)
            {
                ModelSql model = SqlModels[i];
                if (model.Endpoint <= startPoint)
                {
                    return model;
                }
            }

            return null;
        }
    }
}