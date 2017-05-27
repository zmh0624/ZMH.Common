using FastMember;
using System;
using System.Linq.Expressions;

namespace Ecis.Common.Extension
{
    public static class FastMemberEx
    {
        public static void AssignValueToProperty(this ObjectAccessor accessor, string propertyName, object value)
        {
            var targetType = Expression.Parameter(accessor.Target.GetType());
            var property = Expression.Property(targetType, propertyName);

            var type = property.Type;
            type = Nullable.GetUnderlyingType(type) ?? type;
            value = value == null ? GetDefault(type) : Convert.ChangeType(value, type);
            accessor[propertyName] = value;
        }

        private static object GetDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static object GetProperties<T>(this T t, string name)
        {
            ObjectAccessor accessor = ObjectAccessor.Create(t);
            return accessor[name];
        }
    }
}