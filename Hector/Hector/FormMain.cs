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

        private void importerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MaFenetreModale fenetreModale = new MaFenetreModale();
            //fenetreModale.ShowDialog();

            //if (fenetreModale.ShowDialog() == DialogResult.OK)
            //{
            //    var resultat = fenetreModale.Result;
            //    // Utilisez le résultat comme nécessaire
            //}
        }

        private void exporterToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
