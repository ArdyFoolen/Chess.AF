using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess.AF.ChessForm
{
    public partial class LoadFen : Form
    {
        public LoadFen()
        {
            InitializeComponent();
        }

        public string Fen { get; private set; }

        private bool ValidateFen()
            => txtFen.Text.CreateFen().Match(
                None: () => false,
                Some: s => true
                );

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateFen())
            {
                Fen = txtFen.Text;
                this.DialogResult = DialogResult.OK;
                Close();
                return;
            }

            MessageBox.Show($"Fen '{txtFen.Text}' NOT valid", "Invalid Fen", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void LoadFen_Load(object sender, EventArgs e)
        {
            this.Fen = null;
        }
    }
}
