using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Hector
{
    public partial class FormImport : Form
    {
        public FormImport()
        {
            InitializeComponent();
        }

        private void btnOpenFileDialog_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            labelFileName.Text = openFileDialog1.SafeFileName;
            txtDirectoryPath.Text = openFileDialog1.FileName;
            /*using (StreamReader Sr = File.OpenText(openFileDialog1.FileName))
            {
                string Ligne = "";
                textBox1.Text = Ligne;
                while ((Ligne = Sr.ReadLine()) != null)
                {
                    //On rajoute chaque ligne du fichier dans la Textbox
                    textBox1.Text += Ligne;
                    textBox1.AppendText(Environment.NewLine);
                }
                if (etoile)
                {
                    etoile = false;
                }
                Text = OGformName + "[" + openFileDialog1.SafeFileName + "]";

                var watcher = new FileSystemWatcher(openFileDialog1.FileName);
                watcher.NotifyFilter = NotifyFilters.Attributes
                                     | NotifyFilters.CreationTime
                                     | NotifyFilters.DirectoryName
                                     | NotifyFilters.FileName
                                     | NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.Security
                                     | NotifyFilters.Size;

                watcher.Changed += OnChanged;
                watcher.Filter = "*.txt";
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;
            }*/
        }

        //Fonctions qui font avancer les barres de progression à chaque avancée
        private void UpdateProgress()
        {
            if (progressBar1.Value < progressBar1.Maximum)
            {
                progressBar1.Value++;
                Application.DoEvents();
            }
        }

        private int TotalLines(string filePath)
        {
            using (StreamReader r = new StreamReader(filePath))
            {
                int i = 0;
                while (r.ReadLine() != null) { i++; }
                return i;
            }
        }

        private void btnEcrase_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = TotalLines(openFileDialog1.FileName);    //on setup le max de la barre de progression pour voir l'avancée par ligne
            progressBar1.Value = 0;                                         //on met la barre de progression à 0
            using (var Sr = new StreamReader(openFileDialog1.FileName))
            {
                UpdateProgress();   //on prend en compte la lecture de la ligne de vérification dans la progression
                if (Sr.ReadLine() != "Description;Ref;Marque;Famille;Sous-Famille;Prix H.T.") MessageBox.Show("Mais c'est quoi ce csv dégeulasse?!!");
                else
                {
                    while (!Sr.EndOfStream)
                    {
                        string line = Sr.ReadLine();
                        string[] values = line.Split(';');

                        //si la ref de l'article values[1] existe déjà on renvoie une erreur (ou on la compte juste pas en la stockant dans une liste d'echec)
                        //si la marque values[2] existe pas on la crée et on récupère son ID
                        //sinon on récupère l'ID de la marque existante
                        //si la famille values[3] existe pas on la crée
                        //si la sous-famille values[4] existe pas on la crée
                        //sinon on récupère l'ID de la sous-famille existante
                        //on ajoute l'article avec sa ref values[1], son nom values[0], sa sous-famille values[4], sa marque values[2], son prix values [5], et sa quantité à 0

                        UpdateProgress();
                    }
                }
            }
        }

        private void btnAjout_Click(object sender, EventArgs e)
        {

        }
    }
}
