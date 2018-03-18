using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTreeDemo
{
    public static class ExpresionLambdaDemo
    {
        public static void NumLessFiveDemo()
        {
            ParameterExpression num1 = Expression.Parameter(typeof(int), "num1");

            ConstantExpression fiveConstantVariable = Expression.Constant(5);

            BinaryExpression binary = Expression.LessThan(num1, fiveConstantVariable);

            var lambdaExp = Expression.Lambda<Func<int, bool>>(binary, num1);

            var result = lambdaExp.Compile()(10);

            Console.WriteLine("Num Less Five Demo, the parameter is 10 "+result);
        }

        public static void NumLessFiveDemoInitParameter()
        {
            ParameterExpression num1 = Expression.Parameter(typeof(int), "num1");

            ConstantExpression fiveConstantVariable = Expression.Constant(5);

            BinaryExpression numLessThenFive = Expression.LessThan(num1, fiveConstantVariable);

            Expression<Func<int, bool>> lambdaExp = Expression.Lambda<Func<int, bool>>(
                numLessThenFive,
                new ParameterExpression[] {num1});

            var result = lambdaExp.Compile()(3);

            Console.WriteLine(result);
        }

        public static void LoopExpressionDemo()
        {
            ParameterExpression value = Expression.Parameter(typeof(int), "value");
            ParameterExpression result = Expression.Parameter(typeof(int), "result");

            LabelTarget label = Expression.Label(typeof(int));

            BlockExpression block = Expression.Block(
                new[] {result},
                Expression.Assign(result, Expression.Constant(1)),
                Expression.Loop(Expression.Loop(Expression.IfThenElse(
                        Expression.GreaterThan(value, Expression.Constant(1)),
                        Expression.MultiplyAssign(result, Expression.PostDecrementAssign(value)),
                        Expression.Break(label, result)
                    ),
                    label)
                )
            );
            int factorial = Expression.Lambda<Func<int, int>>(block, value).Compile()(5);

            Console.WriteLine(factorial);
        }

        public static void ParsingExpressionTree()
        {
            Expression<Func<int, bool>> exprTree = num => num < 5;

            ParameterExpression parm = exprTree.Parameters[0];
            BinaryExpression operation = (BinaryExpression) exprTree.Body;
            ParameterExpression left = (ParameterExpression) operation.Left;
            ConstantExpression right = (ConstantExpression) operation.Right;

            Console.WriteLine("Decomposed expression: {0} => {1} {2} {3}",
                parm.Name,left.Name,operation.NodeType,right.Value);
        }

        //If an expression tree does not represent a lambda expression,
        //you can create a new lambda expression that has the original expression tree as its body, 
        //by calling the Lambda<TDelegate>(Expression, IEnumerable<ParameterExpression>) method. 
        //Then, you can execute the lambda expression as described earlier in this section.
        public static void ExecuteExpresionTree()
        {
            BinaryExpression binaryExpr = Expression.Power(Expression.Constant(2D), Expression.Constant(3D));

            Expression<Func<double>> lambdaExpr = Expression.Lambda<Func<double>>(binaryExpr);

            Func<double> compliedExpression = lambdaExpr.Compile();

            double result = compliedExpression();

            Console.WriteLine("result: " + result);
        }
    }
}
