using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTreeDemo
{
    public class DynamicQueries
    {
        public static readonly string[] Companies = new string[]
            {"Quest", "Dell", "Microsoft", "IBM", "Apple", "Google", "Facebook", "Twitter", "HP"};


        public static IQueryable<string> QueryableData = Companies.AsQueryable<string>();


        //companies.Where(company => (company.ToLower() == "coho winery" || company.Length > 16)).OrderBy(company => company)
        public static void Query(string name)
        {
            ParameterExpression paramCompany = Expression.Parameter(typeof(string), "company");

            MethodInfo toLower = typeof(string).GetMethod("ToLower",Type.EmptyTypes) ?? throw new InvalidOperationException();

            Expression left =
                Expression.Call(paramCompany, toLower );

            Expression right = Expression.Constant(name);

            BinaryExpression equal = Expression.Equal(left, right);

            PropertyInfo length = typeof(string).GetProperty("Length") ?? throw new InvalidOperationException();

            left = Expression.Property(paramCompany, length);

            right = Expression.Constant(8);

            BinaryExpression great = Expression.GreaterThan(left, right);

            Expression condition = Expression.OrElse(equal, great);

            MethodCallExpression whereMethod = Expression.Call(
                typeof(Queryable),
                "Where",
                new Type[] {QueryableData.ElementType},
                QueryableData.Expression,
                Expression.Lambda<Func<string, bool>>(condition, new ParameterExpression[] {paramCompany}));

            MethodCallExpression orderByMethod = Expression.Call(
                typeof(Queryable),
                "OrderBy",
                new Type[] {QueryableData.ElementType, QueryableData.ElementType},
                whereMethod,
                Expression.Lambda<Func<string, string>>(paramCompany, new ParameterExpression[] {paramCompany}));


            IQueryable<string> results = QueryableData.Provider.CreateQuery<string>(orderByMethod);

            foreach (string company in results)
            {
                Console.WriteLine(company);
            }

        }
    }
}
