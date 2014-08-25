using System;
using System.Windows.Forms;

namespace ProjectConfigSync.Controls
{
    public class NestedDataGridViewColumn : DataGridViewColumn
    {
        public NestedDataGridViewColumn()
            : base(new NestedDataGridViewCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(NestedDataGridViewCell)))
                {
                    throw new InvalidCastException("Must be a NestedDataGridViewCell");
                }

                base.CellTemplate = value;
            }
        }
    }
}
