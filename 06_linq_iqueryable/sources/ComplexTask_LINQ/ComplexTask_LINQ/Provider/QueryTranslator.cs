using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ComplexTask_LINQ.Provider
{
    public class QueryTranslator : ExpressionVisitor
    {
        private StringBuilder _queryBuilder;
        private string _tableName;
        private List<string> _selectFields = new();
        private List<string> _joins = new();

        public QueryTranslator()
        {
            _queryBuilder = new StringBuilder();
        }

        public string Translate<T>(Expression expression)
        {
            _tableName = typeof(T).GenericTypeArguments.Length > 0
             ? typeof(T).GenericTypeArguments[0].Name + "s"
             : typeof(T).Name + "s";

            Visit(expression);

            var query = new StringBuilder();

            if (_selectFields.Any())
            {
                query.Append($"SELECT {string.Join(", ", _selectFields)} FROM {_tableName} ");
            }
            else
            {
                query.Append($"SELECT * FROM {_tableName} ");
            }

            if (_joins.Any())
            {
                query.Append(string.Join(" ", _joins));
            }

            query.Append(_queryBuilder.ToString());

            return query.ToString();
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable))
            {
                if (node.Method.Name == "Where")
                {
                    _queryBuilder.Append("WHERE ");
                    Visit(node.Arguments[1]);
                    return node;
                }
                if (node.Method.Name == "Select")
                {
                    var lambda = (LambdaExpression)((UnaryExpression)node.Arguments[1]).Operand;

                    var body = lambda.Body as NewExpression;
                    if (body != null)
                    {
                        foreach (var member in body.Members)
                        {
                            _selectFields.Add(member.Name);
                        }
                    }
                    else
                    {
                        _selectFields.Add(lambda.Body.ToString());
                    }

                    return node;
                }

                if (node.Method.Name == "Join")
                {
                    ProcessJoin(node);
                    return node;
                }
            }

            throw new NotSupportedException($"Метод {node.Method.Name} не поддерживается.");
        }

        private void ProcessJoin(MethodCallExpression node)
        {
            Expression outerKeySelector = node.Arguments[2];
            Expression innerKeySelector = node.Arguments[3];

            var outerKey = ExtractMemberName(outerKeySelector);
            var innerKey = ExtractMemberName(innerKeySelector);
            var joinCondition = $"{outerKey} = {innerKey}";

            _joins.Add($"JOIN {_tableName} ON {joinCondition}");
        }

        private string ExtractMemberName(Expression expression)
        {
            if (expression is UnaryExpression unaryExpression)
            {
                expression = unaryExpression.Operand;
            }

            if (expression is LambdaExpression lambdaExpression)
            {
                var body = lambdaExpression.Body;
                return ExtractMemberName(body);
            }

            if (expression is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }

            if (expression is ParameterExpression parameterExpression)
            {
                return parameterExpression.Name;
            }

            throw new NotSupportedException($"Неизвестный тип выражения: {expression.GetType().Name}");
        }
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            Visit(node.Body);
            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node.Left);
            _queryBuilder.Append($" {GetSqlOperator(node.NodeType)} ");
            Visit(node.Right);
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Type == typeof(string))
            {
                _queryBuilder.Append($"'{node.Value}'");
            }
            else if (node.Type.IsPrimitive || node.Type == typeof(decimal))
            {
                _queryBuilder.Append(node.Value);
            }
            else if (node.Value is IQueryable)
            {
                return node;
            }
            else
            {
                var queryableConstant = node.Value;
                _queryBuilder.Append(queryableConstant.ToString());
            }
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression is ConstantExpression constantExpression)
            {
                var fieldInfo = node.Member as System.Reflection.FieldInfo;
                var propertyInfo = node.Member as System.Reflection.PropertyInfo;

                object container = constantExpression.Value;
                object? value = fieldInfo?.GetValue(container) ?? propertyInfo?.GetValue(container);

                if (value is string)
                {
                    _queryBuilder.Append($"'{value}'");
                }
                else
                {
                    _queryBuilder.Append(value);
                }

                return node;
            }

            _queryBuilder.Append(node.Member.Name);
            return node;
        }

        private string GetSqlOperator(ExpressionType type)
        {
            return type switch
            {
                ExpressionType.Equal => "=",
                ExpressionType.GreaterThan => ">",
                ExpressionType.LessThan => "<",
                ExpressionType.AndAlso => "AND",
                ExpressionType.OrElse => "OR",
                _ => throw new NotSupportedException($"Оператор {type} не поддерживается.")
            };
        }
    }
}
