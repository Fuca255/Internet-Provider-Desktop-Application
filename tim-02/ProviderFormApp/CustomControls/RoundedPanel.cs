using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Provajder
{
    public class RoundedPanel : Panel
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            GraphicsPath path = new GraphicsPath();
            int radius = 60; // Prilagodite prema vlastitim preferencijama

            path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
            path.AddArc(this.Width - 2 * radius, 0, radius * 2, radius * 2, 270, 90);
            path.AddArc(this.Width - 2 * radius, this.Height - 2 * radius, radius * 2, radius * 2, 0, 90);
            path.AddArc(0, this.Height - 2 * radius, radius * 2, radius * 2, 90, 90);

            path.CloseFigure();
            this.Region = new Region(path);
        }
    }
}
