﻿
namespace Chess.AF.ChessForm.Controls
{
    partial class BaseSquareControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnImage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnImage
            // 
            this.btnImage.BackColor = System.Drawing.Color.Transparent;
            this.btnImage.FlatAppearance.BorderSize = 0;
            this.btnImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImage.Location = new System.Drawing.Point(0, 0);
            this.btnImage.Margin = new System.Windows.Forms.Padding(0);
            this.btnImage.Name = "btnImage";
            this.btnImage.Size = new System.Drawing.Size(70, 70);
            this.btnImage.TabIndex = 0;
            this.btnImage.UseVisualStyleBackColor = false;
            this.btnImage.Paint += new System.Windows.Forms.PaintEventHandler(this.btnImage_Paint);
            this.btnImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnImage_MouseClick);
            this.btnImage.MouseEnter += new System.EventHandler(this.btnImage_MouseEnter);
            this.btnImage.MouseLeave += new System.EventHandler(this.btnImage_MouseLeave);
            // 
            // BaseSquareControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnImage);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "BaseSquareControl";
            this.Size = new System.Drawing.Size(62, 60);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnImage;
    }
}
