using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Hector
{
    public partial class FormImport : Form
    {
        public FormImport()
        {
            InitializeComponent();
        }

        //Code for the folder browse button click event.
        private void btnDirectoryPath_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = txtDirectoryPath.Text;
            DialogResult drResult = folderBrowserDialog1.ShowDialog();
            if (drResult == DialogResult.OK)
                txtDirectoryPath.Text = folderBrowserDialog1.SelectedPath;
        }

        //Code for the ‘Load Directory’ button click event. In this function, we are clearing the old data such as resetting the progressbar value
        //and clearing Treeview node's data and then, we are calling our main function ‘LoadDirectory’.
        private void btnLoadDirectory_Click(object sender, EventArgs e)
        {
            //On initialise la barre de progression
            progressBar1.Value = 0;
            //On réinitialise le TreeView si jamais il n'était pas vide
            treeView1.Nodes.Clear();
            if (Directory.Exists(txtDirectoryPath.Text))
                LoadDirectory(txtDirectoryPath.Text);
            else
                MessageBox.Show("Select Directory!!");
        }

        //In this function, we are passing a root directory path which we are getting from the textbox in function parameter
        //and loading sub directories of the given path and creating nodes for the each respective directory, as shown in the below code. 
        public void LoadDirectory(string Dir)
        {
            DirectoryInfo di = new DirectoryInfo(Dir);
            //Setting ProgressBar Maximum Value
            progressBar1.Maximum = Directory.GetFiles(Dir, "*.*", SearchOption.AllDirectories).Length + Directory.GetDirectories(Dir, "**", SearchOption.AllDirectories).Length;
            TreeNode tds = treeView1.Nodes.Add(di.Name);
            tds.Tag = di.FullName;
            tds.StateImageIndex = 0;
            LoadFiles(Dir, tds);
            LoadSubDirectories(Dir, tds);
        }

        //In this function, we are passing a directory path in function parameter and loading sub directories of the given path
        //and creating nodes for the each respective directory and setting Image for the directory node, as Shown in the below Code,
        private void LoadSubDirectories(string dir, TreeNode td)
        {
            // Get all subdirectories
            string[] subdirectoryEntries = Directory.GetDirectories(dir);
            // Loop through them to see if they have any other subdirectories
            foreach (string subdirectory in subdirectoryEntries)
            {
                DirectoryInfo di = new DirectoryInfo(subdirectory);
                TreeNode tds = td.Nodes.Add(di.Name);
                tds.StateImageIndex = 0;
                tds.Tag = di.FullName;
                LoadFiles(subdirectory, tds);
                LoadSubDirectories(subdirectory, tds);
                UpdateProgress1();
            }
        }

        //In this function we are passing a directory path in function parameter and loading files of the given path
        //and creating nodes for the each respective file and setting Image for the file node, as Shown in the below Code,  
        private void LoadFiles(string dir, TreeNode td)
        {
            string[] Files = Directory.GetFiles(dir, "*.*");
            // Loop through them to see files
            foreach (string file in Files)
            {
                FileInfo fi = new FileInfo(file);
                TreeNode tds = td.Nodes.Add(fi.Name);
                tds.Tag = fi.FullName;
                tds.StateImageIndex = 1;
                UpdateProgress1();
            }
        }

        //Fonctions qui font avancer les barres de progression à chaque avancée
        private void UpdateProgress1()
        {
            if (progressBar1.Value < progressBar1.Maximum)
            {
                progressBar1.Value++;
                Application.DoEvents();
            }
        }
        private void UpdateProgress2()
        {
            if (progressBar2.Value < progressBar2.Maximum)
            {
                progressBar2.Value++;
                Application.DoEvents();
            }
        }

        //Fonction qui affiche dans labelFileName le nom du fichier cliqué dans la TreeView
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            labelFileName.Text = treeView1.SelectedNode.Text;
        }

        private void btnEcrase_Click(object sender, EventArgs e)
        {
            progressBar2.Maximum= 100;
            progressBar2.Value= 0;
            for (int i = 0; i < 100; i++)
            {
                progressBar2.Value++;
            }
        }

        private void btnAjout_Click(object sender, EventArgs e)
        {

        }
    }
}
