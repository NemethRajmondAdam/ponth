using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ponth.CostumeControls
{
    public class cDropDown : Form
    {
        public event Action<string> ItemSelected;

        public cDropDown(List<string> items)
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            BackColor = Color.White;
            AutoSize = true;

            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.FlowDirection = FlowDirection.LeftToRight;
            panel.WrapContents = true;         
            panel.AutoSize = true;
            panel.MaximumSize = new Size(400, 0);
            panel.Padding = new Padding(5);

            foreach (var item in items)
            {
                cButtons btn = new cButtons();
                btn.Text = item;
                btn.Size = new Size(100, 100);
                btn.ForeColor = Color.White;
                btn.Margin = new Padding(5);
                btn.Click += (s, e) =>
                {
                    ItemSelected?.Invoke(item);
                    this.Close();
                };

                panel.Controls.Add(btn);
            }

            Controls.Add(panel);
        }
    }

}
