using IEnumComposer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumComposer
{
    public class CodeScanner
    {
        private IEnumDbReader _dbReader;
        private IEnumLog _log;

        public List<ModelSql> SqlModels { get; private set; }
        public List<EnumModel> EnumModels { get; private set; }

        public CodeScanner(IEnumDbReader dbReader, IEnumLog log = null)
        {
            _dbReader = dbReader;
            _log = log ?? new DedbugLog();
            SqlModels = new List<ModelSql>();
            EnumModels = new List<EnumModel>();
        }

        public void Scan(string sourceCode)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);
            SyntaxNode syntaxRoot = tree.GetRoot();

            ExtractSqlAttributes(syntaxRoot);

            ExtractEnumAttributes(tree, syntaxRoot);

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

        private void ExtractEnumAttributes(SyntaxTree tree, SyntaxNode syntaxRoot)
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

                        ModelSql sql = FindModelSql(enumeration.FullSpan.Start);
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
                //SyntaxList<AttributeListSyntax> attributesList = enumeration.AttributeLists;
                //foreach (AttributeListSyntax attibutes in attributesList)
                //{
                //    foreach (AttributeSyntax attribute in attibutes.Attributes)
                //    {
                //        string attributeName = attribute.Name.ToString();
                //        if (attributeName == "EnumSqlSelect" || attributeName == "EnumSqlSelectAttribute")
                //        {
                //            /* extract SqlSelect statement from EnumSqlSelectAttribute */
                //            if (attribute.ArgumentList.Arguments.Count != 1)
                //            {
                //                throw new ApplicationException(string.Format("Invalid number of arguments {0} (expected 1), in EnumSqlSelectAttribute for {1} enumeration.",
                //                    attribute.ArgumentList.Arguments.Count, model.Name));
                //            }
                //            model.SqlSelect = GetAttributeValue(attribute.ArgumentList.Arguments[0]);
                //        }

                //        ModelSql sql = FindModelSql(enumeration.FullSpan.Start);
                //        if (sql == null)
                //        {
                //            throw new ApplicationException($"EnumSqlCnn attribute was not found for the enumeration {model.Name}.");
                //        }

                //        model.SqlProvider = sql.SqlProvider;
                //        model.SqlDatasource = sql.SqlDatasource;
                //    }
                //}

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
                if (model.Endpoint >= startPoint )
                {
                    return model;
                }
            }

            return null;
        }
    }
}