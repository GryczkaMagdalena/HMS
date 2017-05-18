using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HMS.Tests.Infrastructure
{
    class DynamicObjectResultValue : DynamicObject, IEquatable<DynamicObjectResultValue>
    {
        private readonly object value;

        public DynamicObjectResultValue(object value)
        {
            this.value = value;
        }

        public static bool operator == (DynamicObjectResultValue a,DynamicObjectResultValue b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if(ReferenceEquals((object)a,null)|| ReferenceEquals((object)b, null))
            {
                return false;
            }
            return a.value == b.value;
        }

        public static bool operator !=(DynamicObjectResultValue a,DynamicObjectResultValue b)
        {
            return !(a == b);
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return value.GetType().GetProperties().Select(p => p.Name);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            var property = value.GetType().GetProperty(binder.Name);
            if (property != null)
            {
                var propertyValue = property.GetValue(value, null);
                result = propertyValue;

                return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if(obj is DynamicObjectResultValue)
            {
                return Equals(obj as DynamicObjectResultValue);
            }
            if (ReferenceEquals(obj, null)) return false;
            return this.value == obj;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
        public override string ToString()
        {
            return string.Format("{0}", value);
        }

        public bool Equals(DynamicObjectResultValue other)
        {
            if (ReferenceEquals(other, null)) return false;
            return this.value == other.value;
        }
    }
}
