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
                        string ConncetionString = @"Data Source=" + DBPath + ";";

                        //On ouvre un SQLiteConnection pour faire des tests, puis ajouter les valeurs du .csv dans le .SQLite
                        using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
                        {
                            Database.Open();
                            SQLiteCommand selectCommand, insertCommand, replaceCommand;
                            SQLiteDataReader query;

                            string IDmarque = "";
                            selectCommand = new SQLiteCommand("SELECT RefMarque, Nom FROM Marques WHERE Nom = '"+ values[2] + "'");
                            selectCommand.Connection = Database;
                            query = selectCommand.ExecuteReader();
                            //CAS1.1: si la marque values[2] existe on récupère son ID
                            if (query.Read())
                            {
                                IDmarque = query.GetInt32(0).ToString();
                            }
                            //CAS 1.2: sinon on la crée et on récupère son ID ensuite
                            else
                            {
                                //Création de la marque
                                insertCommand = new SQLiteCommand("INSERT INTO Marques (Nom) VALUES ('" + values[2] + "')");
                                insertCommand.Connection = Database;
                                insertCommand.ExecuteNonQuery();

                                //Récupération de son ID
                                selectCommand = new SQLiteCommand("SELECT RefMarque FROM Marques WHERE Nom = '" + values[2] + "'");
                                selectCommand.Connection = Database;
                                query = selectCommand.ExecuteReader();
                                query.Read();
                                IDmarque = query.GetInt32(0).ToString();
                            }

                            selectCommand = new SQLiteCommand("SELECT RefFamille, Nom FROM Familles WHERE Nom = '" + values[3] + "'");
                            selectCommand.Connection = Database;
                            query = selectCommand.ExecuteReader();
                            //CAS 2: si la famille values[3] n'existe pas on la crée
                            if (!query.Read())
                            {
                                insertCommand = new SQLiteCommand("INSERT INTO Famille (Nom) VALUES ('" + values[3] + "')");
                                insertCommand.Connection = Database;
                                insertCommand.ExecuteNonQuery();
                            }

                            string IDsousfamille = "";
                            selectCommand = new SQLiteCommand("SELECT RefSousFamille, RefFamille, Nom FROM SousFamilles WHERE Nom = '" + values[4] + "'");
                            selectCommand.Connection = Database;
                            query = selectCommand.ExecuteReader();
                            //CAS 3.1: si la sous-famille values[4] existe on récupère son ID
                            if (query.Read())
                            {
                                IDsousfamille = query.GetInt32(0).ToString();
                            }
                            //CAS 3.2: sinon on la crée et on récupère son ID ensuite
                            else
                            {
                                //Récupération de l'ID de la famille
                                selectCommand = new SQLiteCommand("SELECT RefFamille FROM Familles WHERE Nom = '" + values[3] + "'");
                                selectCommand.Connection = Database;
                                query = selectCommand.ExecuteReader();
                                query.Read();
                                string IDfamille = query.GetInt32(0).ToString();

                                //Création de la sous-famille
                                insertCommand = new SQLiteCommand("INSERT INTO SousFamilles (RefFamille, Nom) VALUES ('" + IDfamille + "', '" + values[4] + "')");
                                insertCommand.Connection = Database;
                                insertCommand.ExecuteNonQuery();

                                //Récupération de son ID
                                selectCommand = new SQLiteCommand("SELECT RefSousFamille FROM SousFamilles WHERE Nom = '" + values[4] + "'");
                                selectCommand.Connection = Database;
                                query = selectCommand.ExecuteReader();
                                query.Read();
                                IDsousfamille = query.GetInt32(0).ToString();
                            }

                            //on remplace la virgule du float du prix par un point pour etre dans le format des requetes SQL
                            string prix = values[5].Replace(',', '.');
                            //on ajoute l'article avec sa ref values[1], son nom values[0], ID de sa sous-famille values[4], ID de sa marque values[2], son prix values [5], et sa quantité à 0
                            //on utilise REPLACE pour mettre à jour l'article si son ID existe déjà
                            replaceCommand = new SQLiteCommand("INSERT OR REPLACE INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite)"+
                                " VALUES ('" + values[1] +"', '" + values[0] + "', '" + IDsousfamille + "', '" + IDmarque + "', " + prix +", 0)");
                            replaceCommand.Connection = Database;
                            replaceCommand.ExecuteNonQuery();

                            Database.Close();
                        }
                        UpdateProgress();
                    }
                }
            }
        }

        private void btnEcrase_Click(object sender, EventArgs e)
        {
            //supprimer la base de données actuelle
            AjoutCSV_dans_SQLite();
        }

        private void btnAjout_Click(object sender, EventArgs e)
        {
            AjoutCSV_dans_SQLite();
        }
    }
}
