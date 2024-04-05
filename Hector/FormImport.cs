using System;
using System.Collections.Generic;
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

        //Fonction qui ouvre le openFileDialog lorsque l'on appuie sur le bouton pour chercher un fichier csv
        private void btnOpenFileDialog_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        //Fonction qui actualise le label du nom du fichier et le path du fichier lorsque l'on en a sélectionné un via le openFileDialog
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

        //Fonction qui compte le nombre totale de lignes dans un fichier csv (pour la barre de progression)
        private int TotalLines(string filePath)
        {
            using (StreamReader r = new StreamReader(filePath))
            {
                int i = 0;
                while (r.ReadLine() != null) { i++; }
                return i;
            }
        }

        //Fonction qui vérifie que le fichier csv est bon, et rempli le tableau value des données du csv si oui, en revoyant true (sinon il renvoie false)
        private bool LectureCSV(List<string[]> values)
        {
            bool result = false;
            var Sr = new StreamReader(openFileDialog1.FileName);

            //on vérifie si le format du fichier CSV est bon
            if (Sr.ReadLine() != "Description;Ref;Marque;Famille;Sous-Famille;Prix H.T.")
            {
                MessageBox.Show("Ce fichier csv n'est pas dans un bon format (en-tête)");
            }
            else
            {
                UpdateProgress();   //pour la barre de progression, on prend en compte la lecture de la ligne de vérification

                //on crée un tableau 2D values dans lequel on met toutes les données du csv
                while (!Sr.EndOfStream)
                {
                    string line = Sr.ReadLine();
                    values.Add(line.Split(';'));
                }
                result = true;
            }
            Sr.Close();
            return result;
        }

        //Fonction pour vérifier l'existence d'une marque à partir de son nom, et la créer si ce n'est pas le cas
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

        //Fonction pour vérifier l'existence d'une famille à partir de son nom, et la créer si ce n'est pas le cas
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

        //Fonction pour vérifier l'existence d'une sous-famille à partir de son nom, et la créer si ce n'est pas le cas
        private void CheckSousFamille(SQLiteConnection Database, string nomFamille, string nomSousFamille)
        {
            Database.Open();
            //On vérifie que la sous-famille nommé nomSousFamille existe, si non on la crée
            using (SQLiteCommand Command = new SQLiteCommand("SELECT * FROM SousFamilles WHERE Nom = '" + nomSousFamille + "'", Database))
            {
                SQLiteDataReader Query = Command.ExecuteReader();
                if (!Query.HasRows)
                {
                    //On crée la sous-famille
                    using (SQLiteCommand CommandArg = new SQLiteCommand("INSERT INTO SousFamilles (RefFamille, Nom) VALUES ((SELECT RefFamille FROM Familles WHERE Nom = '" + nomFamille + "'), '" + nomSousFamille + "')", Database))
                        CommandArg.ExecuteNonQuery();
                }
            }
            Database.Close();
        }

        //Fonction qui va lire le fichier csv obtenu à partir du openFileDialog, puis entrer un à un les articles,
        //en prenant soin de crée les marques/familles/sous-familles au fur et à mesure qu'on en a besoin, si jamais ils n'existent pas
        private void AjoutCSV_dans_SQLite()
        {
            progressBar.Maximum = TotalLines(openFileDialog1.FileName);    //on setup le max de la barre de progression pour voir l'avancée par ligne
            progressBar.Value = 0;                                         //on met la barre de progression à 0
            List<string[]> values = new List<string[]>(); ;
            if(LectureCSV(values))      //Si le format csv est bon, les valeurs sont mises dans values et on commence l'importation
            {
                string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");  //le path vers la base SQLite que l'on remplit
                string ConnectionString = @"Data Source=" + DBPath + ";";
                SQLiteConnection Database = new SQLiteConnection(ConnectionString);

                for (int IterationArticle = 0; IterationArticle < values.Count; IterationArticle++)
                {
                    /*
                    //On crée la marque values[2] si elle n'existe pas
                    CheckMarque(Database, values[IterationArticle][2]);

                    //On crée la famille values[3] si elle n'existe pas
                    CheckFamille(Database, values[IterationArticle][3]);

                    //On crée la sous-famille values[4] si elle n'existe pas
                    CheckSousFamille(Database, values[IterationArticle][3], values[IterationArticle][4]);
                    */

                    //CREATION DE L'ARTICLE

                    string prix = values[IterationArticle][5].Replace(',', '.'); //on remplace la virgule du float du prix par un point pour etre dans le format des requetes SQL

                    //on ajoute l'article avec sa ref values[1], son nom values[0], ID de sa sous-famille values[4], ID de sa marque values[2], son prix values [5], et sa quantité à 0
                    //on utilise INSERT OR REPLACE pour mettre à jour l'article si son ID existe déjà

                    /*
                    string RequeteAjoutArticle = "INSERT OR REPLACE INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite)" +
                        " VALUES ('" + values[1] + "', '" + values[0] + "', " +
                        "(SELECT RefSousFamille FROM SousFamilles WHERE Nom = '" + values[4] + "'), " +
                        "(SELECT RefMarque FROM Marques WHERE Nom = '" + values[2] + "'), " + prix + ", 0)";
                    */

                    string RequeteAjoutArticle = "INSERT OR REPLACE INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite)" +
                        " VALUES ('" + values[1] + "', '" + values[0] + "', '1', '1', " + prix + ", 0)";

                    Database.Open();
                    using (SQLiteCommand Command = new SQLiteCommand(RequeteAjoutArticle, Database))
                        Command.ExecuteNonQuery();
                    Database.Close();
                    UpdateProgress(); //on fait avancer la barre de progression
                }
            }
        }

        private void btnEcrase_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.CheckFileExists)
            {
                //Il faut supprimer la base de données actuelle ici
                AjoutCSV_dans_SQLite();
            }
            else MessageBox.Show("Veuillez choisir un tableau csv via le bouton 'Search file'");
        }

        private void btnAjout_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.CheckFileExists)
            {
                AjoutCSV_dans_SQLite();
            }
            else MessageBox.Show("Veuillez choisir un tableau csv via le bouton 'Search file'");
        }
    }
}
