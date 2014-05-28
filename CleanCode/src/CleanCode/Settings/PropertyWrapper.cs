using System;
using System.Linq.Expressions;

namespace CleanCode.Settings
{
    public class PropertyWrapper<TOwner, TPropertyType>
    {
        private readonly TOwner owner;
        private readonly Func<TOwner, TPropertyType> getter;
        private readonly Action<TOwner, TPropertyType> setter;

        public PropertyWrapper(TOwner owner, Expression<Func<TOwner, TPropertyType>> selector)
        {
            this.owner = owner;
            var newValue = Expression.Parameter(selector.Body.Type);
            var assign = Expression.Lambda<Action<TOwner, TPropertyType>>(
                Expression.Assign(selector.Body, newValue),
                selector.Parameters[0], newValue);

            getter = selector.Compile();
            setter = assign.Compile();
        }

        public void SetValue(TPropertyType value)
        {
            setter(owner, value);
        }

        public TPropertyType GetValue()
        {
            return getter(owner);
        }

    }
}