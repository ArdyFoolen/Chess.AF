
namespace Chess.AF.ChessForm.Forms
{
    partial class PgnDialog
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
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExportNew = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbHistory = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOpenFileDialog = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnExportAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(13, 41);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExportNew
            // 
            this.btnExportNew.Location = new System.Drawing.Point(94, 41);
            this.btnExportNew.Name = "btnExportNew";
            this.btnExportNew.Size = new System.Drawing.Size(82, 23);
            this.btnExportNew.TabIndex = 1;
            this.btnExportNew.Text = "Export New";
            this.btnExportNew.UseVisualStyleBackColor = true;
            this.btnExportNew.Click += new System.EventHandler(this.btnExportNew_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(263, 41);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cacnel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbHistory
            // 
            this.cmbHistory.FormattingEnabled = true;
            this.cmbHistory.Location = new System.Drawing.Point(71, 12);
            this.cmbHistory.Name = "cmbHistory";
            this.cmbHistory.Size = new System.Drawing.Size(562, 23);
            this.cmbHistory.TabIndex = 3;
            this.cmbHistory.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cmbHistory_KeyUp);
            this.cmbHistory.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmbHistory_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "File path";
            // 
            // btnOpenFileDialog
            // 
            this.btnOpenFileDialog.Location = new System.Drawing.Point(644, 13);
            this.btnOpenFileDialog.Name = "btnOpenFileDialog";
            this.btnOpenFileDialog.Size = new System.Drawing.Size(27, 23);
            this.btnOpenFileDialog.TabIndex = 5;
            this.btnOpenFileDialog.Text = "...";
            this.btnOpenFileDialog.UseVisualStyleBackColor = true;
            this.btnOpenFileDialog.Click += new System.EventHandler(this.btnOpenFileDialog_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnExportAdd
            // 
            this.btnExportAdd.Location = new System.Drawing.Point(182, 41);
            this.btnExportAdd.Name = "btnExportAdd";
            this.btnExportAdd.Size = new System.Drawing.Size(75, 23);
            this.btnExportAdd.TabIndex = 6;
            this.btnExportAdd.Text = "Export Add";
            this.btnExportAdd.UseVisualStyleBackColor = true;
            this.btnExportAdd.Click += new System.EventHandler(this.btnExportAdd_Click);
            // 
            // PgnDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 78);
            this.Controls.Add(this.btnExportAdd);
            this.Controls.Add(this.btnOpenFileDialog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbHistory);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnExportNew);
            this.Controls.Add(this.btnImport);
            this.Name = "PgnDialog";
            this.Text = "Pgn import/export dialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExportNew;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbHistory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpenFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnExportAdd;
    }
}