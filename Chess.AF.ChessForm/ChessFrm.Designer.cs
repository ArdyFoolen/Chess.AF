
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
            //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChessFrm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnLoadFen = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLoadFen});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 31);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.btnLoadFen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            //this.btnLoadFen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.btnLoadFen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLoadFen.Name = "btnLoadFen";
            this.btnLoadFen.Size = new System.Drawing.Size(28, 28);
            this.btnLoadFen.Text = "Load Fen";
            this.btnLoadFen.Click += BtnLoadFen_Click;
            // 
            // ChessFrm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStrip1);
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
    }
}

