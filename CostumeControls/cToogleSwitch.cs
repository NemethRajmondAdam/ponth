using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace ponth.CostumeControls
{
    internal class cToogleSwitch
    {
        public class RJToggleButton : CheckBox
        {

            //Fields
            private Color onBackColor = Color.MediumSlateBlue;
            private Color onToggleColor = Color.WhiteSmoke;
            private Color offBackColor = Color.Gray;
            private Color offToggleColor = Color.Gainsboro;
            private bool solidStyle = true;

            //Constructor
            public RJToggleButton()
            {
                this.MinimumSize = new Size(45, 22);
            }

            //Methods
            private GraphicsPath GetFigurePath()
            {
                int arcSize = this.Height - 1;
                Rectangle leftArc = new Rectangle(0, 0, arcSize, arcSize);
                Rectangle rightArc = new Rectangle(this.Width - arcSize - 2, 0, arcSize, arcSize);
                GraphicsPath path = new GraphicsPath();
                path.StartFigure();
                path.AddArc(leftArc, 90, 180);
                path.AddArc(rightArc, 270, 180);
                path.CloseFigure();
                return path;
            }
        }
    }
}
