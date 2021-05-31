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

        /// <summary>
        /// Default "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
        /// 1) Pieces PNBRQK Capitol is white other black
        ///    Rows separated by / and empty squares by nbr from 1-8
        /// 2) Who to move w or b
        /// 3) Rokade No rokade -, or K/k white/black kingside, Q/q White/Black queenside
        /// 4) En Passant square
        /// 5) Moves
        /// 6) Ply-count/Half-moves
        /// </summary>
        public string Fen { get; private set; }

        private string GetFen()
            => string.Join(" ", txtFen1.Text, txtFen2.Text, txtFen3.Text, txtFen4.Text, txtFen5.Text, txtFen6.Text);

        private bool ValidateFen()
            => GetFen().CreateFen().Match(
                None: () => false,
                Some: s => true
                );

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateFen())
            {
                Fen = GetFen();
                this.DialogResult = DialogResult.OK;
                Close();
                return;
            }

            MessageBox.Show($"Fen '{txtFen1.Text}' NOT valid", "Invalid Fen", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
