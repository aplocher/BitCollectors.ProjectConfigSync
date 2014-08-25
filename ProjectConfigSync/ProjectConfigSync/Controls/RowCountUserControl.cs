using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectConfigSync.Controls
{
    public partial class RowCountUserControl : UserControl
    {
        private readonly Pen _borderPen;
        //private Timer _mouseStatusTimer = new Timer();

        public RowCountUserControl()
        {
            InitializeComponent();

            this._borderPen = new Pen(Brushes.LightSlateGray, 8);

            //_mouseStatusTimer.Interval = 250;
            //_mouseStatusTimer.Tick += MouseStatusTimerTick;
        }

        //protected override void OnVisibleChanged(EventArgs e)
        //{
        //    base.OnVisibleChanged(e);

        //    if (this.Visible)
        //    {
        //        foreach (var control in this.Controls)
        //        {
        //            if (control is Panel)
        //            {
        //                Panel panel = control as Panel;
        //                panel.BackColor = Color.White;
        //            }
        //        }

        //        _mouseStatusTimer.Start();
        //    }
        //    else
        //    {
        //        _mouseStatusTimer.Stop();
        //    }
        //}

        //private void MouseStatusTimerTick(object sender, EventArgs e)
        //{

        //    foreach (Control control in this.Controls)
        //    {
        //        if (control is Panel)
        //        {
        //            var panel = control as Panel;

        //            panel.BackColor =
        //                panel.ClientRectangle.Contains(panel.PointToClient(Cursor.Position))
        //                ? Color.SkyBlue
        //                : Color.White;
        //        }
        //    }
        //}

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //foreach (Control control in this.Controls)
            //{
            //    if (control is Panel)
            //    {
            //        control.MouseEnter += this.PanelMouseEnter;
            //        control.MouseLeave += this.PanelMouseLeave;

            //        foreach (Control panelChildren in control.Controls)
            //        {
            //            panelChildren.MouseEnter += this.PanelMouseEnter;
            //            panelChildren.MouseLeave += this.PanelMouseLeave;
            //        }
            //    }
            //}
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            e.Graphics.DrawRectangle(_borderPen, this.ClientRectangle);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (this.ClientRectangle.Contains(this.PointToClient(Control.MousePosition)))
            {
                return;
            }

            base.OnMouseLeave(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        //private void PanelMouseEnter(object sender, EventArgs e)
        //{
        //    Panel panel = sender as Panel;
        //    if (sender is Panel)
        //    {
        //        panel = sender as Panel;
        //    }
        //    else if (sender is Control && ((Control)sender).Parent is Panel)
        //    {
        //        panel = ((Control)sender).Parent as Panel;
        //    }

        //    panel.BackColor = Color.SkyBlue;
        //}



        private void PanelMouseLeave(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            var mousePosition = Cursor.Position;

            if (sender is Panel)
            {
                panel = sender as Panel;
            }
            else if (sender is Control && ((Control)sender).Parent is Panel)
            {
                panel = ((Control)sender).Parent as Panel;
            }

            if (!panel.ClientRectangle.Contains(panel.PointToClient(mousePosition)))
            {
                return;
            }

            panel.BackColor = Color.White;
        }

        private void HighlightPanel(Panel panel)
        {
            panel.BackColor = Color.SkyBlue;
        }
    }
}
