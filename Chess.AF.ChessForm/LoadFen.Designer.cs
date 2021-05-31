
namespace Chess.AF.ChessForm
{
    partial class LoadFen
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
            this.lblFen = new System.Windows.Forms.Label();
            this.txtFen1 = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDefault = new System.Windows.Forms.Button();
            this.txtFen2 = new System.Windows.Forms.TextBox();
            this.txtFen3 = new System.Windows.Forms.TextBox();
            this.txtFen4 = new System.Windows.Forms.TextBox();
            this.txtFen5 = new System.Windows.Forms.TextBox();
            this.txtFen6 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblFen
            // 
            this.lblFen.AutoSize = true;
            this.lblFen.Location = new System.Drawing.Point(13, 15);
            this.lblFen.Name = "lblFen";
            this.lblFen.Size = new System.Drawing.Size(26, 15);
            this.lblFen.TabIndex = 0;
            this.lblFen.Text = "Fen";
            // 
            // txtFen1
            // 
            this.txtFen1.Location = new System.Drawing.Point(104, 12);
            this.txtFen1.Name = "txtFen1";
            this.txtFen1.Size = new System.Drawing.Size(170, 23);
            this.txtFen1.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(13, 57);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(199, 57);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDefault
            // 
            this.btnDefault.Location = new System.Drawing.Point(104, 57);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(75, 23);
            this.btnDefault.TabIndex = 4;
            this.btnDefault.Text = "Default";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // txtFen2
            // 
            this.txtFen2.Location = new System.Drawing.Point(280, 12);
            this.txtFen2.Name = "txtFen2";
            this.txtFen2.Size = new System.Drawing.Size(27, 23);
            this.txtFen2.TabIndex = 5;
            // 
            // txtFen3
            // 
            this.txtFen3.Location = new System.Drawing.Point(313, 12);
            this.txtFen3.Name = "txtFen3";
            this.txtFen3.Size = new System.Drawing.Size(42, 23);
            this.txtFen3.TabIndex = 6;
            // 
            // txtFen4
            // 
            this.txtFen4.Location = new System.Drawing.Point(361, 12);
            this.txtFen4.Name = "txtFen4";
            this.txtFen4.Size = new System.Drawing.Size(32, 23);
            this.txtFen4.TabIndex = 7;
            // 
            // txtFen5
            // 
            this.txtFen5.Location = new System.Drawing.Point(399, 12);
            this.txtFen5.Name = "txtFen5";
            this.txtFen5.Size = new System.Drawing.Size(23, 23);
            this.txtFen5.TabIndex = 8;
            // 
            // txtFen6
            // 
            this.txtFen6.Location = new System.Drawing.Point(428, 12);
            this.txtFen6.Name = "txtFen6";
            this.txtFen6.Size = new System.Drawing.Size(23, 23);
            this.txtFen6.TabIndex = 9;
            // 
            // LoadFen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 103);
            this.Controls.Add(this.txtFen6);
            this.Controls.Add(this.txtFen5);
            this.Controls.Add(this.txtFen4);
            this.Controls.Add(this.txtFen3);
            this.Controls.Add(this.txtFen2);
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtFen1);
            this.Controls.Add(this.lblFen);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoadFen";
            this.Text = "Load Fen";
            this.Load += new System.EventHandler(this.LoadFen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFen;
        private System.Windows.Forms.TextBox txtFen1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.TextBox txtFen2;
        private System.Windows.Forms.TextBox txtFen3;
        private System.Windows.Forms.TextBox txtFen4;
        private System.Windows.Forms.TextBox txtFen5;
        private System.Windows.Forms.TextBox txtFen6;
    }
}