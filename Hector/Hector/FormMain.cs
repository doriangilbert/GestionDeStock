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

                string RequeteSelectFamilles = "SELECT * FROM Familles";

                using (SQLiteCommand CommandeFamilles = new SQLiteCommand(RequeteSelectFamilles, Connexion))
                {
                    SQLiteDataReader LecteurFamilles = CommandeFamilles.ExecuteReader();

                    int IndiceNoeudFamille = 0;

                    while (LecteurFamilles.Read())
                    {
                        treeView1.BeginUpdate();
                        treeView1.Nodes[1].Nodes.Add(LecteurFamilles["Nom"].ToString());
                        treeView1.EndUpdate();
                        
                        string RequeteSelectSousFamilles = "SELECT * FROM SousFamilles WHERE RefFamille = @RefFamille";

                        using (SQLiteCommand CommandeSousFamilles = new SQLiteCommand(RequeteSelectSousFamilles, Connexion))
                        {
                            CommandeSousFamilles.Parameters.Add(new SQLiteParameter("@RefFamille", LecteurFamilles["RefFamille"].ToString()));

                            SQLiteDataReader LecteurSousFamilles = CommandeSousFamilles.ExecuteReader();

                            while (LecteurSousFamilles.Read())
                            {
                                treeView1.BeginUpdate();
                                treeView1.Nodes[1].Nodes[IndiceNoeudFamille].Nodes.Add(LecteurSousFamilles["Nom"].ToString());
                                treeView1.EndUpdate();
                            }
                        }

                        IndiceNoeudFamille++;
                    }
                }

                string RequeteSelectMarques = "SELECT * FROM Marques";

                using (SQLiteCommand CommandeMarques = new SQLiteCommand(RequeteSelectMarques, Connexion))
                {
                    SQLiteDataReader LecteurMarques = CommandeMarques.ExecuteReader();

                    while (LecteurMarques.Read())
                    {
                        treeView1.BeginUpdate();
                        treeView1.Nodes[2].Nodes.Add(LecteurMarques["Nom"].ToString());
                        treeView1.EndUpdate();
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
