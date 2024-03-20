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

        public static void InitialisezDatabase()
        {
            string DBPath = Path.Combine(Application.StartupPath, "Hector.SQLite");
            using (SQLiteConnection Database = new SQLiteConnection($"Filename={DBPath}"))
            {
                Database.Open();

                SQLiteCommand selectCommand = new SQLiteCommand("SELECT * FROM Familles");

            }

    }

}
}
