
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
            this.components = new System.ComponentModel.Container();
            this.lblFen = new System.Windows.Forms.Label();
            this.txtFen1 = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDefault = new System.Windows.Forms.Button();
            this.cmbFen1 = new System.Windows.Forms.ComboBox();
            this.cmbFen2 = new System.Windows.Forms.ComboBox();
            this.cmbFen3 = new System.Windows.Forms.ComboBox();
            this.nbrFen1 = new System.Windows.Forms.NumericUpDown();
            this.nbrFen2 = new System.Windows.Forms.NumericUpDown();
            this.cmbFenHistory = new System.Windows.Forms.ComboBox();
            this.lblHistory = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip3 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip4 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip5 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip6 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip7 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nbrFen1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbrFen2)).BeginInit();
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
            this.txtFen1.Location = new System.Drawing.Point(83, 12);
            this.txtFen1.Name = "txtFen1";
            this.txtFen1.Size = new System.Drawing.Size(310, 23);
            this.txtFen1.TabIndex = 1;
            this.txtFen1.Text = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(12, 101);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(197, 101);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDefault
            // 
            this.btnDefault.Location = new System.Drawing.Point(106, 101);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(75, 23);
            this.btnDefault.TabIndex = 4;
            this.btnDefault.Text = "Default";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // cmbFen1
            // 
            this.cmbFen1.FormattingEnabled = true;
            this.cmbFen1.Items.AddRange(new object[] {
            "w",
            "b"});
            this.cmbFen1.Location = new System.Drawing.Point(399, 12);
            this.cmbFen1.Name = "cmbFen1";
            this.cmbFen1.Size = new System.Drawing.Size(38, 23);
            this.cmbFen1.TabIndex = 11;
            this.cmbFen1.Text = "w";
            // 
            // cmbFen2
            // 
            this.cmbFen2.FormattingEnabled = true;
            this.cmbFen2.Items.AddRange(new object[] {
            "-",
            "K",
            "Q",
            "k",
            "q",
            "KQ",
            "Kk",
            "kq",
            "Qk",
            "Qq",
            "KQk",
            "KQq",
            "Kkq",
            "Qkq",
            "KQkq"});
            this.cmbFen2.Location = new System.Drawing.Point(443, 12);
            this.cmbFen2.Name = "cmbFen2";
            this.cmbFen2.Size = new System.Drawing.Size(60, 23);
            this.cmbFen2.TabIndex = 12;
            this.cmbFen2.Text = "KQkq";
            // 
            // cmbFen3
            // 
            this.cmbFen3.FormattingEnabled = true;
            this.cmbFen3.Location = new System.Drawing.Point(509, 12);
            this.cmbFen3.Name = "cmbFen3";
            this.cmbFen3.Size = new System.Drawing.Size(39, 23);
            this.cmbFen3.TabIndex = 13;
            this.cmbFen3.Text = "-";
            // 
            // nbrFen1
            // 
            this.nbrFen1.Location = new System.Drawing.Point(554, 13);
            this.nbrFen1.Name = "nbrFen1";
            this.nbrFen1.Size = new System.Drawing.Size(43, 23);
            this.nbrFen1.TabIndex = 16;
            // 
            // nbrFen2
            // 
            this.nbrFen2.Location = new System.Drawing.Point(603, 13);
            this.nbrFen2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nbrFen2.Name = "nbrFen2";
            this.nbrFen2.Size = new System.Drawing.Size(43, 23);
            this.nbrFen2.TabIndex = 17;
            this.nbrFen2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cmbFenHistory
            // 
            this.cmbFenHistory.FormattingEnabled = true;
            this.cmbFenHistory.Location = new System.Drawing.Point(83, 42);
            this.cmbFenHistory.Name = "cmbFenHistory";
            this.cmbFenHistory.Size = new System.Drawing.Size(563, 23);
            this.cmbFenHistory.TabIndex = 18;
            this.cmbFenHistory.SelectedIndexChanged += new System.EventHandler(this.cmbFenHistory_SelectedIndexChanged);
            this.cmbFenHistory.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cmbFenHistory_KeyUp);
            // 
            // lblHistory
            // 
            this.lblHistory.AutoSize = true;
            this.lblHistory.Location = new System.Drawing.Point(13, 45);
            this.lblHistory.Name = "lblHistory";
            this.lblHistory.Size = new System.Drawing.Size(45, 15);
            this.lblHistory.TabIndex = 19;
            this.lblHistory.Text = "History";
            // 
            // toolTip7
            // 
            this.toolTip7.ShowAlways = true;
            // 
            // LoadFen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 154);
            this.Controls.Add(this.lblHistory);
            this.Controls.Add(this.cmbFenHistory);
            this.Controls.Add(this.nbrFen2);
            this.Controls.Add(this.nbrFen1);
            this.Controls.Add(this.cmbFen3);
            this.Controls.Add(this.cmbFen2);
            this.Controls.Add(this.cmbFen1);
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
            ((System.ComponentModel.ISupportInitialize)(this.nbrFen1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbrFen2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFen;
        private System.Windows.Forms.TextBox txtFen1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.ComboBox cmbFen1;
        private System.Windows.Forms.ComboBox cmbFen2;
        private System.Windows.Forms.ComboBox cmbFen3;
        private System.Windows.Forms.NumericUpDown nbrFen1;
        private System.Windows.Forms.NumericUpDown nbrFen2;
        private System.Windows.Forms.ComboBox cmbFenHistory;
        private System.Windows.Forms.Label lblHistory;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.ToolTip toolTip3;
        private System.Windows.Forms.ToolTip toolTip4;
        private System.Windows.Forms.ToolTip toolTip5;
        private System.Windows.Forms.ToolTip toolTip6;
        private System.Windows.Forms.ToolTip toolTip7;
    }
}