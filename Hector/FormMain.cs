using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Hector
{

    public partial class FormMain : Form
    {
        private ListViewColumnSorter LvwColumnSorter;

        public FormMain()
        {

            InitializeComponent();

            LvwColumnSorter = new ListViewColumnSorter();

            this.listView1.ListViewItemSorter = LvwColumnSorter;

            HectorSQL.InitialiseDatabase(treeView1);

            this.statusStrip1.Items.Add("Nombre d'articles dans la base de données : "+HectorSQL.GetNumberArticles().ToString());

        }

        /// <summary>
        /// Fonction qui permet d'afficher les informations souhaiter en fonction du noeuds séléctionner. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Le noeuds séléctionner</param>
        private void SelectionNoeud(object sender, TreeViewEventArgs e)
        {
            //On créer un nouveau sorter pour faire en sorte de repartie sur de bonne base
            LvwColumnSorter = new ListViewColumnSorter();

            this.listView1.ListViewItemSorter = LvwColumnSorter;


            //On remet tout à zéro pour être sûr de ne pas avoir de problème
            listView1.Clear();

            listView1.Groups.Clear();

            listView1.Sorting = SortOrder.None;

            //En fonction du noeuds cliquer, on affiche les infomations voulu 
            if (e.Node.Parent != null)
            {

                if(e.Node.Parent.Name == "NodeFamille")
                {

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

        /// <summary>
        /// Fonction qui permet la trie des informations par colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrieParColonne(object sender, ColumnClickEventArgs e)
        {
            listView1.Groups.Clear();
            List<string> itemChoises = new List<string>();

            int column = e.Column;

            int x = 0;
            //Pour chaque item dans la liste 
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
            }
            // Determine si la colonnes cliquer est déjà trier.
            if (e.Column == LvwColumnSorter.SortColumn)
            {
                // Si c'est la cas, on la trie dans l'autre sens.
                if (LvwColumnSorter.Order == SortOrder.Ascending)
                {

                    LvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {

                    LvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {

                LvwColumnSorter.SortColumn = e.Column;

                LvwColumnSorter.Order = SortOrder.Ascending;
            }

            // On fait le trie.

            this.listView1.Sort();
        }

        /// <summary>
        /// Fonction qui permet de gére le double click sur les éléments de la listView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"><b>MouseEventArgs</b> qui contient toutes les informations sur la positions de la souris</param>
        private void ItemDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = listView1.HitTest(e.X, e.Y);

            //On récupére les informations de l'item grâce à la position de la souris sur le listview

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

        /// <summary>
        /// Fonction qui permet d'actualiser les données avec celles qu'on as dans le dossier de l'éxécutable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Actualiser(object sender, EventArgs e)
        {
            //On remet tout à zéro, on vide tout et on récupére les informations dans le fichier Hector.SQLite

            listView1.Clear();

            LvwColumnSorter = new ListViewColumnSorter();

            this.listView1.ListViewItemSorter = LvwColumnSorter;

            HectorSQL.InitialiseDatabase(treeView1);

            this.statusStrip1.Items.Clear();

            this.statusStrip1.Items.Add("Nombre d'articles dans la base de données : " + HectorSQL.GetNumberArticles().ToString());
        }

        /// <summary>
        /// Fonction permettant de réagir en fonction de la saisie d'une touche sur le clavier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">La lettre/le symbole</param>
        private void PressionToucheClavier(object sender, KeyPressEventArgs e)
        {
            //Si on as au moins 1 items de selectionner, on avance

            if(listView1.SelectedItems.Count >0 )
            {
                if (e.KeyChar == (char)Keys.Return)
                {

                    MessageBox.Show("Fonction d'ajout ou de modification indisponible");
                
                }
                if (e.KeyChar == (char)Keys.Space)
                {

                    MessageBox.Show("Fonction d'ajout ou de modification indisponible");
                
                }

                //Si c'est la touche supp/back qui as été touché, on va faire la suppresson de l'élément

                if (e.KeyChar == (char)Keys.Back)
                {

                    var Resultat = MessageBox.Show("Voulez vous vraiment supprimé l'élément séléctionner ? ", "Suppresion d'élément", MessageBoxButtons.OKCancel);
                    
                    if (Resultat == DialogResult.OK)
                    {
                        
                        string ResultatSuppression = HectorSQL.DelElements(listView1.SelectedItems[0].Text, treeView1.SelectedNode);

                        MessageBox.Show(ResultatSuppression);

                        LvwColumnSorter = new ListViewColumnSorter();

                        //Récatualisation des informations 

                        listView1.Clear();

                        this.listView1.ListViewItemSorter = LvwColumnSorter;

                        HectorSQL.InitialiseDatabase(treeView1);

                        this.statusStrip1.Items.Clear();

                        this.statusStrip1.Items.Add("Nombre d'articles dans la base de données : " + HectorSQL.GetNumberArticles().ToString());
                    }
                }
            }
        }
        
        /// <summary>
        /// Fonction qui permet d'ajouter un élément dans la base de donnée via le menu contextuel
        /// </summary>
        /// <param name="sender">L'object qui nous envoie l'information</param>
        /// <param name="e"></param>
        private void AjoutSouris(object sender, EventArgs e)
        {
            MessageBox.Show("Fonction d'ajout ou de modification indisponible");
        }

        /// <summary>
        /// Fonction qui permet de supprimer un élément dans la base de donnée via le menu contextuel
        /// </summary>
        /// <param name="sender">L'object qui nous envoie l'information</param>
        /// <param name="e"></param>
        private void SuppressionSouris(object sender, EventArgs e)
        {
            MessageBox.Show("Fonction de suppression par menu indisponible");
        }

        private void exporterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormExport formExport = new FormExport();
            formExport.StartPosition = FormStartPosition.CenterParent;
            formExport.ShowDialog(this);
        }
    }
}
