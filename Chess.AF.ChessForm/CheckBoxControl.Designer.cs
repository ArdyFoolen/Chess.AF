
namespace Chess.AF.ChessForm
{
    partial class CheckBoxControl
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
            this.lblDescription = new System.Windows.Forms.Label();
            this.chkBoth = new System.Windows.Forms.CheckBox();
            this.chkWhite = new System.Windows.Forms.CheckBox();
            this.chkBlack = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(0, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(38, 15);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "label1";
            // 
            // chkBoth
            // 
            this.chkBoth.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkBoth.AutoSize = true;
            this.chkBoth.Location = new System.Drawing.Point(45, 4);
            this.chkBoth.Name = "chkBoth";
            this.chkBoth.Size = new System.Drawing.Size(6, 6);
            this.chkBoth.TabIndex = 1;
            this.chkBoth.UseVisualStyleBackColor = true;
            // 
            // chkWhite
            // 
            this.chkWhite.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkWhite.AutoSize = true;
            this.chkWhite.Location = new System.Drawing.Point(58, 4);
            this.chkWhite.Name = "chkWhite";
            this.chkWhite.Size = new System.Drawing.Size(6, 6);
            this.chkWhite.TabIndex = 2;
            this.chkWhite.UseVisualStyleBackColor = true;
            // 
            // chkBlack
            // 
            this.chkBlack.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkBlack.AutoSize = true;
            this.chkBlack.Location = new System.Drawing.Point(70, -1);
            this.chkBlack.Name = "chkBlack";
            this.chkBlack.Size = new System.Drawing.Size(6, 6);
            this.chkBlack.TabIndex = 3;
            this.chkBlack.UseVisualStyleBackColor = true;
            // 
            // CheckBoxControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkBlack);
            this.Controls.Add(this.chkWhite);
            this.Controls.Add(this.chkBoth);
            this.Controls.Add(this.lblDescription);
            this.Name = "CheckBoxControl";
            this.Size = new System.Drawing.Size(228, 33);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.CheckBox chkBoth;
        private System.Windows.Forms.CheckBox chkWhite;
        private System.Windows.Forms.CheckBox chkBlack;
    }
}
