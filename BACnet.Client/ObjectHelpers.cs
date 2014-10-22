using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Types;

namespace BACnet.Client
{
    public static class ObjectHelpers
    {
        public static PropertyIdentifier GetPropertyIdentifier(Expression expr)
        {
            if (expr.NodeType != ExpressionType.MemberAccess)
                throw new Exception();
            MemberExpression member = (MemberExpression)expr;
            if (member.Member.MemberType != MemberTypes.Property)
                throw new Exception();
            PropertyAttribute attr = (PropertyAttribute)member.Member.GetCustomAttribute(typeof(PropertyAttribute));
            return (PropertyIdentifier)attr.PropertyIdentifier;
        }

        public static PropertyReference GetPropertyReference<TObj, TProp>(Expression<Func<TObj, TProp>> expression)
        {
            var body = expression.Body;
            PropertyIdentifier propertyIdentifier;
            Option<uint> propertyArrayIndex = default(Option<uint>);

            if(body.NodeType == ExpressionType.Index)
            {
                var indexExpr = (IndexExpression)body;
                var paramExpr = indexExpr.Arguments[0];

                if(paramExpr.NodeType != ExpressionType.Constant)
                    throw new Exception();

                ConstantExpression constExpr = (ConstantExpression)paramExpr;
                propertyIdentifier = GetPropertyIdentifier(indexExpr.Object);
                propertyArrayIndex = Convert.ToUInt32(constExpr.Value) + 1;
            }
            else if(body.NodeType == ExpressionType.MemberAccess)
            {
                propertyIdentifier = GetPropertyIdentifier(body);
            }
            else
                throw new Exception();

            return new PropertyReference(propertyIdentifier, propertyArrayIndex);
        }

        public static ObjectPropertyReference GetObjectPropertyReference<TObj, TProp>(ObjectId objectIdentifier, Expression<Func<TObj, TProp>> expression)
        {
            var prop = GetPropertyReference(expression);

            return new ObjectPropertyReference(
                objectIdentifier,
                prop.PropertyIdentifier,
                prop.PropertyArrayIndex);
        }
    }
}
