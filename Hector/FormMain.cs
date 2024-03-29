using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;


namespace Hector
{

    public partial class FormMain : Form
    {
        private ListViewColumnSorter lvwColumnSorter;

        public FormMain()
        {

            InitializeComponent();
            // Create an instance of a ListView column sorter and assign it
            // to the ListView control.
            lvwColumnSorter = new ListViewColumnSorter();
            this.listView1.ListViewItemSorter = lvwColumnSorter;
            HectorSQL.InitialiseDatabase(treeView1);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(e.Node.Parent != null)
            {
                //MessageBox.Show("Noeud enfant cliquer : " + e.Node.Name);
                if(e.Node.Parent.Name == "NodeFamille")
                {
                    //MessageBox.Show("Noeud sousfamille cliequer : " + e.Node.Text);
                    HectorSQL.GetSousFamilles(listView1, e.Node.Text);
                }
                else
                {
                    if (e.Node.Parent.Name == "NodeMarque")
                    {
                        HectorSQL.GetArticlesByMarque(listView1, e.Node.Text);
                    }
                    else
                    {
                        if (e.Node.Parent.Name != null)
                        {
                            HectorSQL.GetArticlesBySousFamille(listView1, e.Node.Text);
                        }
                    }
                }
            }
            else
            {
                switch (e.Node.Name)
                {
                    case "NodeArticle":
                        HectorSQL.GetArticles(listView1);
                        break;
                    case "NodeFamille":
                        HectorSQL.GetFamilles(listView1);
                        break;
                    case "NodeMarque":
                        HectorSQL.GetMarques(listView1);
                        break;

                }
            }
            
        }

        private void importerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormImport formImport = new FormImport();
            formImport.StartPosition = FormStartPosition.CenterParent;
            formImport.ShowDialog(this);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {// Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listView1.Sort();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            
        }
    }
}
