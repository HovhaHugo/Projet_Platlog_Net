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

        public static void InitialiseDatabase()
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");
            string ConncetionString = @"Data Source="+DBPath+";";
            //using (SQLiteConnection Database = new SQLiteConnection($"Filename={DBPath}"))
            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();
            }
        }


        public static void GetNomFamille(ListView listView1)
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");
            string ConncetionString = @"Data Source=" + DBPath + ";";
            listView1.Columns.Add("Id", 2, HorizontalAlignment.Left);
            listView1.Columns.Add("Nom", 2, HorizontalAlignment.Left);
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
    }
}
