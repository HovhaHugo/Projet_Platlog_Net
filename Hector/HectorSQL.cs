using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hector
{
    class HectorSQL
    {

        /// <summary>
        /// Initialise la connection avec la base de donnée et permet de mettre le nom des noeuds des sous-familles ou des marques.
        /// </summary>
        /// <param name="treeView1"> Le treeview qui sera mis a jour</param>
        public static void InitialiseDatabase(TreeView treeView1)
        {
            //Partie que l'on va réutilisé par la suite, on crèer notre connection à la base de données.
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");
            string ConncetionString = @"Data Source="+DBPath+";";

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();

                foreach (TreeNode NodePrincipal in treeView1.Nodes)
                {
                    if (NodePrincipal.Name == "NodeFamille")
                    {

                        NodePrincipal.Nodes.Clear();

                        //Partie pour les familles
                        SQLiteCommand SelectCommandFamille = new SQLiteCommand("SELECT * FROM Familles", Database);

                        SQLiteDataReader Query = SelectCommandFamille.ExecuteReader();

                        while (Query.Read())
                        {

                            TreeNode NodeFamille = new TreeNode(Query.GetString(1).ToString());

                            NodePrincipal.Nodes.Add(NodeFamille);
                        }

                        //Partie pour les sous familles
                        SQLiteCommand SelectCommandSousFamille = new SQLiteCommand("SELECT * FROM SousFamilles " +
                            "INNER JOIN Familles On Familles.RefFamille = SousFamilles.RefFamille", Database);

                        SQLiteDataReader QuerySousFamille = SelectCommandSousFamille.ExecuteReader();

                        while (QuerySousFamille.Read())
                        {

                            TreeNode NodeSousFamille = new TreeNode(QuerySousFamille.GetString(2).ToString());

                            foreach(TreeNode NodeFamille in NodePrincipal.Nodes)
                            {

                                if(QuerySousFamille.GetString(4).ToString() == NodeFamille.Text)
                                {

                                    NodeFamille.Nodes.Add(NodeSousFamille);
                                }
                            }
                        }

                    }
                    if (NodePrincipal.Name == "NodeMarque")
                    {

                        NodePrincipal.Nodes.Clear();

                        SQLiteCommand SelectCommand = new SQLiteCommand("SELECT * FROM Marques", Database);

                        SelectCommand.Connection = Database;

                        SQLiteDataReader Query = SelectCommand.ExecuteReader();

                        while (Query.Read())
                        {

                            TreeNode NodeMarque = new TreeNode(Query.GetString(1).ToString());

                            NodePrincipal.Nodes.Add(NodeMarque);
                        }
                    }
                }
            }
        }

        #region Getteurs

        /// <summary>
        /// Permet de récupérer la liste des Familles et les mets dans le listViews
        /// </summary>
        /// <param name="ListView1"> Le listview à modifier </param>
        public static void GetFamilles(ListView ListView1)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");

            string ConncetionString = @"Data Source=" + DBPath + ";";

            ListView1.Columns.Add("Description", -2, HorizontalAlignment.Left);

            ListView1.GridLines = true;

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();

                SQLiteCommand SelectCommand = new SQLiteCommand("SELECT * FROM Familles", Database);

                SQLiteDataReader Query = SelectCommand.ExecuteReader();

                while (Query.Read())
                {
                    ListView1.Items.Add(Query.GetString(1).ToString());
                }
            }
        }

        /// <summary>
        /// Permet de récupérer la liste des SousFamilles et les mets dans le listViews
        /// </summary>
        /// <param name="ListView1"> Le listview à modifier </param>
        /// <param name="nomNode">Le nom de la famille parent à la sous famille</param>
        public static void GetSousFamilles(ListView ListView1, String nomNode)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");

            string ConncetionString = @"Data Source=" + DBPath + ";";

            ListView1.Columns.Add("Description", -2, HorizontalAlignment.Left);

            ListView1.GridLines = true;

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();

                SQLiteCommand SelectCommand = new SQLiteCommand("SELECT RefSousFamille, Familles.Nom, SousFamilles.Nom " +
                    "FROM SousFamilles INNER JOIN Familles ON Familles.RefFamille = SousFamilles.RefFamille " +
                    "WHERE Familles.Nom = '" + nomNode + "'", Database);

                SQLiteDataReader Query = SelectCommand.ExecuteReader();

                while (Query.Read())
                {
                    ListViewItem Item = new ListViewItem(Query.GetString(2).ToString());

                    ListView1.Items.Add(Item);
                }
            }
        }

        /// <summary>
        ///  Permet de récupérer la liste des Articles et les mets dans le listViews
        /// </summary>
        /// <param name="ListView1"> Le listview à modifier </param>
        public static void GetArticles(ListView ListView1)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");
            string ConncetionString = @"Data Source=" + DBPath + ";";

            ListView1.Columns.Add("Reférence",90, HorizontalAlignment.Left);

            ListView1.Columns.Add("Description",150, HorizontalAlignment.Left);

            ListView1.Columns.Add("Famille",150, HorizontalAlignment.Left);

            ListView1.Columns.Add("Sous-Famille",150, HorizontalAlignment.Left);

            ListView1.Columns.Add("Marque",150, HorizontalAlignment.Left);

            ListView1.Columns.Add("Quantite",80, HorizontalAlignment.Left);

            ListView1.GridLines = true;

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();

                SQLiteCommand SelectCommand = new SQLiteCommand("SELECT RefArticle, Description, " +
                    "Familles.Nom, SousFamilles.Nom, Marques.Nom, Quantite " +
                    "FROM Articles " +
                    "INNER JOIN SousFamilles ON SousFamilles.RefSousFamille = Articles.RefSousFamille " +
                    "INNER JOIN Familles ON Familles.RefFamille = SousFamilles.RefFamille " +
                    "INNER JOIN Marques ON Marques.RefMarque = Articles.RefMarque ",Database);

                SQLiteDataReader Query = SelectCommand.ExecuteReader();

                while (Query.Read())
                {
                    ListViewItem Item = new ListViewItem(Query.GetString(0).ToString());

                    Item.SubItems.Add(Query.GetString(1).ToString());

                    Item.SubItems.Add(Query.GetString(2).ToString());

                    Item.SubItems.Add(Query.GetString(3).ToString());

                    Item.SubItems.Add(Query.GetString(4).ToString());

                    Item.SubItems.Add(Query.GetValue(5).ToString());
                   
                    ListView1.Items.Add(Item);
                }
            }
        }


        /// <summary>
        /// Permet de récupérer la liste des Marques et les mets dans le listViews
        /// </summary>
        /// <param name="ListView1"> Le listview à modifier </param>
        public static void GetMarques(ListView ListView1)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");

            string ConncetionString = @"Data Source=" + DBPath + ";";

            ListView1.Columns.Add("Description", -2, HorizontalAlignment.Left);

            ListView1.GridLines = true;

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();

                SQLiteCommand SelectCommand = new SQLiteCommand("SELECT * FROM Marques", Database);

                SQLiteDataReader Query = SelectCommand.ExecuteReader();

                while (Query.Read())
                {
                    ListViewItem Item = new ListViewItem(Query.GetString(1).ToString());

                    ListView1.Items.Add(Item);
                }
            }
        }

        #endregion

        #region GetArticlesBy...

        /// <summary>
        /// Permet de récupérer la liste des Articles trié par la Famille séléctionné 
        /// </summary>
        /// <param name="ListView1"> Le listview qui a être modifier</param>
        /// <param name="nomNode">Le nom de la famille qui à été séléctionner</param>
        public static void GetArticlesByFamille(ListView ListView1, String nomNode)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");
            string ConncetionString = @"Data Source=" + DBPath + ";";

            ListView1.Columns.Add("Reférence", 48, HorizontalAlignment.Left);
            ListView1.Columns.Add("Description", 100, HorizontalAlignment.Left);
            ListView1.Columns.Add("Sous-Famille", 100, HorizontalAlignment.Left);
            ListView1.Columns.Add("Marque", 100, HorizontalAlignment.Left);
            ListView1.Columns.Add("Prix", 100, HorizontalAlignment.Left);
            ListView1.Columns.Add("Quantite", 100, HorizontalAlignment.Left);

            ListView1.GridLines = true;

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();
                SQLiteCommand SelectCommand = new SQLiteCommand("SELECT RefArticle, Description, " +
                    "SousFamilles.Nom, Marques.Nom, PrixHT, Quantite " +
                    "FROM Articles " +
                    "INNER JOIN SousFamilles ON SousFamilles.RefSousFamille = Articles.RefSousFamille " +
                    "INNER JOIN Marques ON Marques.RefMarque = Articles.RefMarque " +
                    "INNER JOIN Familles ON Familles.RefFamille = SousFamilles.RefFamille " +
                    "WHERE Familles.Nom == '" + nomNode + "'", Database);

                SQLiteDataReader Query = SelectCommand.ExecuteReader();

                while (Query.Read())
                {
                    ListViewItem Item = new ListViewItem(Query.GetString(0).ToString());

                    Item.SubItems.Add(Query.GetString(1).ToString());

                    Item.SubItems.Add(Query.GetString(2).ToString());

                    Item.SubItems.Add(Query.GetString(3).ToString());

                    Item.SubItems.Add(Query.GetValue(4).ToString());

                    Item.SubItems.Add(Query.GetValue(5).ToString());

                    ListView1.Items.Add(Item);
                }
            }
        }

        /// <summary>
        /// Permet de récupérer la liste des Articles trié par la Marque séléctionné
        /// </summary>
        /// <param name="ListView1"> Le listview qui a être modifier </param>
        /// <param name="nomNode">Le nom de la marque qui à été séléctionner</param>
        public static void GetArticlesByMarque(ListView ListView1, String nomNode)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");
            string ConncetionString = @"Data Source=" + DBPath + ";";

            ListView1.Columns.Add("Reférence", 48, HorizontalAlignment.Left);

            ListView1.Columns.Add("Description", 100, HorizontalAlignment.Left);

            ListView1.Columns.Add("Sous-Famille", 100, HorizontalAlignment.Left);

            ListView1.Columns.Add("Marque", 100, HorizontalAlignment.Left);

            ListView1.Columns.Add("Prix", 100, HorizontalAlignment.Left);

            ListView1.Columns.Add("Quantite", 100, HorizontalAlignment.Left);

            ListView1.GridLines = true;

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();

                SQLiteCommand SelectCommand = new SQLiteCommand("SELECT RefArticle, Description, " +
                    "SousFamilles.Nom, Marques.Nom, PrixHT, Quantite " +
                    "FROM Articles " +
                    "INNER JOIN SousFamilles ON SousFamilles.RefSousFamille = Articles.RefSousFamille " +
                    "INNER JOIN Marques ON Marques.RefMarque = Articles.RefMarque " +
                    "WHERE Marques.Nom == '" + nomNode + "'", Database);

                SQLiteDataReader Query = SelectCommand.ExecuteReader();

                while (Query.Read())
                {
                    ListViewItem Item = new ListViewItem(Query.GetString(0).ToString());

                    Item.SubItems.Add(Query.GetString(1).ToString());

                    Item.SubItems.Add(Query.GetString(2).ToString());

                    Item.SubItems.Add(Query.GetString(3).ToString());

                    Item.SubItems.Add(Query.GetValue(4).ToString());

                    Item.SubItems.Add(Query.GetValue(5).ToString());

                    ListView1.Items.Add(Item);
                    //MessageBox.Show(Query.GetString(0).ToString());
                }
            }
        }

        /// <summary>
        /// Permet de récupérer la liste des Articles trié par la SousFamille séléctionné
        /// </summary>
        /// <param name="ListView1"> Le listview qui a être modifier </param>
        /// <param name="nomNode"> Le nom de la marque qui à été séléctionner </param>
        public static void GetArticlesBySousFamille(ListView ListView1, String nomNode)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");

            string ConncetionString = @"Data Source=" + DBPath + ";";

            ListView1.Columns.Add("Reférence", 48, HorizontalAlignment.Left);

            ListView1.Columns.Add("Description", 100, HorizontalAlignment.Left);

            ListView1.Columns.Add("Sous-Famille", 100, HorizontalAlignment.Left);

            ListView1.Columns.Add("Marque", 100, HorizontalAlignment.Left);

            ListView1.Columns.Add("Prix", 100, HorizontalAlignment.Left);

            ListView1.Columns.Add("Quantite", 100, HorizontalAlignment.Left);

            ListView1.GridLines = true;

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();

                SQLiteCommand SelectCommand = new SQLiteCommand("SELECT RefArticle, Description, " +
                    "SousFamilles.Nom, Marques.Nom, PrixHT, Quantite " +
                    "FROM Articles " +
                    "INNER JOIN SousFamilles ON SousFamilles.RefSousFamille = Articles.RefSousFamille " +
                    "INNER JOIN Marques ON Marques.RefMarque = Articles.RefMarque " +
                    "WHERE SousFamilles.Nom == '" + nomNode + "'", Database);

                SQLiteDataReader Query = SelectCommand.ExecuteReader();

                while (Query.Read())
                {
                    ListViewItem Item = new ListViewItem(Query.GetString(0).ToString());

                    Item.SubItems.Add(Query.GetString(1).ToString());

                    Item.SubItems.Add(Query.GetString(2).ToString());

                    Item.SubItems.Add(Query.GetString(3).ToString());

                    Item.SubItems.Add(Query.GetValue(4).ToString());

                    Item.SubItems.Add(Query.GetValue(5).ToString());

                    ListView1.Items.Add(Item);
                }
            }
        }

        #endregion

        /// <summary>
        /// Permet de récupérer le nombre d'article actuellement présent dans la base de données
        /// </summary>
        /// <returns>Le nombre d'article total dans la base de données</returns>
        public static int GetNumberArticles()
        {

            int NombreArticles = 0;

            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");

            string ConncetionString = @"Data Source=" + DBPath + ";";

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();

                SQLiteCommand SelectCommand = new SQLiteCommand("SELECT RefArticle, Description, " +
                    "Familles.Nom, SousFamilles.Nom, Marques.Nom, Quantite " +
                    "FROM Articles " +
                    "INNER JOIN SousFamilles ON SousFamilles.RefSousFamille = Articles.RefSousFamille " +
                    "INNER JOIN Familles ON Familles.RefFamille = SousFamilles.RefFamille " +
                    "INNER JOIN Marques ON Marques.RefMarque = Articles.RefMarque ", Database);

                SQLiteDataReader Query = SelectCommand.ExecuteReader();

                while (Query.Read())
                {

                    NombreArticles++;

                }
            }
            return NombreArticles;
        }

        /// <summary>
        /// Fonction qui permet d'utiliser les bonnes suppresions en fonction du noeud séléctionner
        /// </summary>
        /// <param name="ElementASupprimer">L'éléments à supprimer</param>
        /// <param name="NoeudsElement">Le noeuds séléctionner</param>
        /// <returns></returns>
        public static string DelElements(string ElementASupprimer, TreeNode NoeudsElement)
        { 
            string ConfirmationSuppression = "Il y a eu un problème dans le programme.";
                
            if(NoeudsElement.Text == "Familles")
            {

                ConfirmationSuppression =DelFamilles(ElementASupprimer);

            }
            else
            {

                if (NoeudsElement.Text == "Marques")
                {

                    ConfirmationSuppression = DelMarques(ElementASupprimer);

                }
                else
                {

                    if (NoeudsElement.Text == "Tous les articles")
                    {

                        ConfirmationSuppression = DelArticle(ElementASupprimer);

                    }
                }
            }
            if (NoeudsElement.Parent != null)
            {

                if (NoeudsElement.Parent.Text == "Familles")
                {

                    ConfirmationSuppression = DelSousFamilles(ElementASupprimer);

                }
                else
                {

                    ConfirmationSuppression = DelArticle(ElementASupprimer);

                }
            }

            return ConfirmationSuppression;
        }

        #region Delete

        /// <summary>
        /// Fonction de suppression d'un article dans la base de données 
        /// </summary>
        /// <param name="ElementASupprimer"></param>
        /// <returns></returns>
        public static string DelArticle(string ElementASupprimer)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");

            string ConncetionString = @"Data Source=" + DBPath + ";";

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();

                SQLiteCommand DeleteCommande = new SQLiteCommand("DELETE FROM Marques WHERE RefArticle='" + ElementASupprimer + "'", Database);

                DeleteCommande.ExecuteNonQuery();
            }

            return "La suppression à bien eu lieu";
        }

        /// <summary>
        /// Fonction de suppression d'une marque dans la base de données 
        /// </summary>
        /// <param name="ElementASupprimer"> L'élément qui va être supprimer</param>
        /// <returns>"La suppression à bien eu lieu" Si la suppression c'est bien passé, "Suppression impossible, la marque est utilisé dans la liste d'article" sinon</returns>
        public static string DelMarques(string ElementASupprimer)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");

            string ConncetionString = @"Data Source=" + DBPath + ";";

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))

            {
                Database.Open();

                SQLiteCommand SelectCommand = new SQLiteCommand("SELECT count(*) FROM Articles " +
                    "INNER JOIN Marques ON Marques.RefMarque = Articles.RefMarque " +
                    "WHERE Marques.Nom='" + ElementASupprimer+"'", Database);

                int Count = Convert.ToInt32(SelectCommand.ExecuteScalar());

                if (Count > 0)
                {

                    return "Suppression impossible, la marque est utilisé dans la liste d'article";
                
                }
                else
                {

                    SQLiteCommand DeleteCommande = new SQLiteCommand("DELETE FROM Marques WHERE Nom='" + ElementASupprimer + "'", Database);

                    DeleteCommande.ExecuteNonQuery();

                }
            }

            return "La suppression à bien eu lieu";
        }

        /// <summary>
        /// Fonction de suppression d'une famille dans la base de données
        /// </summary>
        /// <param name="ElementASupprimer">L'élément qui va être supprimer </param>
        /// <returns>"La suppression à bien eu lieu" Si la suppression c'est bien passé, "Suppression impossible, la famille à encore des sous familles de rattaché" sinon</returns>
        public static string DelFamilles(string ElementASupprimer)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");

            string ConncetionString = @"Data Source=" + DBPath + ";";

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();

                SQLiteCommand SelectCommand = new SQLiteCommand("SELECT count(*) FROM SousFamilles " +
                    "INNER JOIN Familles ON Familles.RefFamille = SousFamilles.RefFamille " +
                    "WHERE Familles.Nom='" + ElementASupprimer + "'", Database);

                int Count = Convert.ToInt32(SelectCommand.ExecuteScalar());
                
                if (Count > 0)
                {
                    
                    return "Suppression impossible, la famille à encore des sous familles de rattaché";

                }
                else
                {

                    SQLiteCommand DeleteCommande = new SQLiteCommand("DELETE FROM Familles WHERE Nom='" + ElementASupprimer + "'", Database);

                    DeleteCommande.ExecuteNonQuery();

                }
            }

            return "La suppression à bien eu lieu";
        }

        /// <summary>
        /// Fonction de suppression d'une sous-famille dans la base de données
        /// </summary>
        /// <param name="ElementASupprimer"> L'élément qui va être supprimer </param>
        /// <returns>"La suppression à bien eu lieu" Si la suppression c'est bien passé, "Suppression impossible, la sous-familles est utilisé dans la liste d'article" sinon</returns>
        public static string DelSousFamilles(string ElementASupprimer)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");

            string ConncetionString = @"Data Source=" + DBPath + ";";

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();

                SQLiteCommand SelectCommand = new SQLiteCommand("SELECT count(*) FROM Articles " +
                    "INNER JOIN SousFamilles ON SousFamilles.RefSousFamille = Artciles.RefSousFamille " +
                    "WHERE SousFamilles.Nom='" + ElementASupprimer + "'", Database);

                int Count = Convert.ToInt32(SelectCommand.ExecuteScalar());
                
                if (Count > 0)
                {

                    return "Suppression impossible, la sous-familles est utilisé dans la liste d'article";

                }
                else
                {

                    SQLiteCommand DeleteCommande = new SQLiteCommand("DELETE FROM SousFamilles WHERE Nom='" + ElementASupprimer + "'", Database);

                    DeleteCommande.ExecuteNonQuery();

                }
            }

            return "La suppression à bien eu lieu";
        }
        #endregion

    }
}
