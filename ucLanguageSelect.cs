using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ponth
{
    public partial class ucLanguageSelect : UserControl
    {
        private Desing d = new Desing();

        private List<Button> btnList;
        private Button selectedButton;

        private int langI;

        public ucLanguageSelect()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            btnDE.Click += LanguageButton_Click;
            btnEN.Click += LanguageButton_Click;
            btnES.Click += LanguageButton_Click;
            btnFR.Click += LanguageButton_Click;
            btnHUN.Click += LanguageButton_Click;

            btnList = new List<Button>
            {
                btnHUN,
                btnEN,
                btnDE,
                btnFR,
                btnES
            };

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

        private void LanguageButton_Click(object sender, EventArgs e)
        {
            Button clickedBtn = (Button)sender;

            langI = Convert.ToInt32(clickedBtn.Tag);

            LanguageManager.ApplyCulture();

            if (selectedButton != null)
            {
                selectedButton.Enabled = true;
                selectedButton.BackColor = SystemColors.Control;
                selectedButton.ForeColor = Color.Black;
            }

            selectedBtnEdit(clickedBtn);
            selectedButton = clickedBtn;

            // ✅ NYELV KIVÁLASZTÁS JELZÉSE (LIVE)
            OnLanguageSelected?.Invoke(langI);
        }

        private void selectedBtnEdit(Button btn)
        {
            btn.Enabled = false;
            btn.BackColor = Color.LightCyan;
            btn.ForeColor = Color.Gray;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            LanguageManager.ToggleLanguage(langI);
            LanguageManager.ApplyCulture();

            // ✅ végleges nyelv jelzése
            OnLanguageChanged?.Invoke(langI);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancel?.Invoke();
        }

        // ✅ EVENTEK MAINFORM FELÉ
        public event Action<int> OnLanguageSelected;   // preview
        public event Action<int> OnLanguageChanged;    // save
        public event Action OnCancel;
    }
}
