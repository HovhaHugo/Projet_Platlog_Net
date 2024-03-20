using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Hector
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            HectorSQL.InitialiseDatabase();
            listView1.Columns.Add("Id", 48, HorizontalAlignment.Left);
            listView1.Columns.Add("Nom", 300, HorizontalAlignment.Left);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Name)
            {
                case "NodeArticle":
                    break;
                case "NodeFamille":
                    HectorSQL.GetNomFamille(listView1);
                    break;
                case "NodeMarque":
                    MessageBox.Show("Marques cliquer !");
                    break;
                case "NodePapier":
                    MessageBox.Show("Papier cliquer !");
                    break;
                case "NodeClairefontaine":
                    MessageBox.Show("Claire Fontaine cliquer !");
                    break;
                case "NodePapierLaser":
                    MessageBox.Show("Papier Laser cliquer !");
                    break;
                case "NodeSousFamille1":
                    MessageBox.Show("Ecriture cliquer !");
                    break;
                case "NodeStylo":
                    MessageBox.Show("Stylo cliquer !");
                    break;

            }
        }

        private void importerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormImport formImport = new FormImport();
            formImport.ShowDialog();
        }
    }
}
