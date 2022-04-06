
namespace Chess.AF.ChessForm.Forms
{
    partial class SetupPosition
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
            this.btnSetup = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.cmbEpSquare = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rdWhiteToMove = new System.Windows.Forms.RadioButton();
            this.rdBlackToMove = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbWhiteRokade = new System.Windows.Forms.ComboBox();
            this.cmbBlackRokade = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnSetup
            // 
            this.btnSetup.Location = new System.Drawing.Point(580, 41);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(89, 23);
            this.btnSetup.TabIndex = 0;
            this.btnSetup.Text = "Setup";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(580, 70);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(580, 99);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(89, 23);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear board";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // cmbEpSquare
            // 
            this.cmbEpSquare.FormattingEnabled = true;
            this.cmbEpSquare.Location = new System.Drawing.Point(674, 153);
            this.cmbEpSquare.Name = "cmbEpSquare";
            this.cmbEpSquare.Size = new System.Drawing.Size(145, 23);
            this.cmbEpSquare.TabIndex = 2;
            this.cmbEpSquare.SelectedIndexChanged += new System.EventHandler(this.cmbEpSquare_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(580, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "En Passant";
            // 
            // rdWhiteToMove
            // 
            this.rdWhiteToMove.AutoSize = true;
            this.rdWhiteToMove.Location = new System.Drawing.Point(580, 128);
            this.rdWhiteToMove.Name = "rdWhiteToMove";
            this.rdWhiteToMove.Size = new System.Drawing.Size(103, 19);
            this.rdWhiteToMove.TabIndex = 4;
            this.rdWhiteToMove.TabStop = true;
            this.rdWhiteToMove.Text = "White to Move";
            this.rdWhiteToMove.UseVisualStyleBackColor = true;
            this.rdWhiteToMove.CheckedChanged += new System.EventHandler(this.rdWhiteToMove_CheckedChanged);
            // 
            // rdBlackToMove
            // 
            this.rdBlackToMove.AutoSize = true;
            this.rdBlackToMove.Location = new System.Drawing.Point(689, 128);
            this.rdBlackToMove.Name = "rdBlackToMove";
            this.rdBlackToMove.Size = new System.Drawing.Size(100, 19);
            this.rdBlackToMove.TabIndex = 5;
            this.rdBlackToMove.TabStop = true;
            this.rdBlackToMove.Text = "Black to Move";
            this.rdBlackToMove.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(580, 185);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "White Rokade";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(580, 214);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Black Rokade";
            // 
            // cmbWhiteRokade
            // 
            this.cmbWhiteRokade.FormattingEnabled = true;
            this.cmbWhiteRokade.Location = new System.Drawing.Point(674, 182);
            this.cmbWhiteRokade.Name = "cmbWhiteRokade";
            this.cmbWhiteRokade.Size = new System.Drawing.Size(145, 23);
            this.cmbWhiteRokade.TabIndex = 8;
            this.cmbWhiteRokade.SelectedIndexChanged += new System.EventHandler(this.cmbWhiteRokade_SelectedIndexChanged);
            // 
            // cmbBlackRokade
            // 
            this.cmbBlackRokade.FormattingEnabled = true;
            this.cmbBlackRokade.Location = new System.Drawing.Point(674, 211);
            this.cmbBlackRokade.Name = "cmbBlackRokade";
            this.cmbBlackRokade.Size = new System.Drawing.Size(145, 23);
            this.cmbBlackRokade.TabIndex = 9;
            this.cmbBlackRokade.SelectedIndexChanged += new System.EventHandler(this.cmbBlackRokade_SelectedIndexChanged);
            // 
            // SetupPosition
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(878, 594);
            this.Controls.Add(this.cmbBlackRokade);
            this.Controls.Add(this.cmbWhiteRokade);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rdBlackToMove);
            this.Controls.Add(this.rdWhiteToMove);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbEpSquare);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSetup);
            this.Controls.Add(this.btnCancel);
            this.Name = "SetupPosition";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.Text = "Setup Position";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ComboBox cmbEpSquare;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdWhiteToMove;
        private System.Windows.Forms.RadioButton rdBlackToMove;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbWhiteRokade;
        private System.Windows.Forms.ComboBox cmbBlackRokade;
    }
}