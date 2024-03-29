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
        /// <param name="treeView1"></param>
        public static void InitialiseDatabase(TreeView treeView1)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");
            string ConncetionString = @"Data Source="+DBPath+";";
            //using (SQLiteConnection Database = new SQLiteConnection($"Filename={DBPath}"))

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();

                foreach (TreeNode NodePrincipal in treeView1.Nodes)
                {
                    if (NodePrincipal.Name == "NodeFamille")
                    {
                        SQLiteCommand selectCommandFamille = new SQLiteCommand("SELECT * FROM Familles");
                        SQLiteCommand selectCommandSousFamille = new SQLiteCommand("SELECT * FROM SousFamilles INNER JOIN Familles On Familles.RefFamille = SousFamilles.RefFamille");
                        selectCommandFamille.Connection = Database;
                        selectCommandSousFamille.Connection = Database;

                        SQLiteDataReader query = selectCommandFamille.ExecuteReader();
                        SQLiteDataReader querySousFamille = selectCommandSousFamille.ExecuteReader();
                        while (query.Read())
                        {
                            TreeNode NodeFamille = new TreeNode(query.GetString(1).ToString());

                            NodePrincipal.Nodes.Add(NodeFamille);
                        }
                        while (querySousFamille.Read())
                        {
                            TreeNode NodeSousFamille = new TreeNode(querySousFamille.GetString(2).ToString());
                            foreach(TreeNode NodeFamille in NodePrincipal.Nodes)
                            {
                                if(querySousFamille.GetString(4).ToString() == NodeFamille.Text)
                                {
                                    NodeFamille.Nodes.Add(NodeSousFamille);
                                }
                            }
                        }

                    }
                    if (NodePrincipal.Name == "NodeMarque")
                    {
                        SQLiteCommand selectCommand = new SQLiteCommand("SELECT * FROM Marques");
                        selectCommand.Connection = Database;

                        SQLiteDataReader query = selectCommand.ExecuteReader();

                        while (query.Read())
                        {
                            TreeNode NodeMarque = new TreeNode(query.GetString(1).ToString());

                            NodePrincipal.Nodes.Add(NodeMarque);
                        }
                    }
                }
            }
        }

        #region Getteurs de base

        /// <summary>
        /// Permet de récupérer la liste des Familles et les mets dans le listViews
        /// </summary>
        /// <param name="listView1"> Le listview à modifier </param>
        public static void GetFamilles(ListView listView1)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");
            string ConncetionString = @"Data Source=" + DBPath + ";";

            listView1.Clear();
            //listView1.Columns.Add("Référence", 48, HorizontalAlignment.Left);
            listView1.Columns.Add("Description", -2, HorizontalAlignment.Left);
            listView1.GridLines = true;

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();
                SQLiteCommand selectCommand = new SQLiteCommand("SELECT * FROM Familles");
                selectCommand.Connection = Database;

                SQLiteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    ListViewItem item = new ListViewItem(query.GetString(1).ToString());
                    //item.SubItems.Add(query.GetString(1).ToString());

                    listView1.Items.Add(item);
                    //MessageBox.Show(query.GetString(0).ToString());
                }
            }
        }

        /// <summary>
        /// Permet de récupérer la liste des SousFamilles et les mets dans le listViews
        /// </summary>
        /// <param name="listView1"> Le listview à modifier </param>
        /// <param name="nomNode">Le nom de la famille parent à la sous famille</param>
        public static void GetSousFamilles(ListView listView1, String nomNode)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");
            string ConncetionString = @"Data Source=" + DBPath + ";";

            listView1.Clear();
            //listView1.Columns.Add("Référence", 48, HorizontalAlignment.Left);
            //listView1.Columns.Add("Famille", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Description", -2, HorizontalAlignment.Left);
            listView1.GridLines = true;

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();
                SQLiteCommand selectCommand = new SQLiteCommand("SELECT RefSousFamille, Familles.Nom, SousFamilles.Nom " +
                    "FROM SousFamilles INNER JOIN Familles ON Familles.RefFamille = SousFamilles.RefFamille " +
                    "WHERE Familles.Nom = '" + nomNode + "'");
                selectCommand.Connection = Database;

                SQLiteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    ListViewItem item = new ListViewItem(query.GetString(2).ToString());
                    //item.SubItems.Add(query.GetString(1).ToString());
                    //item.SubItems.Add(query.GetString(2).ToString());

                    listView1.Items.Add(item);
                    //MessageBox.Show(query.GetString(0).ToString());
                }
            }
        }

        /// <summary>
        ///  Permet de récupérer la liste des Articles et les mets dans le listViews
        /// </summary>
        /// <param name="listView1"> Le listview à modifier </param>
        public static void GetArticles(ListView listView1)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");
            string ConncetionString = @"Data Source=" + DBPath + ";";

            listView1.Clear();
            listView1.Columns.Add("Reférence",90, HorizontalAlignment.Left);
            //listView1.Groups.Add(new ListViewGroup("Référence", HorizontalAlignment.Left));
            listView1.Columns.Add("Description",150, HorizontalAlignment.Left);
            //listView1.Groups.Add(new ListViewGroup("Description", HorizontalAlignment.Left));
            listView1.Columns.Add("Famille",150, HorizontalAlignment.Left);
            //listView1.Groups.Add(new ListViewGroup("Famille", HorizontalAlignment.Left));
            listView1.Columns.Add("Sous-Famille",150, HorizontalAlignment.Left);
            //listView1.Groups.Add(new ListViewGroup("Sous-Famille", HorizontalAlignment.Left));
            listView1.Columns.Add("Marque",150, HorizontalAlignment.Left);
            //listView1.Groups.Add(new ListViewGroup("Marque", HorizontalAlignment.Left));
            listView1.Columns.Add("Quantite",80, HorizontalAlignment.Left);
            //listView1.Groups.Add(new ListViewGroup("Quantite", HorizontalAlignment.Left));
            listView1.GridLines = true;

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();
                SQLiteCommand selectCommand = new SQLiteCommand("SELECT RefArticle, Description, " +
                    "Familles.Nom, SousFamilles.Nom, Marques.Nom, Quantite " +
                    "FROM Articles " +
                    "INNER JOIN SousFamilles ON SousFamilles.RefSousFamille = Articles.RefSousFamille " +
                    "INNER JOIN Familles ON Familles.RefFamille = SousFamilles.RefFamille " +
                    "INNER JOIN Marques ON Marques.RefMarque = Articles.RefMarque ");
                selectCommand.Connection = Database;

                SQLiteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    ListViewItem item = new ListViewItem(query.GetString(0).ToString());
                    /*ListViewItem itemDesc = new ListViewItem(query.GetString(1).ToString());
                    ListViewItem itemFam = new ListViewItem(query.GetString(2).ToString());
                    ListViewItem itemSousFam = new ListViewItem(query.GetString(3).ToString());
                    ListViewItem itemMarque = new ListViewItem(query.GetString(4).ToString());
                    ListViewItem itemQuantite = new ListViewItem(query.GetValue(5).ToString());*/
                    item.SubItems.Add(query.GetString(1).ToString());
                    item.SubItems.Add(query.GetString(2).ToString());
                    item.SubItems.Add(query.GetString(3).ToString());
                    item.SubItems.Add(query.GetString(4).ToString());
                    item.SubItems.Add(query.GetValue(5).ToString());

                    /*itemDesc.Group = listView1.Groups[1];
                    itemFam.Group = listView1.Groups[2];
                    itemSousFam.Group = listView1.Groups[3];
                    itemMarque.Group = listView1.Groups[4];
                    itemQuantite.Group = listView1.Groups[5];*/
                    listView1.Items.Add(item);

                    /*listView1.Items.Add(itemDesc);
                    listView1.Items.Add(itemFam);
                    listView1.Items.Add(itemSousFam);
                    listView1.Items.Add(itemMarque); 
                    listView1.Items.Add(itemQuantite);*/
                    //MessageBox.Show(query.GetString(0).ToString());
                }
            }
        }


        /// <summary>
        /// Permet de récupérer la liste des Marques et les mets dans le listViews
        /// </summary>
        /// <param name="listView1"> Le listview à modifier </param>
        public static void GetMarques(ListView listView1)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");
            string ConncetionString = @"Data Source=" + DBPath + ";";

            listView1.Clear();
            //listView1.Columns.Add("Id", 48, HorizontalAlignment.Left);
            listView1.Columns.Add("Description", -2, HorizontalAlignment.Left);
            listView1.GridLines = true;

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();
                SQLiteCommand selectCommand = new SQLiteCommand("SELECT * FROM Marques");
                selectCommand.Connection = Database;

                SQLiteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    ListViewItem item = new ListViewItem(query.GetString(1).ToString());
                    //item.SubItems.Add(query.GetString(1).ToString());

                    listView1.Items.Add(item);
                    //MessageBox.Show(query.GetString(0).ToString());
                }
            }
        }

        #endregion

        #region GetArticlesBy...

        /// <summary>
        /// Permet de récupérer la liste des Articles trié par la Famille séléctionné 
        /// </summary>
        /// <param name="listView1"> Le listview qui a être modifier</param>
        /// <param name="nomNode">Le nom de la famille qui à été séléctionner</param>
        public static void GetArticlesByFamille(ListView listView1, String nomNode)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");
            string ConncetionString = @"Data Source=" + DBPath + ";";

            listView1.Clear();
            listView1.Columns.Add("Reférence", 48, HorizontalAlignment.Left);
            listView1.Columns.Add("Description", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Sous-Famille", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Marque", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Prix", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Quantite", 100, HorizontalAlignment.Left);
            listView1.GridLines = true;

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();
                SQLiteCommand selectCommand = new SQLiteCommand("SELECT RefArticle, Description, " +
                    "SousFamilles.Nom, Marques.Nom, PrixHT, Quantite " +
                    "FROM Articles " +
                    "INNER JOIN SousFamilles ON SousFamilles.RefSousFamille = Articles.RefSousFamille " +
                    "INNER JOIN Marques ON Marques.RefMarque = Articles.RefMarque " +
                    "INNER JOIN Familles ON Familles.RefFamille = SousFamilles.RefFamille " +
                    "WHERE Familles.Nom == '" + nomNode + "'");
                selectCommand.Connection = Database;

                SQLiteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    ListViewItem item = new ListViewItem(query.GetString(0).ToString());
                    item.SubItems.Add(query.GetString(1).ToString());
                    item.SubItems.Add(query.GetString(2).ToString());
                    item.SubItems.Add(query.GetString(3).ToString());
                    item.SubItems.Add(query.GetValue(4).ToString());
                    item.SubItems.Add(query.GetValue(5).ToString());

                    listView1.Items.Add(item);
                    //MessageBox.Show(query.GetString(0).ToString());
                }
            }
        }

        /// <summary>
        /// Permet de récupérer la liste des Articles trié par la Marque séléctionné
        /// </summary>
        /// <param name="listView1"> Le listview qui a être modifier </param>
        /// <param name="nomNode">Le nom de la marque qui à été séléctionner</param>
        public static void GetArticlesByMarque(ListView listView1, String nomNode)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");
            string ConncetionString = @"Data Source=" + DBPath + ";";

            listView1.Clear();
            listView1.Columns.Add("Reférence", 48, HorizontalAlignment.Left);
            listView1.Columns.Add("Description", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Sous-Famille", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Marque", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Prix", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Quantite", 100, HorizontalAlignment.Left);
            listView1.GridLines = true;

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();
                SQLiteCommand selectCommand = new SQLiteCommand("SELECT RefArticle, Description, " +
                    "SousFamilles.Nom, Marques.Nom, PrixHT, Quantite " +
                    "FROM Articles " +
                    "INNER JOIN SousFamilles ON SousFamilles.RefSousFamille = Articles.RefSousFamille " +
                    "INNER JOIN Marques ON Marques.RefMarque = Articles.RefMarque " +
                    "WHERE Marques.Nom == '" + nomNode + "'");
                selectCommand.Connection = Database;

                SQLiteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    ListViewItem item = new ListViewItem(query.GetString(0).ToString());
                    item.SubItems.Add(query.GetString(1).ToString());
                    item.SubItems.Add(query.GetString(2).ToString());
                    item.SubItems.Add(query.GetString(3).ToString());
                    item.SubItems.Add(query.GetValue(4).ToString());
                    item.SubItems.Add(query.GetValue(5).ToString());

                    listView1.Items.Add(item);
                    //MessageBox.Show(query.GetString(0).ToString());
                }
            }
        }

        /// <summary>
        /// Permet de récupérer la liste des Articles trié par la SousFamille séléctionné
        /// </summary>
        /// <param name="listView1"> Le listview qui a être modifier </param>
        /// <param name="nomNode"> Le nom de la marque qui à été séléctionner </param>
        public static void GetArticlesBySousFamille(ListView listView1, String nomNode)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");
            string ConncetionString = @"Data Source=" + DBPath + ";";

            listView1.Clear();
            listView1.Columns.Add("Reférence", 48, HorizontalAlignment.Left);
            listView1.Columns.Add("Description", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Sous-Famille", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Marque", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Prix", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Quantite", 100, HorizontalAlignment.Left);
            listView1.GridLines = true;

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();
                SQLiteCommand selectCommand = new SQLiteCommand("SELECT RefArticle, Description, " +
                    "SousFamilles.Nom, Marques.Nom, PrixHT, Quantite " +
                    "FROM Articles " +
                    "INNER JOIN SousFamilles ON SousFamilles.RefSousFamille = Articles.RefSousFamille " +
                    "INNER JOIN Marques ON Marques.RefMarque = Articles.RefMarque " +
                    "WHERE SousFamilles.Nom == '" + nomNode + "'");
                selectCommand.Connection = Database;

                SQLiteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    ListViewItem item = new ListViewItem(query.GetString(0).ToString());
                    item.SubItems.Add(query.GetString(1).ToString());
                    item.SubItems.Add(query.GetString(2).ToString());
                    item.SubItems.Add(query.GetString(3).ToString());
                    item.SubItems.Add(query.GetValue(4).ToString());
                    item.SubItems.Add(query.GetValue(5).ToString());

                    listView1.Items.Add(item);
                    //MessageBox.Show(query.GetString(0).ToString());
                }
            }
        }

        #endregion
    }
}
