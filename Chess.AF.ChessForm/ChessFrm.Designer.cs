
using System;
using System.Windows.Forms;

namespace Chess.AF.ChessForm
{
    partial class ChessFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnLoadFen = new System.Windows.Forms.ToolStripButton();
            this.btnLoadPgn = new System.Windows.Forms.ToolStripButton();
            this.btnFirstMove = new System.Windows.Forms.ToolStripButton();
            this.btnPreviousMove = new System.Windows.Forms.ToolStripButton();
            this.btnNextMove = new System.Windows.Forms.ToolStripButton();
            this.btnLastMove = new System.Windows.Forms.ToolStripButton();
            this.btnReverseBoard = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLoadFen,
            this.btnLoadPgn,
            this.btnFirstMove,
            this.btnPreviousMove,
            this.btnNextMove,
            this.btnLastMove,
            this.btnReverseBoard});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnLoadFen
            // 
            this.btnLoadFen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLoadFen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLoadFen.Name = "btnLoadFen";
            this.btnLoadFen.Size = new System.Drawing.Size(23, 22);
            this.btnLoadFen.Text = "Load Fen";
            this.btnLoadFen.Click += BtnLoadFen_Click;
            // 
            // btnLoadPgn
            // 
            this.btnLoadPgn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLoadPgn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLoadPgn.Name = "btnLoadPgn";
            this.btnLoadPgn.Size = new System.Drawing.Size(23, 22);
            this.btnLoadPgn.Text = "Load Pgn";
            this.btnLoadPgn.Click += BtnLoadPgn_Click;
            // 
            // btnFirstMove
            // 
            this.btnFirstMove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFirstMove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFirstMove.Name = "btnFirstMove";
            this.btnFirstMove.Size = new System.Drawing.Size(23, 22);
            this.btnFirstMove.Text = "Goto First Move";
            this.btnFirstMove.Click += BtnFirstMove_Click;
            // 
            // btnPreviousMove
            // 
            this.btnPreviousMove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPreviousMove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPreviousMove.Name = "btnPreviousMove";
            this.btnPreviousMove.Size = new System.Drawing.Size(23, 22);
            this.btnPreviousMove.Text = "Goto Previous Move";
            this.btnPreviousMove.Click += BtnPreviousMove_Click;
            // 
            // btnNextMove
            // 
            this.btnNextMove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNextMove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNextMove.Name = "btnNextMove";
            this.btnNextMove.Size = new System.Drawing.Size(23, 22);
            this.btnNextMove.Text = "Goto Next Move";
            this.btnNextMove.Click += BtnNextMove_Click;
            // 
            // btnLastMove
            // 
            this.btnLastMove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLastMove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLastMove.Name = "btnLastMove";
            this.btnLastMove.Size = new System.Drawing.Size(23, 22);
            this.btnLastMove.Text = "Goto Last Move";
            this.btnLastMove.Click += BtnLastMove_Click;
            // 
            // btnReverseBoard
            // 
            this.btnReverseBoard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnReverseBoard.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReverseBoard.Name = "btnReverseBoard";
            this.btnReverseBoard.Size = new System.Drawing.Size(23, 22);
            this.btnReverseBoard.Text = "Reverse Board";
            this.btnReverseBoard.Click += btnReverseBoard_Click;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ChessFrm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 594);
            this.Controls.Add(this.toolStrip1);
            this.MaximumSize = new System.Drawing.Size(816, 633);
            this.MinimumSize = new System.Drawing.Size(816, 633);
            this.Name = "ChessFrm";
            this.Text = "Chess Form";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnLoadFen;
        private System.Windows.Forms.ToolStripButton btnLoadPgn;
        private System.Windows.Forms.ToolStripButton btnFirstMove;
        private System.Windows.Forms.ToolStripButton btnPreviousMove;
        private System.Windows.Forms.ToolStripButton btnNextMove;
        private System.Windows.Forms.ToolStripButton btnLastMove;
        private System.Windows.Forms.ToolStripButton btnReverseBoard;
        private OpenFileDialog openFileDialog1;
    }
}

