using IEnumComposer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumComposer
{
    public class CodeScanner
    {
        private IEnumDbReader _dbReader;
        private IEnumLog _log;

        private List<ModelSql> _sqls;


        public CodeScanner(IEnumDbReader dbReader, IEnumLog log = null)
        {
            _dbReader = dbReader;
            _log = log ?? new DedbugLog();
            _sqls = new List<ModelSql>();
        }

        public void Scan(string sourceCode)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);
            SyntaxNode syntaxRoot = tree.GetRoot();

            ExtractSqlAttributes(syntaxRoot);



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
                    _sqls.Add(sql);
                    sql.SqlProvider = GetAttributeValue(attribute.ArgumentList.Arguments[0]);
                    sql.SqlDatasource = GetAttributeValue(attribute.ArgumentList.Arguments[1]);
                    sql.Endpoint = attribute.FullSpan.End;
                }
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
    }
}
