using System;

namespace ProjectConfigSync.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PropertyOrdinalAttribute : Attribute
    {
        public int OrdinalPosition { get; private set; }

        public PropertyOrdinalAttribute(int ordinalPosition)
        {
            this.OrdinalPosition = ordinalPosition;
        }
    }
}
