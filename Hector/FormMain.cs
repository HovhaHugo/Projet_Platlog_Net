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
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Name)
            {
                case "NodeArticle":
                    Console.Out.WriteLine("Article cliquer !");
                    break;
                case "NodeFamille":
                    Console.Out.WriteLine("Famille cliquer !");
                    break;
                case "NodeMarque":
                    Console.Out.WriteLine("Marques cliquer !");
                    break;
                case "NodePapier":
                    Console.Out.WriteLine("Papier cliquer !");
                    break;
                case "NodeClairefontaine":
                    Console.Out.WriteLine("Claire Fontaine cliquer !");
                    break;
                case "NodePapierLaser":
                    Console.Out.WriteLine("Papier Laser cliquer !");
                    break;
                case "NodeSousFamille1":
                    Console.Out.WriteLine("Ecriture cliquer !");
                    break;
                case "NodeStylo":
                    Console.Out.WriteLine("Stylo cliquer !");
                    break;

            }
        }
    }
}
