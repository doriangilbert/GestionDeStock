using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hector
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }


        private void FormMain_Load(object Sender, EventArgs Args)
        {
            using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
            {
                Connexion.Open();
                using (SQLiteCommand Commande = Connexion.CreateCommand())
                {
                    Commande.CommandText = "SELECT * FROM Familles";
                    Commande.CommandType = CommandType.Text;
                    SQLiteDataReader Lecteur = Commande.ExecuteReader();
                    while (Lecteur.Read())
                    {
                        Console.WriteLine(Lecteur["RefFamille"] + " " + Lecteur["Nom"]);
                    }
                }

            }
        }


        private void ActualiserToolStripMenuItem_Click(object Sender, EventArgs Args)
        {

        }


        private void ImporterToolStripMenuItem_Click_1(object Sender, EventArgs Args)
        {
            FormImport FenetreImportation = new FormImport();
            FenetreImportation.ShowDialog();
        }


        private void ExporterToolStripMenuItem_Click(object Sender, EventArgs Args)
        {

        }
    }
}
