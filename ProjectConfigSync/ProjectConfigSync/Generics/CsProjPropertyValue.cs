using System.Xml.Linq;

namespace ProjectConfigSync.Generics
{
    public class CsProjPropertyValue
    {
        private readonly XElement _element = null;

        public CsProjPropertyValue(XElement element, object replacementValue = null)
        {
            _element = element;

            if (replacementValue != null && _element != null &&
                ((replacementValue is string && _element.Value != replacementValue.ToString().TrimEnd()) ||
                (!(replacementValue is string) && _element.Value.ToLower() != replacementValue.ToString().TrimEnd().ToLower())))
            {
                _element.SetValue(replacementValue.ToString().TrimEnd());
            }
        }

        public bool IsValueExplicitlySet
        {
            get
            {
                return (_element != null);
            }
        }

        public string Value
        {
            get
            {
                if (!IsValueExplicitlySet)
                {
                    return null;
                }

                return string.IsNullOrEmpty(this._element.Value)
                    ? string.Empty
                    : this._element.Value.TrimEnd();
            }
        }

        public override string ToString()
        {
            return Value;
        }
    }
}