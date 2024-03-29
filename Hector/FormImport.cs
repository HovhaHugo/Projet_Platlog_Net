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
using System.Data.SQLite;

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
        }

        //Fonction qui fait avancer la barre de progression à chaque avancée du programme
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

        private void InsererElement(string table, string nom)
        {
            //insère un nouvel élément avec comme Nom nom, dans la Table table (soit une marque, soit une sous-famille)
        }

        private void AjoutCSV_dans_SQLite()
        {
            progressBar1.Maximum = TotalLines(openFileDialog1.FileName);    //on setup le max de la barre de progression pour voir l'avancée par ligne
            progressBar1.Value = 0;                                         //on met la barre de progression à 0
            using (var Sr = new StreamReader(openFileDialog1.FileName))
            {
                UpdateProgress();   //on prend en compte la lecture de la ligne de vérification dans la progression
                //on vérifie si le format du fichier CSV est bon
                if (Sr.ReadLine() != "Description;Ref;Marque;Famille;Sous-Famille;Prix H.T.") MessageBox.Show("Ce fichier csv n'est pas dans un bon format");
                else
                {
                    while (!Sr.EndOfStream)
                    {
                        string line = Sr.ReadLine();
                        string[] values = line.Split(';');

                        string DBPath = Path.Combine(Application.StartupPath, "Hector2.SQLite");
                        string ConnectionString = @"Data Source=" + DBPath + ";";

                        int test = 0;
                        //On ouvre un SQLiteConnection pour faire des tests, puis ajouter les valeurs du .csv dans le .SQLite
                        using (SQLiteConnection Database = new SQLiteConnection(ConnectionString))
                        {
                            SQLiteCommand Command;
                            SQLiteDataReader query;
                            
                            string IDmarque = "";
                            Database.Open();
                            using (Command = new SQLiteCommand("SELECT RefMarque, Nom FROM Marques WHERE Nom = '" + values[2] + "'", Database))
                            {
                                query = Command.ExecuteReader();
                                //CAS1.1: si la marque values[2] existe on récupère son ID
                                if (query.Read())
                                {
                                    IDmarque = query.GetInt32(0).ToString();
                                }
                                //CAS 1.2: sinon on la crée et on récupère son ID ensuite
                                else
                                {
                                    test = 1;
                                }
                            }
                            Database.Close();

                            if (test == 1)
                            {
                                //Création de la marque
                                Database.Open();
                                using (Command = new SQLiteCommand("INSERT INTO Marques (Nom) VALUES ('" + values[2] + "')", Database))
                                    Command.ExecuteNonQuery();
                                Database.Close();

                                //Récupération de son ID
                                Database.Open();
                                using (Command = new SQLiteCommand("SELECT RefMarque FROM Marques WHERE Nom = '" + values[2] + "'", Database))
                                {
                                    query = Command.ExecuteReader();
                                    query.Read();
                                    IDmarque = query.GetInt32(0).ToString();
                                }
                                Database.Close();
                                test = 0;
                            }
                            
                            Database.Open();
                            using (Command = new SQLiteCommand("SELECT RefFamille, Nom FROM Familles WHERE Nom = '" + values[3] + "'", Database))
                            {
                                query = Command.ExecuteReader();
                                //CAS 2: si la famille values[3] n'existe pas on la crée
                                if (!query.Read())
                                {
                                    test = 1;
                                }
                            }
                            Database.Close();

                            if(test == 1)
                            {
                                Database.Open();
                                using (Command = new SQLiteCommand("INSERT INTO Famille (Nom) VALUES ('" + values[3] + "')", Database))
                                    Command.ExecuteNonQuery();
                                Database.Close();
                                test = 0;
                            }

                            string IDsousfamille = "";
                            Database.Open();
                            using (Command = new SQLiteCommand("SELECT RefSousFamille, RefFamille, Nom FROM SousFamilles WHERE Nom = '" + values[4] + "'", Database))
                            {
                                query = Command.ExecuteReader();
                                //CAS 3.1: si la sous-famille values[4] existe on récupère son ID
                                if (query.Read())
                                {
                                    IDsousfamille = query.GetInt32(0).ToString();
                                }
                                //CAS 3.2: sinon on la crée et on récupère son ID ensuite
                                else
                                {
                                    test = 1;
                                }
                            }
                            Database.Close();

                            if (test == 1)
                            {
                                string IDfamille;
                                //Récupération de l'ID de la famille
                                Database.Open();
                                using (Command = new SQLiteCommand("SELECT RefFamille FROM Familles WHERE Nom = '" + values[3] + "'", Database))
                                {
                                    query = Command.ExecuteReader();
                                    query.Read();
                                    IDfamille = query.GetInt32(0).ToString();
                                }
                                Database.Close();

                                //Création de la sous-famille
                                Database.Open();
                                using (Command = new SQLiteCommand("INSERT INTO SousFamilles (RefFamille, Nom) VALUES ('" + IDfamille + "', '" + values[4] + "')", Database))
                                    Command.ExecuteNonQuery();
                                Database.Close();

                                //Récupération de son ID
                                Database.Open();
                                using (Command = new SQLiteCommand("SELECT RefSousFamille FROM SousFamilles WHERE Nom = '" + values[4] + "'", Database))
                                {
                                    query = Command.ExecuteReader();
                                    query.Read();
                                    IDsousfamille = query.GetInt32(0).ToString();
                                }
                                Database.Close();
                            }
                            
                            //on remplace la virgule du float du prix par un point pour etre dans le format des requetes SQL
                            string prix = values[5].Replace(',', '.');
                            //on ajoute l'article avec sa ref values[1], son nom values[0], ID de sa sous-famille values[4], ID de sa marque values[2], son prix values [5], et sa quantité à 0
                            //on utilise REPLACE pour mettre à jour l'article si son ID existe déjà
                            Database.Open();
                            using (Command = new SQLiteCommand("INSERT OR REPLACE INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite)" +
                                " VALUES ('" + values[1] + "', '" + values[0] + "', '" + /*IDsousfamille*/"1" + "', '" + /*IDmarque*/"1" + "', " + prix + ", 0)", Database))
                                Command.ExecuteNonQuery();
                            Database.Close();
                        }
                        UpdateProgress();
                    }
                }
            }
        }

        private void btnEcrase_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.CheckFileExists)
            {
                //supprimer la base de données actuelle
                AjoutCSV_dans_SQLite();
            }
            else MessageBox.Show("Veuillez choisir un tableau csv via le bouton 'Search file'");
        }

        private void btnAjout_Click(object sender, EventArgs e)
        {
            AjoutCSV_dans_SQLite();
        }
    }
}
