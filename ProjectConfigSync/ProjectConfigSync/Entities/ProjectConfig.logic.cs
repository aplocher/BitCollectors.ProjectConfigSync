
using System.Linq;
using ProjectConfigSync.Attributes;

namespace ProjectConfigSync.Entities
{
    public partial class ProjectConfig
    {
        public object GetValueFromOrdinal(int ordinal)
        {
            var propertyInfo = this.GetType()
                .GetProperties().FirstOrDefault(x =>
                    x.GetCustomAttributes(typeof(PropertyOrdinalAttribute), false).Cast<PropertyOrdinalAttribute>().Any(attribute =>
                        attribute.OrdinalPosition == ordinal));

            if (propertyInfo != null)
            {
                var value = propertyInfo.GetValue(this, null);
                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }

        public void SetValueFromOrdinal(int ordinal, object value)
        {
            var propertyInfo = this.GetType()
                .GetProperties().FirstOrDefault(x =>
                    x.GetCustomAttributes(typeof(PropertyOrdinalAttribute), false).Cast<PropertyOrdinalAttribute>().Any(attribute =>
                        attribute.OrdinalPosition == ordinal));

            if (propertyInfo != null)
            {
                propertyInfo.SetValue(this, value, null);
            }
        }
    }
}
