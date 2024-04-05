using System;
using System.ComponentModel;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

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
            if (progressBar.Value < progressBar.Maximum)
            {
                progressBar.Value++;
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

        //VERIFICATION DE L'EXISTENCE DE LA MARQUE / CREATION DE LA MARQUE SINON
        private void CheckMarque(SQLiteConnection Database, string nomMarque)
        {
            Database.Open();
            //On vérifie que la marque nommé nomMarque existe, si non on la crée
            using (SQLiteCommand Command = new SQLiteCommand("SELECT * FROM Marques WHERE Nom = '" + nomMarque + "'", Database))
            {
                SQLiteDataReader Query = Command.ExecuteReader();
                if (!Query.HasRows)
                {
                    using (SQLiteCommand CommandArg = new SQLiteCommand("INSERT INTO Marques (Nom) VALUES ('" + nomMarque + "')", Database))
                        CommandArg.ExecuteNonQuery();
                }
            }
            Database.Close();
        }

        //VERIFICATION DE L'EXISTENCE DE LA FAMILLE / CREATION DE LA FAMILLE SINON
        private void CheckFamille(SQLiteConnection Database, string nomFamille)
        {
            Database.Open();
            //On vérifie que la famille nommé nomFamille existe, si non on la crée
            using (SQLiteCommand Command = new SQLiteCommand("SELECT RefFamille, Nom FROM Familles WHERE Nom = '" + nomFamille + "'", Database))
            {
                SQLiteDataReader Query = Command.ExecuteReader();
                if (!Query.HasRows)
                {
                    using (SQLiteCommand CommandArg = new SQLiteCommand("INSERT INTO Famille (Nom) VALUES ('" + nomFamille + "')", Database))
                        CommandArg.ExecuteNonQuery();
                }
            }
            Database.Close();
        }

        //VERIFICATION DE L'EXISTENCE DE LA SOUS-FAMILLE / CREATION DE LA SOUS-FAMILLE SINON
        private void CheckSousFamille(SQLiteConnection Database, string nomFamille, string nomSousFamille)
        {
            Database.Open();
            //On vérifie que la sous-famille nommé nomSousFamille existe, si non on la crée
            using (SQLiteCommand Command = new SQLiteCommand("SELECT * FROM SousFamilles WHERE Nom = '" + nomSousFamille + "'", Database))
            {
                SQLiteDataReader Query = Command.ExecuteReader();
                if (!Query.HasRows)
                {
                    string IDfamille;
                    //On récupère l'ID de la famille nomFamille à laquelle appartient notre sous-famille
                    using (SQLiteCommand CommandArg = new SQLiteCommand("SELECT RefFamille FROM Familles WHERE Nom = '" + nomFamille + "'", Database))
                    {
                        SQLiteDataReader QueryArg = CommandArg.ExecuteReader();
                        QueryArg.Read();
                        IDfamille = QueryArg.GetInt32(0).ToString();
                    }

                    //On crée la sous-famille
                    using (SQLiteCommand CommandArg = new SQLiteCommand("INSERT INTO SousFamilles (RefFamille, Nom) VALUES ('" + IDfamille + "', '" + nomSousFamille + "')", Database))
                        CommandArg.ExecuteNonQuery();
                }
            }
            Database.Close();
        }

            private void AjoutCSV_dans_SQLite()
        {
            progressBar.Maximum = TotalLines(openFileDialog1.FileName);    //on setup le max de la barre de progression pour voir l'avancée par ligne
            progressBar.Value = 0;                                         //on met la barre de progression à 0
            using (var Sr = new StreamReader(openFileDialog1.FileName))
            {
                //on vérifie si le format du fichier CSV est bon
                if (Sr.ReadLine() != "Description;Ref;Marque;Famille;Sous-Famille;Prix H.T.") MessageBox.Show("Ce fichier csv n'est pas dans un bon format (en-tête)");
                else
                {
                    UpdateProgress();   //on prend en compte la lecture de la ligne de vérification dans la progression
                    while (!Sr.EndOfStream)
                    {
                        string line = Sr.ReadLine();
                        string[] values = line.Split(';');

                        string DBPath = Path.Combine(Application.StartupPath, "Hector2.SQLite");  //le path vers la base SQLite que l'on remplit
                        string ConnectionString = @"Data Source=" + DBPath + ";";

                        //On ouvre un SQLiteConnection pour faire des tests, puis ajouter les valeurs du .csv dans le .SQLite
                        using (SQLiteConnection Database = new SQLiteConnection(ConnectionString))
                        {
                            //On récupère l'ID de la marque values[2]
                            CheckMarque(Database, values[2]);

                            //On crée la famille values[3] si elle n'existe pas
                            CheckFamille(Database, values[3]);

                            //On récupère l'ID de la sous-famille values[4]
                            CheckSousFamille(Database, values[3], values[4]);

                            //CREATION DE L'ARTICLE

                            string prix = values[5].Replace(',', '.'); //on remplace la virgule du float du prix par un point pour etre dans le format des requetes SQL

                            //on ajoute l'article avec sa ref values[1], son nom values[0], ID de sa sous-famille values[4], ID de sa marque values[2], son prix values [5], et sa quantité à 0
                            //on utilise INSERT OR REPLACE pour mettre à jour l'article si son ID existe déjà
                            Database.Open();
                            string RequeteAjoutArticle = "INSERT OR REPLACE INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite)" +
                                " VALUES ('" + values[1] + "', '" + values[0] + "', " +
                                "(SELECT RefSousFamille FROM SousFamilles WHERE Nom = '" + values[4] + "'), " +
                                "(SELECT RefMarque FROM Marques WHERE Nom = '" + values[2] + "'), " + prix + ", 0)";

                            using (SQLiteCommand Command = new SQLiteCommand(RequeteAjoutArticle, Database))
                                Command.ExecuteNonQuery();
                            Database.Close();
                        }
                        UpdateProgress(); //on fait avancer la barre de progression
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
