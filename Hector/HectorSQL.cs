using System;
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

        public static bool InitialisezDatabase()
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");
            string ConncetionString = @"Data Source="+DBPath+";";
            //using (SQLiteConnection Database = new SQLiteConnection($"Filename={DBPath}"))
            using (SQLiteConnection Database = new SQLiteConnection(ConncetionString))
            {
                Database.Open();

                SQLiteCommand selectCommand = new SQLiteCommand("SELECT Nom FROM Familles");
                selectCommand.Connection = Database;

                SQLiteDataReader query = selectCommand.ExecuteReader();


                if (query.Read())
                {
                    MessageBox.Show(query.GetString(0).ToString());
                }

                /*while (query.Read())
                {
                    MessageBox.Show(query.GetString(0).ToString());
                }*/
                return true;

            }

    }

}
}
