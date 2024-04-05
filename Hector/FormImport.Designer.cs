namespace Hector
{
    partial class FormImport
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
            this.btnOpenFileDialog = new System.Windows.Forms.Button();
            this.txtDirectoryPath = new System.Windows.Forms.TextBox();
            this.labelFileName = new System.Windows.Forms.Label();
            this.btnAjout = new System.Windows.Forms.Button();
            this.btnEcrase = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // btnOpenFileDialog
            // 
            this.btnOpenFileDialog.BackColor = System.Drawing.SystemColors.Control;
            this.btnOpenFileDialog.Location = new System.Drawing.Point(12, 28);
            this.btnOpenFileDialog.Name = "btnOpenFileDialog";
            this.btnOpenFileDialog.Size = new System.Drawing.Size(102, 31);
            this.btnOpenFileDialog.TabIndex = 0;
            this.btnOpenFileDialog.Text = "Search file";
            this.btnOpenFileDialog.UseVisualStyleBackColor = false;
            this.btnOpenFileDialog.Click += new System.EventHandler(this.btnOpenFileDialog_Click);
            // 
            // txtDirectoryPath
            // 
            this.txtDirectoryPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtDirectoryPath.Location = new System.Drawing.Point(0, 0);
            this.txtDirectoryPath.Name = "txtDirectoryPath";
            this.txtDirectoryPath.Size = new System.Drawing.Size(430, 22);
            this.txtDirectoryPath.TabIndex = 2;
            // 
            // labelFileName
            // 
            this.labelFileName.AutoSize = true;
            this.labelFileName.Location = new System.Drawing.Point(120, 35);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(78, 16);
            this.labelFileName.TabIndex = 6;
            this.labelFileName.Text = "Nom_fichier";
            // 
            // btnAjout
            // 
            this.btnAjout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAjout.Location = new System.Drawing.Point(222, 79);
            this.btnAjout.Name = "btnAjout";
            this.btnAjout.Size = new System.Drawing.Size(196, 23);
            this.btnAjout.TabIndex = 7;
            this.btnAjout.Text = "Ajouter";
            this.btnAjout.UseVisualStyleBackColor = true;
            this.btnAjout.Click += new System.EventHandler(this.btnAjout_Click);
            // 
            // btnEcrase
            // 
            this.btnEcrase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEcrase.Location = new System.Drawing.Point(12, 79);
            this.btnEcrase.Name = "btnEcrase";
            this.btnEcrase.Size = new System.Drawing.Size(196, 23);
            this.btnEcrase.TabIndex = 8;
            this.btnEcrase.Text = "Ecraser";
            this.btnEcrase.UseVisualStyleBackColor = true;
            this.btnEcrase.Click += new System.EventHandler(this.btnEcrase_Click);
            // 
            // progressBar1
            // 
            this.progressBar.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(0, 108);
            this.progressBar.Name = "progressBar1";
            this.progressBar.Size = new System.Drawing.Size(430, 23);
            this.progressBar.TabIndex = 9;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Fichiers cvs (*.csv)|*.csv";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // FormImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 131);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnEcrase);
            this.Controls.Add(this.btnAjout);
            this.Controls.Add(this.labelFileName);
            this.Controls.Add(this.txtDirectoryPath);
            this.Controls.Add(this.btnOpenFileDialog);
            this.Name = "FormImport";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenFileDialog;
        private System.Windows.Forms.TextBox txtDirectoryPath;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.Button btnAjout;
        private System.Windows.Forms.Button btnEcrase;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}