using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hector
{
    public partial class FormImport : Form
    {
        public FormImport()
        {
            InitializeComponent();
        }

        private void ChoisirFichier_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog
            {
                // On instancie un objet de type OpenFileDialog avec comme filtre des fichiers de type "csv"
                // Nous pourrons lire le fichier avec cet objet.
                Filter = "csv files (*.csv)|*.csv"
            };

            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Obtenir le chemin du fichier à ouvrir
                string filePath = OpenFileDialog.FileName;
                FichierChoisi.Text = Path.GetFileName(filePath);
            }
        }

        private void EcraserDonnees_Click(object sender, EventArgs e)
        {

        }

        private void AjoutDonnees_Click(object sender, EventArgs e)
        {

        }
    }
}
