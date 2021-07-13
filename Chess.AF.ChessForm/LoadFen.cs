using Chess.AF.ChessForm.Helpers;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Chess.AF.ChessForm.ResourceHelper;

namespace Chess.AF.ChessForm
{
    public partial class LoadFen : Form
    {
        private ContextMenuStrip popupMenu { get; }

        public LoadFen()
        {
            InitializeComponent();

            this.popupMenu = ControlsFactory.CreateComboPopupMenu(new EventHandler(this.deleteToolStripMenuItem_Click));

            var squares = Enum.GetNames(typeof(SquareEnum)).ToList();
            squares.Insert(0, "-");
            cmbFen3.DataSource = squares;

            ReadHistory();
            AddToHistory("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

            toolTip1.SetToolTip(txtFen1, TooltipFen1);
            toolTip2.SetToolTip(cmbFen1, TooltipFen2);
            toolTip3.SetToolTip(cmbFen2, TooltipFen3);
            toolTip4.SetToolTip(cmbFen3, TooltipFen4);
            toolTip5.SetToolTip(nbrFen1, TooltipFen5);
            toolTip6.SetToolTip(nbrFen2, TooltipFen6);
            toolTip7.SetToolTip(cmbFenHistory, TooltipFenHistory);

            this.StartPosition = FormStartPosition.CenterParent;
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
            => string.Join(" ", txtFen1.Text, cmbFen1.SelectedItem.ToString(), cmbFen2.SelectedItem.ToString(), cmbFen3.SelectedItem.ToString(),
                nbrFen1.Value.ToString(), nbrFen2.Value.ToString());

        private bool ValidateFen(string fen)
            => fen.CreateFen().Match(
                None: () => false,
                Some: s => true
                );

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateFen(GetFen()))
            {
                Fen = GetFen();
                AddToHistory(Fen);
                this.DialogResult = DialogResult.OK;
                Close();
                return;
            }

            MessageBox.Show($"Fen '{txtFen1.Text}' NOT valid", "Invalid Fen", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void AddToHistory(string fen)
        {
            if (!cmbFenHistory.Items.Contains(fen) && ValidateFen(fen))
            {
                cmbFenHistory.Items.Add(fen);
                SaveHistory();
            }
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

        private void cmbFenHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFenHistory.SelectedItem == null)
                return;

            var splits = cmbFenHistory.SelectedItem.ToString().Split(' ');
            if (splits.Length != 6)
                return;
            txtFen1.Text = splits[0];
            cmbFen1.SelectedItem = splits[1];
            cmbFen2.SelectedItem = splits[2];
            cmbFen3.SelectedItem = splits[3];
            nbrFen1.Value = int.Parse(splits[4]);
            nbrFen2.Value = int.Parse(splits[5]);
        }

        private void cmbFenHistory_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var splits = cmbFenHistory.Text.Split(' ');
                if (splits.Length != 6 || string.IsNullOrWhiteSpace(splits[5]))
                    return;

                AddToHistory(cmbFenHistory.Text);
                cmbFenHistory.SelectedItem = cmbFenHistory.Text;
            }
            else Application.DoEvents();
        }

        private void SaveHistory()
        {
            try
            {
                File.WriteAllLines(path, cmbFenHistory.Items.Cast<string>());
            }
            catch (Exception) { }
        }

        private void ReadHistory()
        {
            try
            {
                cmbFenHistory.Items.AddRange(File.ReadAllLines(path));
            }
            catch (Exception) { }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cmbFenHistory.SelectedItem == null)
                return;

            cmbFenHistory.Items.Remove(cmbFenHistory.SelectedItem);
            SaveHistory();
            cmbFenHistory.SelectedItem = null;
        }

        private string path { get { return $"{Environment.CurrentDirectory}\\FenHistory.txt"; } }

        private void cmbFenHistory_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
            {
                cmbFenHistory.ContextMenuStrip = popupMenu;
                popupMenu.Show();
            }
        }
    }
}
