using Newtonsoft.Json;
using Remote.Linq;
using Remote.Linq.Newtonsoft.Json;

namespace SnowaTec.Test.Web.Contract
{
    public class LinqExpressioncs
    {
        /// <summary>
        /// Deserialise a LINQ expression tree
        /// </summary>
        public static Remote.Linq.Expressions.Expression DeserialiseRemoteExpression<TExpression>(string json) where TExpression : Remote.Linq.Expressions.Expression
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings().ConfigureRemoteLinq();
            Remote.Linq.Expressions.Expression result = JsonConvert.DeserializeObject<TExpression>(json, serializerSettings);
            return result;
        }
        /// <summary>
        /// Serialise a remote LINQ expression tree
        /// </summary>
        public static string SerialiseRemoteExpression<TExpression>(TExpression expression) where TExpression : Remote.Linq.Expressions.Expression
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings().ConfigureRemoteLinq();
            string json = JsonConvert.SerializeObject(expression, serializerSettings);
            return json;
        }
        /// <summary>
        /// Convert the specified Remote.Linq Expression to a .NET Expression
        /// </summary>
        public static System.Linq.Expressions.Expression<Func<T, TResult>> ToLinqExpression<T, TResult>(Remote.Linq.Expressions.LambdaExpression expression)
        {
            var exp = expression.ToLinqExpression();
            var lambdaExpression = System.Linq.Expressions.Expression.Lambda<Func<T, TResult>>(exp.Body, exp.Parameters);
            return lambdaExpression;
        }
    }
}
