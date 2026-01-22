using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ponth
{
    public partial class LanguageSelectForm : Form
    {
        public LanguageSelectForm()
        {
            InitializeComponent();
        }

        private Desing d = new Desing();

        private List<Button> btnList;
        private Button selectedButton;

        private int langI;

        private void LanguageButton_Click(object sender, EventArgs e)
        {
            /*if (sender is not Button clickedBtn || clickedBtn.Tag == null)
                return;*/

            Button clickedBtn = (Button)sender; 

            // Nyelv index
            langI = Convert.ToInt32(clickedBtn.Tag);

            // Nyelv alkalmazása
            LanguageManager.ApplyCulture();

            // Előző gomb visszaállítása
            if (selectedButton != null)
            {
                selectedButton.Enabled = true;
                selectedButton.BackColor = SystemColors.Control;
                selectedButton.ForeColor = Color.Black;

            }

            // Új gomb kijelölése
            selectedBtnEdit(clickedBtn);

            selectedButton = clickedBtn;
        }

        private void selectedBtnEdit(Button btn)
        {
            btn.Enabled = false;
            btn.BackColor = Color.LightCyan;
            btn.ForeColor = Color.Gray;

        }


        private void LanguageSelectForm_Load(object sender, EventArgs e)
        {
            btnList = new List<Button>();
            btnList.Add(btnHUN);
            btnList.Add(btnEN);
            btnList.Add(btnDE);
            btnList.Add(btnFR);
            btnList.Add(btnES);

            for (int i = 0; i < LanguageManager.SupportedLanguages.Length; i++)
            {
                if (LanguageManager.SupportedLanguages[i].Code == LanguageManager.CurrentLanguage)
                {
                    langI = i;
                    break;
                }
            }

            d.MakeRoundedBtn(btnSave, 20);
            d.MakeRoundedBtn(btnCancel, 20);
            d.MakeRoundedBtn(btnHUN, 20);
            d.MakeRoundedBtn(btnEN, 20);
            d.MakeRoundedBtn(btnDE, 20);
            d.MakeRoundedBtn(btnFR, 20);
            d.MakeRoundedBtn(btnES, 20);

            selectedButton = btnList[langI];
            selectedBtnEdit(selectedButton);
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Nyelv váltása
            LanguageManager.ToggleLanguage(langI);
            LanguageManager.ApplyCulture();
        }
    }
}
