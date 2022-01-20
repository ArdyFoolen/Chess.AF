using AF.Functional;
using Chess.AF.ChessForm.Helpers;
using Chess.AF.Controllers;
using Chess.AF.ImportExport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AF.Functional.F;

namespace Chess.AF.ChessForm
{
    public partial class PgnDialog : Form
    {
        private IGameController gameController;
        private IPgnController pgnController;

        public Option<Pgn> Pgn { get; private set; } = None;
        private ContextMenuStrip popupMenu { get; }

        public PgnDialog(IGameController gameController, IPgnController pgnController)
        {
            InitializeComponent();

            this.popupMenu = ControlsFactory.CreateComboPopupMenu(new EventHandler(this.deleteToolStripMenuItem_Click));

            this.gameController = gameController;
            this.pgnController = pgnController;

            ReadHistory();

            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOpenFileDialog_Click(object sender, EventArgs e)
        {
            this.openFileDialog1 = new OpenFileDialog();
            this.openFileDialog1.Filter = "pgn files (*.pgn)|*.pgn|All files (*.*)|*.*";
            this.openFileDialog1.FilterIndex = 0;
            this.openFileDialog1.Title = "Browse Portable Game Notation files";
            var result = this.openFileDialog1.ShowDialog();
            if (DialogResult.OK.Equals(result))
                AddToHistory(this.openFileDialog1.FileName);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (cmbHistory.SelectedItem == null)
            {
                MessageBox.Show("No file path selected", "Import Pgn file to Game", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }

            if (ValidatePath(cmbHistory.SelectedItem.ToString()))
            {
                Pgn = this.pgnController.Read(cmbHistory.SelectedItem.ToString());
                this.gameController.SetFromPgn(Pgn);

                this.DialogResult = DialogResult.OK;
                Close();
                return;
            }
        }

        private void btnExportNew_Click(object sender, EventArgs e)
        {
            if (!trySetAndValidateHistory())
                return;

            writeOrAddPgn((p, f) => this.pgnController.Write(p, f));
        }

        private void btnExportAdd_Click(object sender, EventArgs e)
        {
            if (!trySetAndValidateHistory())
                return;

            writeOrAddPgn((p, f) => this.pgnController.WriteAndAdd(p, f));
        }

        private void writeOrAddPgn(Action<Option<Pgn>, string> writeOrAdd)
        {
            this.saveFileDialog1 = new SaveFileDialog();
            this.saveFileDialog1.Filter = "pgn files (*.pgn)|*.pgn|All files (*.*)|*.*";
            this.saveFileDialog1.FilterIndex = 0;
            this.saveFileDialog1.Title = "Save Portable Game Notation file";
            this.saveFileDialog1.FileName = cmbHistory.SelectedItem.ToString();
            var result = this.saveFileDialog1.ShowDialog();
            if (DialogResult.OK.Equals(result))
            {
                var pgn = this.gameController.Export();
                writeOrAdd(pgn, this.saveFileDialog1.FileName);
                AddToHistory(this.saveFileDialog1.FileName);
            }

            this.DialogResult = DialogResult.Yes;
            Close();
        }

        private bool trySetAndValidateHistory()
        {
            setHistoryFromTextIfNotSet();
            return ValidateHistory();
        }

        private bool ValidateHistory()
        {
            if (cmbHistory.SelectedItem == null)
            {
                MessageBox.Show("No file path selected", "Export Game to Pgn file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return false;
            }
            if (ValidatePath(cmbHistory.SelectedItem.ToString()))
                return true;
            return false;
        }

        private void setHistoryFromTextIfNotSet()
        {
            if (cmbHistory.SelectedItem == null)
                cmbHistory.SelectedItem = cmbHistory.Text;
        }

        private void cmbHistory_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddToHistory(cmbHistory.Text);
                cmbHistory.SelectedItem = cmbHistory.Text;
            }
            else Application.DoEvents();
        }
        private void AddToHistory(string path)
        {
            if (ValidatePath(path))
            {
                if (!cmbHistory.Items.Contains(path))
                {
                    cmbHistory.Items.Add(path);
                    SaveHistory();
                }
                cmbHistory.SelectedItem = path;
            }
        }

        private bool ValidatePath(string path)
            => Directory.Exists(Path.GetDirectoryName(path));

        private void SaveHistory()
        {
            try
            {
                File.WriteAllLines(path, cmbHistory.Items.Cast<string>());
            }
            catch (Exception) { }
        }

        private void ReadHistory()
        {
            try
            {
                cmbHistory.Items.AddRange(File.ReadAllLines(path));
            }
            catch (Exception) { }
        }

        private string path { get { return $"{Environment.CurrentDirectory}\\PgnFilePathHistory.txt"; } }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setHistoryFromTextIfNotSet();
            if (cmbHistory.SelectedItem == null)
                return;

            cmbHistory.Items.Remove(cmbHistory.SelectedItem);
            SaveHistory();
            cmbHistory.SelectedItem = null;
        }

        private void cmbHistory_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
            {
                cmbHistory.ContextMenuStrip = popupMenu;
                popupMenu.Show();
            }
        }
    }
}
