
namespace Hector
{
    partial class FormAjoutArticle
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.LblReference = new System.Windows.Forms.Label();
            this.LblDescription = new System.Windows.Forms.Label();
            this.LblSousFamille = new System.Windows.Forms.Label();
            this.LblFamille = new System.Windows.Forms.Label();
            this.LblMarque = new System.Windows.Forms.Label();
            this.LblPrix = new System.Windows.Forms.Label();
            this.LblQuantite = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(183, 159);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(186, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.Enabled = false;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(183, 201);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(186, 21);
            this.comboBox2.TabIndex = 1;
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(183, 241);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(186, 21);
            this.comboBox3.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(183, 125);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(186, 20);
            this.textBox1.TabIndex = 3;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(183, 324);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(186, 20);
            this.numericUpDown1.TabIndex = 4;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(183, 286);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(186, 20);
            this.textBox2.TabIndex = 5;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(183, 90);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(186, 20);
            this.textBox3.TabIndex = 6;
            // 
            // LblReference
            // 
            this.LblReference.AutoSize = true;
            this.LblReference.Location = new System.Drawing.Point(50, 96);
            this.LblReference.Name = "LblReference";
            this.LblReference.Size = new System.Drawing.Size(57, 13);
            this.LblReference.TabIndex = 7;
            this.LblReference.Text = "Référence";
            // 
            // LblDescription
            // 
            this.LblDescription.AutoSize = true;
            this.LblDescription.Location = new System.Drawing.Point(50, 132);
            this.LblDescription.Name = "LblDescription";
            this.LblDescription.Size = new System.Drawing.Size(60, 13);
            this.LblDescription.TabIndex = 8;
            this.LblDescription.Text = "Description";
            // 
            // LblSousFamille
            // 
            this.LblSousFamille.AutoSize = true;
            this.LblSousFamille.Location = new System.Drawing.Point(50, 209);
            this.LblSousFamille.Name = "LblSousFamille";
            this.LblSousFamille.Size = new System.Drawing.Size(66, 13);
            this.LblSousFamille.TabIndex = 9;
            this.LblSousFamille.Text = "Sous-Famille";
            // 
            // LblFamille
            // 
            this.LblFamille.AutoSize = true;
            this.LblFamille.Location = new System.Drawing.Point(50, 167);
            this.LblFamille.Name = "LblFamille";
            this.LblFamille.Size = new System.Drawing.Size(39, 13);
            this.LblFamille.TabIndex = 10;
            this.LblFamille.Text = "Famille";
            // 
            // LblMarque
            // 
            this.LblMarque.AutoSize = true;
            this.LblMarque.Location = new System.Drawing.Point(50, 249);
            this.LblMarque.Name = "LblMarque";
            this.LblMarque.Size = new System.Drawing.Size(43, 13);
            this.LblMarque.TabIndex = 11;
            this.LblMarque.Text = "Marque";
            // 
            // LblPrix
            // 
            this.LblPrix.AutoSize = true;
            this.LblPrix.Location = new System.Drawing.Point(50, 293);
            this.LblPrix.Name = "LblPrix";
            this.LblPrix.Size = new System.Drawing.Size(24, 13);
            this.LblPrix.TabIndex = 12;
            this.LblPrix.Text = "Prix";
            // 
            // LblQuantite
            // 
            this.LblQuantite.AutoSize = true;
            this.LblQuantite.Location = new System.Drawing.Point(50, 331);
            this.LblQuantite.Name = "LblQuantite";
            this.LblQuantite.Size = new System.Drawing.Size(47, 13);
            this.LblQuantite.TabIndex = 13;
            this.LblQuantite.Text = "Quantité";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(132, 35);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(150, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Ajout/Modification d\'un article ";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(293, 367);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(390, 367);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FormAjoutArticle
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(477, 402);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.LblQuantite);
            this.Controls.Add(this.LblPrix);
            this.Controls.Add(this.LblMarque);
            this.Controls.Add(this.LblFamille);
            this.Controls.Add(this.LblSousFamille);
            this.Controls.Add(this.LblDescription);
            this.Controls.Add(this.LblReference);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAjoutArticle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ajout d\'un article";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label LblReference;
        private System.Windows.Forms.Label LblDescription;
        private System.Windows.Forms.Label LblSousFamille;
        private System.Windows.Forms.Label LblFamille;
        private System.Windows.Forms.Label LblMarque;
        private System.Windows.Forms.Label LblPrix;
        private System.Windows.Forms.Label LblQuantite;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}