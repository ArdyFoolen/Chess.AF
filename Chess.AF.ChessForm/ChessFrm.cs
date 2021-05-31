using Chess.AF.ChessForm.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Chess.AF.ChessForm.ChessConstants;
using static Chess.AF.ChessForm.ImageHelper;

namespace Chess.AF.ChessForm
{
    public partial class ChessFrm : Form
    {
        private BoardControl boardControl;
        public ChessFrm()
        {
            InitializeComponent();

            this.loadFen.Hide();
            this.Size = new Size(this.Size.Width, FormHeight);

            this.boardControl = new BoardControl(new BoardController());
            this.boardControl.BackColor = Color.SaddleBrown;
            this.boardControl.Size = new Size(BoardWidth, BoardWidth);
            this.boardControl.Margin = new Padding(0);
            this.boardControl.Location = new Point(0, 33);
            this.boardControl.BorderStyle = BorderStyle.FixedSingle;

            this.BackColor = Color.Wheat;
            //Button button = new Button();
            //button.Image = BlackKing();
            //button.Image = BlackKing();

            //button.Location = new Point(600, 50);
            //button.Size = new Size(500 / 6, 90);
            
            //frmLocation.Location = new Point(600, 35);
            //frmLocation.Text = $"Form location: {this.Location.X}:{this.Location.Y}";
            //this.Controls.Add(frmLocation);

            //ptToScreen.Location = new Point(600, 70);
            //var loc = this.PointToScreen(this.Location);
            //ptToScreen.Text = $"Form location: {loc.X}:{loc.Y}";
            //this.Controls.Add(ptToScreen);

            //PromoteControl promoteBlack = new PromoteControl(true);
            //promoteBlack.Location = new Point(600, 100);
            //promoteBlack.Size = new Size(70, 80);

            //PromoteControl promoteWhite = new PromoteControl(false);
            //promoteWhite.Location = new Point(600, 200);
            //promoteWhite.Size = new Size(70, 80);

            this.Controls.Add(this.boardControl);
            //this.Controls.Add(button);
            //this.Controls.Add(promoteBlack);
            //this.Controls.Add(promoteWhite);

            this.btnLoadFen.Image = Fen();
        }

        //Label frmLocation = new Label();
        //Label ptToScreen = new Label();
        LoadFen loadFen = new LoadFen();

        private void BtnLoadFen_Click(object sender, EventArgs e)
        {
            var result = this.loadFen.ShowDialog();
            if (DialogResult.OK.Equals(result))
                this.boardControl.LoadFen(this.loadFen.Fen);
            if (DialogResult.Yes.Equals(result))
                this.boardControl.LoadFen();
            //this.loadFen.Hide();
            //frmLocation.Text = $"Form location: {this.Location.X}:{this.Location.Y}";

            //var loc = this.PointToScreen(this.Location);
            //ptToScreen.Text = $"Form location: {loc.X}:{loc.Y}";

            //MessageBox.Show($"Fen: {boardControl.ToFenString()}");
            //MessageBox.Show($"Size: {Size.Width}:{Size.Height}");
        }
    }
}
