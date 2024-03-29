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
    /// <summary>
    /// Classe de la fenêtre principale de l'application.
    /// </summary>
    public partial class FormMain : Form
    {
        /// <summary>
        /// Pour lancer la fenêtre (constructeur par défaut)
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sender">Objet <b>Object</b> prend en charge les éventuels objets que l'on renseigne en paramètre.</param>
        /// <param name="Args">Objet <b>EventArgs</b> contient les informations sur l'évènement.</param>
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

            ToolStripStatusLabel StatusLabelNbArticles = new ToolStripStatusLabel();
            using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
            {
                Connexion.Open();

                string Requete = "SELECT COUNT(*) AS NbArticles FROM Articles";

                using (SQLiteCommand Commande = new SQLiteCommand(Requete, Connexion))
                {
                    SQLiteDataReader Lecteur = Commande.ExecuteReader();

                    while (Lecteur.Read())
                    {
                        StatusLabelNbArticles.Text = Lecteur["NbArticles"].ToString() + " Articles";
                    }
                }
            }
            statusStrip1.Items.Add(StatusLabelNbArticles);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sender">Objet <b>Object</b> prend en charge les éventuels objets que l'on renseigne en paramètre.</param>
        /// <param name="Args">Objet <b>EventArgs</b> contient les informations sur l'évènement.</param>
        private void ActualiserToolStripMenuItem_Click(object Sender, EventArgs Args)
        {

        }


        /// <summary>
        /// Permet d'ouvrir une fenêtre modale d'importation de fichiers et de données pour la base de données.
        /// </summary>
        /// <param name="Sender">Objet <b>Object</b> prend en charge les éventuels objets que l'on renseigne en paramètre.</param>
        /// <param name="Args">Objet <b>EventArgs</b> contient les informations sur l'évènement.</param>
        private void ImporterToolStripMenuItem_Click_1(object Sender, EventArgs Args)
        {
            FormImport FenetreImportation = new FormImport();
            FenetreImportation.ShowDialog();
        }


        /// <summary>
        /// Permet d'ouvrir une fenêtre modale d'exportation de des données de la base de données dans un fichier.
        /// </summary>
        /// <param name="Sender">Objet <b>Object</b> prend en charge les éventuels objets que l'on renseigne en paramètre.</param>
        /// <param name="Args">Objet <b>EventArgs</b> contient les informations sur l'évènement.</param>
        private void ExporterToolStripMenuItem_Click(object Sender, EventArgs Args)
        {

        }

        private void treeView1_AfterSelect(object Sender, TreeViewEventArgs Args)
        {
            if(Args.Node.Text == "Tous les articles")
            {
                listView1.Columns.Clear();
                listView1.Items.Clear();

                listView1.Columns.Add("RefArticle");
                listView1.Columns.Add("Description");
                listView1.Columns.Add("Familles");
                listView1.Columns.Add("Sous-familles");
                listView1.Columns.Add("Marques");
                listView1.Columns.Add("Prix");
                listView1.Columns.Add("Quantité");

                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                foreach(ColumnHeader EnTeteColonne in listView1.Columns)
                {
                    EnTeteColonne.Width = listView1.Width / listView1.Columns.Count;
                }

                using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
                {
                    Connexion.Open();

                    string Requete = "SELECT RefArticle, Description, Familles.Nom AS NomFamille, SousFamilles.Nom AS NomSousFamille, Marques.Nom AS NomMarque, PrixHT, Quantite FROM Articles, SousFamilles, Familles, Marques"
                                                    + " WHERE Articles.RefSousFamille = SousFamilles.RefSousFamille"
                                                    + " AND SousFamilles.RefFamille = Familles.RefFamille"
                                                    + " AND Articles.RefMarque = Marques.RefMarque";

                    using (SQLiteCommand Commande = new SQLiteCommand(Requete, Connexion))
                    {
                        SQLiteDataReader Lecteur = Commande.ExecuteReader();

                        while (Lecteur.Read())
                        {
                            ListViewItem ItemListe = new ListViewItem(new string[] { Lecteur["RefArticle"].ToString(), Lecteur["Description"].ToString(), Lecteur["NomFamille"].ToString(), Lecteur["NomSousFamille"].ToString(), Lecteur["NomMarque"].ToString(), Lecteur["PrixHT"].ToString(), Lecteur["Quantite"].ToString() });
                            listView1.Items.Add(ItemListe);
                        }
                    }
                }
            }
            else if (Args.Node.Text == "Familles")
            {
                listView1.Columns.Clear();
                listView1.Items.Clear();

                listView1.Columns.Add("Description");

                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                foreach (ColumnHeader EnTeteColonne in listView1.Columns)
                {
                    EnTeteColonne.Width = listView1.Width / listView1.Columns.Count;
                }

                using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
                {
                    Connexion.Open();

                    string Requete = "SELECT Nom FROM Familles";

                    using (SQLiteCommand Commande = new SQLiteCommand(Requete, Connexion))
                    {
                        SQLiteDataReader Lecteur = Commande.ExecuteReader();

                        while (Lecteur.Read())
                        {
                            ListViewItem ItemListe = new ListViewItem(new string[] { Lecteur["Nom"].ToString() });
                            listView1.Items.Add(ItemListe);
                        }
                    }
                }
            }
            else if (Args.Node.Text == "Marques")
            {
                listView1.Columns.Clear();
                listView1.Items.Clear();

                listView1.Columns.Add("Description");

                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                foreach (ColumnHeader EnTeteColonne in listView1.Columns)
                {
                    EnTeteColonne.Width = listView1.Width / listView1.Columns.Count;
                }

                using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
                {
                    Connexion.Open();

                    string Requete = "SELECT Nom FROM Marques";

                    using (SQLiteCommand Commande = new SQLiteCommand(Requete, Connexion))
                    {
                        SQLiteDataReader Lecteur = Commande.ExecuteReader();

                        while (Lecteur.Read())
                        {
                            ListViewItem ItemListe = new ListViewItem(new string[] { Lecteur["Nom"].ToString() });
                            listView1.Items.Add(ItemListe);
                        }
                    }
                }
            }
            else if (Args.Node.Parent.Text == "Familles")
            {
                listView1.Columns.Clear();
                listView1.Items.Clear();

                listView1.Columns.Add("Description");

                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                foreach (ColumnHeader EnTeteColonne in listView1.Columns)
                {
                    EnTeteColonne.Width = listView1.Width / listView1.Columns.Count;
                }

                using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
                {
                    Connexion.Open();

                    string Requete = "SELECT Nom FROM SousFamilles WHERE RefFamille = (SELECT RefFamille FROM Familles WHERE Nom = '" + Args.Node.Text + "')";

                    using (SQLiteCommand Commande = new SQLiteCommand(Requete, Connexion))
                    {
                        SQLiteDataReader Lecteur = Commande.ExecuteReader();

                        while (Lecteur.Read())
                        {
                            ListViewItem ItemListe = new ListViewItem(new string[] { Lecteur["Nom"].ToString() });
                            listView1.Items.Add(ItemListe);
                        }
                    }
                }
            }
            else if (Args.Node.Parent.Parent != null && Args.Node.Parent.Parent.Text == "Familles")
            {
                listView1.Columns.Clear();
                listView1.Items.Clear();

                listView1.Columns.Add("RefArticle");
                listView1.Columns.Add("Description");
                listView1.Columns.Add("Familles");
                listView1.Columns.Add("Sous-familles");
                listView1.Columns.Add("Marques");
                listView1.Columns.Add("Prix");
                listView1.Columns.Add("Quantité");

                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                foreach (ColumnHeader EnTeteColonne in listView1.Columns)
                {
                    EnTeteColonne.Width = listView1.Width / listView1.Columns.Count;
                }

                using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
                {
                    Connexion.Open();

                    string Requete = "SELECT RefArticle, Description, Familles.Nom AS NomFamille, SousFamilles.Nom AS NomSousFamille, Marques.Nom AS NomMarque, PrixHT, Quantite FROM Articles, SousFamilles, Familles, Marques"
                                                    + " WHERE Articles.RefSousFamille = SousFamilles.RefSousFamille"
                                                    + " AND SousFamilles.RefFamille = Familles.RefFamille"
                                                    + " AND Articles.RefMarque = Marques.RefMarque"
                                                    + " AND SousFamilles.RefFamille = (SELECT RefFamille FROM Familles WHERE Nom = '" + Args.Node.Parent.Text + "')"
                                                    + " AND Articles.RefSousFamille = (SELECT RefSousFamille FROM SousFamilles WHERE Nom = '" + Args.Node.Text + "')";

                    using (SQLiteCommand Commande = new SQLiteCommand(Requete, Connexion))
                    {
                        SQLiteDataReader Lecteur = Commande.ExecuteReader();

                        while (Lecteur.Read())
                        {
                            ListViewItem ItemListe = new ListViewItem(new string[] { Lecteur["RefArticle"].ToString(), Lecteur["Description"].ToString(), Lecteur["NomFamille"].ToString(), Lecteur["NomSousFamille"].ToString(), Lecteur["NomMarque"].ToString(), Lecteur["PrixHT"].ToString(), Lecteur["Quantite"].ToString() });
                            listView1.Items.Add(ItemListe);
                        }
                    }
                }
            }
            else if (Args.Node.Parent.Text == "Marques")
            {
                listView1.Columns.Clear();
                listView1.Items.Clear();

                listView1.Columns.Add("RefArticle");
                listView1.Columns.Add("Description");
                listView1.Columns.Add("Familles");
                listView1.Columns.Add("Sous-familles");
                listView1.Columns.Add("Marques");
                listView1.Columns.Add("Prix");
                listView1.Columns.Add("Quantité");

                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                foreach (ColumnHeader EnTeteColonne in listView1.Columns)
                {
                    EnTeteColonne.Width = listView1.Width / listView1.Columns.Count;
                }

                using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
                {
                    Connexion.Open();

                    string Requete = "SELECT RefArticle, Description, Familles.Nom AS NomFamille, SousFamilles.Nom AS NomSousFamille, Marques.Nom AS NomMarque, PrixHT, Quantite FROM Articles, SousFamilles, Familles, Marques"
                                                    + " WHERE Articles.RefSousFamille = SousFamilles.RefSousFamille"
                                                    + " AND SousFamilles.RefFamille = Familles.RefFamille"
                                                    + " AND Articles.RefMarque = Marques.RefMarque"
                                                    + " AND Articles.RefMarque = (SELECT RefMarque FROM Marques WHERE Nom = '" + Args.Node.Text + "')";

                    using (SQLiteCommand Commande = new SQLiteCommand(Requete, Connexion))
                    {
                        SQLiteDataReader Lecteur = Commande.ExecuteReader();

                        while (Lecteur.Read())
                        {
                            ListViewItem ItemListe = new ListViewItem(new string[] { Lecteur["RefArticle"].ToString(), Lecteur["Description"].ToString(), Lecteur["NomFamille"].ToString(), Lecteur["NomSousFamille"].ToString(), Lecteur["NomMarque"].ToString(), Lecteur["PrixHT"].ToString(), Lecteur["Quantite"].ToString() });
                            listView1.Items.Add(ItemListe);
                        }
                    }
                }
            }
            else
            {
                listView1.Columns.Clear();
                listView1.Items.Clear();
            }
        }

        private void listView1_ColumnClick(object Sender, ColumnClickEventArgs Args)
        {
            
        }
    }
}
