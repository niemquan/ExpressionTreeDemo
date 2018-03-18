using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTreeDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            DynamicQueries.Query("quest");

            ModifyExpresion();

            ExpresionLambdaDemo.ExecuteExpresionTree();
            ExpresionLambdaDemo.ParsingExpressionTree();
            ExpresionLambdaDemo.NumLessFiveDemo();
            ExpresionLambdaDemo.NumLessFiveDemoInitParameter();
            Console.ReadKey();
        }


        public static void ModifyExpresion()
        {
            Expression<Func<string, bool>> lambdaExpr = name => name.Length > 10 && name.StartsWith("J");

            Console.WriteLine(lambdaExpr);

            AndAlseModifer treeModify = new AndAlseModifer();
            Expression modifiedExpr = treeModify.ModifyAndElseToOrElse(lambdaExpr);

            Console.WriteLine(modifiedExpr);
        }
    }
}
