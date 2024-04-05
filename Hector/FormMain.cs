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
            this.statusStrip1.Items.Add("Nombre d'articles dans la base de données : "+HectorSQL.GetNumberArticles().ToString());
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            lvwColumnSorter = new ListViewColumnSorter();
            this.listView1.ListViewItemSorter = lvwColumnSorter;
            //_ =this.listView1.Sorting;
            listView1.Clear();
            listView1.Groups.Clear();
            listView1.Sorting = SortOrder.None;
            if (e.Node.Parent != null)
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

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            listView1.Groups.Clear();
            List<string> itemChoises = new List<string>();
            //Pour chaque item dans la liste 
            //Si on as une nouvelle desciptions, on l'ajoute à notre liste, on créer un groupe, et on ajoute l'item à notre groupe
            //Sinon, on ajoute l'item au bon groupe. 
            int column = e.Column;
            int x = 0;
            foreach (ListViewItem item in listView1.Items)
            {
                //Si on as une nouvelle desciptions, on l'ajoute à notre liste, on créer un groupe, et on ajoute l'item à notre groupe
                if (!itemChoises.Contains(item.SubItems[column].Text))
                {
                    itemChoises.Add(item.SubItems[column].Text);
                    listView1.Groups.Add(new ListViewGroup(item.SubItems[column].Text, HorizontalAlignment.Left));
                    item.Group = listView1.Groups[x];

                    x++;
                }
                //Sinon, on ajoute l'item au bon groupe.
                else
                {
                    int numéroGroupe = itemChoises.IndexOf(item.SubItems[column].Text);
                    item.Group = listView1.Groups[numéroGroupe];
                   
                }
                //listView1.Groups.Add(new ListViewGroup(, HorizontalAlignment.Left));
            }
            // Determine if clicked column is already the column that is being sorted.
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

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("Noeud enfant cliquer : " + e.Node.Name);
            ListViewHitTestInfo info = listView1.HitTest(e.X, e.Y);
            ListViewItem item = info.Item;

            if (item != null)
            {
                MessageBox.Show("Fonction d'ajout ou de modification indisponible");
            }
            else
            {
                this.listView1.SelectedItems.Clear();
            }
        }

        private void actualiserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            lvwColumnSorter = new ListViewColumnSorter();
            this.listView1.ListViewItemSorter = lvwColumnSorter;
            HectorSQL.InitialiseDatabase(treeView1);
            this.statusStrip1.Items.Add("Nombre d'articles dans la base de données : " + HectorSQL.GetNumberArticles().ToString());
        }

        private void listView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(listView1.SelectedItems.Count >0 )
            {
                //MessageBox.Show("C'est la touche : " + e.KeyChar+" qui est touché ");
                if (e.KeyChar == (char)Keys.Return)
                {
                    MessageBox.Show("Fonction d'ajout ou de modification indisponible");
                }
                if (e.KeyChar == (char)Keys.Space)
                {
                    MessageBox.Show("Fonction d'ajout ou de modification indisponible");
                }
                if (e.KeyChar == (char)Keys.Back)
                {
                    //MessageBox.Show("C'est la touche : Supp qui est touché avec l'item "+ listView1.SelectedItems[0].Text);
                    var Resultat = MessageBox.Show("Voulez vous vraiment supprimé l'élément séléctionner ? ", "Suppresion d'élément", MessageBoxButtons.OKCancel);
                    if (Resultat == DialogResult.OK)
                    {
                        string ResultatSuppression = HectorSQL.DelElements(listView1.SelectedItems[0].Text, treeView1.SelectedNode);
                        MessageBox.Show(ResultatSuppression);
                    }
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Fonction d'ajout ou de modification indisponible");
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Fonction de suppression par menu indisponible");
        }

        private void exporterToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
    }
}
