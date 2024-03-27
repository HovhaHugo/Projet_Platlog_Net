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


        public static void GetFamilles(ListView listView1)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");
            string ConncetionString = @"Data Source=" + DBPath + ";";

            listView1.Clear();
            listView1.Columns.Add("Référence", 48, HorizontalAlignment.Left);
            listView1.Columns.Add("Nom", 100, HorizontalAlignment.Left);
            listView1.GridLines = true;

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();
                SQLiteCommand selectCommand = new SQLiteCommand("SELECT * FROM Familles");
                selectCommand.Connection = Database;

                SQLiteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    ListViewItem item = new ListViewItem(query.GetInt32(0).ToString());
                    item.SubItems.Add(query.GetString(1).ToString());

                    listView1.Items.Add(item);
                    //MessageBox.Show(query.GetString(0).ToString());
                }
            }
        }

        public static void GetSousFamilles(ListView listView1, String nomNode)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");
            string ConncetionString = @"Data Source=" + DBPath + ";";

            listView1.Clear();
            listView1.Columns.Add("Référence", 48, HorizontalAlignment.Left);
            listView1.Columns.Add("Famille", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Nom", 100, HorizontalAlignment.Left);
            listView1.GridLines = true;

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();
                SQLiteCommand selectCommand = new SQLiteCommand("SELECT RefSousFamille, Familles.Nom, SousFamilles.Nom " +
                    "FROM SousFamilles INNER JOIN Familles ON Familles.RefFamille = SousFamilles.RefFamille " +
                    "WHERE Familles.Nom = '"+ nomNode + "'");
                selectCommand.Connection = Database;

                SQLiteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    ListViewItem item = new ListViewItem(query.GetInt32(0).ToString());
                    item.SubItems.Add(query.GetString(1).ToString());
                    item.SubItems.Add(query.GetString(2).ToString());

                    listView1.Items.Add(item);
                    //MessageBox.Show(query.GetString(0).ToString());
                }
            }
        }

        public static void GetArticles(ListView listView1)
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
                    "INNER JOIN Marques ON Marques.RefMarque = Articles.RefMarque ");
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

        public static void GetMarques(ListView listView1)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");
            string ConncetionString = @"Data Source=" + DBPath + ";";

            listView1.Clear();
            listView1.Columns.Add("Id", 48, HorizontalAlignment.Left);
            listView1.Columns.Add("Nom", 100, HorizontalAlignment.Left);
            listView1.GridLines = true;

            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();
                SQLiteCommand selectCommand = new SQLiteCommand("SELECT * FROM Marques");
                selectCommand.Connection = Database;

                SQLiteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    ListViewItem item = new ListViewItem(query.GetInt32(0).ToString());
                    item.SubItems.Add(query.GetString(1).ToString());

                    listView1.Items.Add(item);
                    //MessageBox.Show(query.GetString(0).ToString());
                }
            }
        }




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
                    "WHERE Familles.Nom == '" + nomNode+"'");
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

    }
}
