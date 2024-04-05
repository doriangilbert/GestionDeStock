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
        /// Fonction appelée lors du chargement de la fenêtre principale.
        /// </summary>
        /// <param name="Sender">Objet <b>Object</b> prend en charge les éventuels objets que l'on renseigne en paramètre.</param>
        /// <param name="Args">Objet <b>EventArgs</b> contient les informations sur l'évènement.</param>
        private void FormMain_Load(object Sender, EventArgs Args)
        {
            // Création de la connexion à la base de données
            using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
            {
                // Ouverture de la connexion
                Connexion.Open();

                // Création de la requête SQL permettant d'obtenir la liste des familles
                string RequeteSelectFamilles = "SELECT * FROM Familles";

                // Création de la commande SQL
                using (SQLiteCommand CommandeFamilles = new SQLiteCommand(RequeteSelectFamilles, Connexion))
                {
                    // Création du lecteur
                    SQLiteDataReader LecteurFamilles = CommandeFamilles.ExecuteReader();

                    // Indice du noeud famille courant
                    int IndiceNoeudFamille = 0;

                    // Lecture du resultat de la requête
                    while (LecteurFamilles.Read())
                    {
                        // Ajout de la famille dans l'arbre
                        treeView1.BeginUpdate();
                        treeView1.Nodes[1].Nodes.Add(LecteurFamilles["Nom"].ToString());
                        treeView1.EndUpdate();
                        
                        // Création de la requête SQL permettant d'obtenir la liste des sous-familles
                        string RequeteSelectSousFamilles = "SELECT * FROM SousFamilles WHERE RefFamille = @RefFamille";

                        // Création de la commande SQL
                        using (SQLiteCommand CommandeSousFamilles = new SQLiteCommand(RequeteSelectSousFamilles, Connexion))
                        {
                            // Ajout de la reference de la famille en paramètre de la requête
                            CommandeSousFamilles.Parameters.Add(new SQLiteParameter("@RefFamille", LecteurFamilles["RefFamille"].ToString()));

                            // Création du lecteur
                            SQLiteDataReader LecteurSousFamilles = CommandeSousFamilles.ExecuteReader();

                            // Lecture du resultat de la requête
                            while (LecteurSousFamilles.Read())
                            {
                                // Ajout de la sous-famille dans l'arbre
                                treeView1.BeginUpdate();
                                treeView1.Nodes[1].Nodes[IndiceNoeudFamille].Nodes.Add(LecteurSousFamilles["Nom"].ToString());
                                treeView1.EndUpdate();
                            }
                        }

                        // Passage au noeud famille suivant
                        IndiceNoeudFamille++;
                    }
                }

                // Création de la requête SQL permettant d'obtenir la liste des marques
                string RequeteSelectMarques = "SELECT * FROM Marques";

                // Création de la commande SQL
                using (SQLiteCommand CommandeMarques = new SQLiteCommand(RequeteSelectMarques, Connexion))
                {
                    // Création du lecteur
                    SQLiteDataReader LecteurMarques = CommandeMarques.ExecuteReader();

                    // Lecture du resultat de la requête
                    while (LecteurMarques.Read())
                    {
                        // Ajout de la marque dans l'arbre
                        treeView1.BeginUpdate();
                        treeView1.Nodes[2].Nodes.Add(LecteurMarques["Nom"].ToString());
                        treeView1.EndUpdate();
                    }
                }
            }

            // Création du label du nombre d'articles dans la barre de statut
            ToolStripStatusLabel StatusLabelNbArticles = new ToolStripStatusLabel();

            // Création de la connexion à la base de données
            using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
            {
                // Ouverture de la connexion
                Connexion.Open();

                // Création de la requête SQL permettant d'obtenir le nombre d'articles
                string Requete = "SELECT COUNT(*) AS NbArticles FROM Articles";

                // Création de la commande SQL
                using (SQLiteCommand Commande = new SQLiteCommand(Requete, Connexion))
                {
                    // Création du lecteur
                    SQLiteDataReader Lecteur = Commande.ExecuteReader();

                    // Lecture du resultat de la requête
                    while (Lecteur.Read())
                    {
                        // Ajout du nombre d'articles dans le label
                        StatusLabelNbArticles.Text = Lecteur["NbArticles"].ToString() + " Articles";
                    }
                }
            }

            // Ajout du label dans la barre de statut
            statusStrip1.Items.Add(StatusLabelNbArticles);
        }


        /// <summary>
        /// Permet d'actualiser les données de la page.
        /// </summary>
        /// <param name="Sender">Objet <b>Object</b> prend en charge les éventuels objets que l'on renseigne en paramètre.</param>
        /// <param name="Args">Objet <b>EventArgs</b> contient les informations sur l'évènement.</param>
        private void ActualiserToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            this.FormMain_Load(Sender, Args);
            //this.Refresh();
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

        /// <summary>
        /// Permet de gérer l'évènement de sélection d'un noeud dans l'arbre.
        /// </summary>
        /// <param name="Sender">Objet <b>Object</b> prend en charge les éventuels objets que l'on renseigne en paramètre.</param>
        /// <param name="Args">Objet <b>EventArgs</b> contient les informations sur l'évènement.</param>
        private void treeView1_AfterSelect(object Sender, TreeViewEventArgs Args)
        {
            // Si le noeud sélectionné est "Tous les articles"
            if(Args.Node.Text == "Tous les articles")
            {
                // Réinitialisation de la liste
                listView1.Columns.Clear();
                listView1.Items.Clear();

                // Ajout des colonnes
                listView1.Columns.Add("RefArticle");
                listView1.Columns.Add("Description");
                listView1.Columns.Add("Familles");
                listView1.Columns.Add("Sous-familles");
                listView1.Columns.Add("Marques");
                listView1.Columns.Add("Prix");
                listView1.Columns.Add("Quantité");

                // Redimensionnement auutomatique des colonnes
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                // Redimensionnement des colonnes pour qu'elles prennent toute la largeur de la vue
                foreach(ColumnHeader EnTeteColonne in listView1.Columns)
                {
                    EnTeteColonne.Width = listView1.Width / listView1.Columns.Count;
                }

                // Création de la connexion à la base de données
                using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
                {
                    // Ouverture de la connexion
                    Connexion.Open();

                    // Création de la requête SQL permettant d'obtenir tous les articles
                    string Requete = "SELECT RefArticle, Description, Familles.Nom AS NomFamille, SousFamilles.Nom AS NomSousFamille, Marques.Nom AS NomMarque, PrixHT, Quantite FROM Articles, SousFamilles, Familles, Marques"
                                                    + " WHERE Articles.RefSousFamille = SousFamilles.RefSousFamille"
                                                    + " AND SousFamilles.RefFamille = Familles.RefFamille"
                                                    + " AND Articles.RefMarque = Marques.RefMarque";

                    // Création de la commande SQL
                    using (SQLiteCommand Commande = new SQLiteCommand(Requete, Connexion))
                    {
                        // Création du lecteur
                        SQLiteDataReader Lecteur = Commande.ExecuteReader();

                        // Lecture du resultat de la requête
                        while (Lecteur.Read())
                        {
                            // Ajout de l'article dans la liste
                            ListViewItem ItemListe = new ListViewItem(new string[] { Lecteur["RefArticle"].ToString(), Lecteur["Description"].ToString(), Lecteur["NomFamille"].ToString(), Lecteur["NomSousFamille"].ToString(), Lecteur["NomMarque"].ToString(), Lecteur["PrixHT"].ToString(), Lecteur["Quantite"].ToString() });
                            listView1.Items.Add(ItemListe);
                        }
                    }
                }
            }
            // Si le noeud sélectionné est "Familles"
            else if (Args.Node.Text == "Familles")
            {
                // Réinitialisation de la liste
                listView1.Columns.Clear();
                listView1.Items.Clear();

                // Ajout de la colonne
                listView1.Columns.Add("Description");

                // Redimensionnement automatique des colonnes
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                // Redimensionnement des colonnes pour qu'elles prennent toute la largeur de la vue
                foreach (ColumnHeader EnTeteColonne in listView1.Columns)
                {
                    EnTeteColonne.Width = listView1.Width / listView1.Columns.Count;
                }

                // Création de la connexion à la base de données
                using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
                {
                    // Ouverture de la connexion
                    Connexion.Open();

                    // Création de la requête SQL permettant d'obtenir les familles
                    string Requete = "SELECT Nom FROM Familles";

                    // Création de la commande SQL
                    using (SQLiteCommand Commande = new SQLiteCommand(Requete, Connexion))
                    {
                        // Création du lecteur
                        SQLiteDataReader Lecteur = Commande.ExecuteReader();

                        // Lecture du resultat de la requête
                        while (Lecteur.Read())
                        {
                            // Ajout de la famille dans la liste
                            ListViewItem ItemListe = new ListViewItem(new string[] { Lecteur["Nom"].ToString() });
                            listView1.Items.Add(ItemListe);
                        }
                    }
                }
            }
            // Si le noeud sélectionné est "Marques"
            else if (Args.Node.Text == "Marques")
            {
                // Réinitialisation de la liste
                listView1.Columns.Clear();
                listView1.Items.Clear();

                // Ajout de la colonne
                listView1.Columns.Add("Description");

                // Redimensionnement automatique des colonnes
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                // Redimensionnement des colonnes pour qu'elles prennent toute la largeur de la vue
                foreach (ColumnHeader EnTeteColonne in listView1.Columns)
                {
                    EnTeteColonne.Width = listView1.Width / listView1.Columns.Count;
                }

                // Création de la connexion à la base de données
                using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
                {
                    // Ouverture de la connexion
                    Connexion.Open();

                    // Création de la requête SQL permettant d'obtenir les marques
                    string Requete = "SELECT Nom FROM Marques";

                    // Création de la commande SQL
                    using (SQLiteCommand Commande = new SQLiteCommand(Requete, Connexion))
                    {
                        // Création du lecteur
                        SQLiteDataReader Lecteur = Commande.ExecuteReader();

                        // Lecture du resultat de la requête
                        while (Lecteur.Read())
                        {
                            // Ajout de la marque dans la liste
                            ListViewItem ItemListe = new ListViewItem(new string[] { Lecteur["Nom"].ToString() });
                            listView1.Items.Add(ItemListe);
                        }
                    }
                }
            }
            // Si le noeud sélectionné correspond à une famille
            else if (Args.Node.Parent.Text == "Familles")
            {
                // Réinitialisation de la liste
                listView1.Columns.Clear();
                listView1.Items.Clear();

                // Ajout de la colonne
                listView1.Columns.Add("Description");

                // Redimensionnement automatique des colonnes
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                // Redimensionnement des colonnes pour qu'elles prennent toute la largeur de la vue
                foreach (ColumnHeader EnTeteColonne in listView1.Columns)
                {
                    EnTeteColonne.Width = listView1.Width / listView1.Columns.Count;
                }

                // Création de la connexion à la base de données
                using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
                {
                    // Ouverture de la connexion
                    Connexion.Open();

                    // Création de la requête SQL permettant d'obtenir les sous-familles correspondantes à la famille sélectionnée
                    string Requete = "SELECT Nom FROM SousFamilles WHERE RefFamille = (SELECT RefFamille FROM Familles WHERE Nom = '" + Args.Node.Text + "')";

                    // Création de la commande SQL
                    using (SQLiteCommand Commande = new SQLiteCommand(Requete, Connexion))
                    {
                        // Création du lecteur
                        SQLiteDataReader Lecteur = Commande.ExecuteReader();

                        // Lecture du resultat de la requête
                        while (Lecteur.Read())
                        {
                            // Ajout de la sous-famille dans la liste
                            ListViewItem ItemListe = new ListViewItem(new string[] { Lecteur["Nom"].ToString() });
                            listView1.Items.Add(ItemListe);
                        }
                    }
                }
            }
            // Si le noeud sélectionné correspond à une sous-famille
            else if (Args.Node.Parent.Parent != null && Args.Node.Parent.Parent.Text == "Familles")
            {
                // Réinitialisation de la liste
                listView1.Columns.Clear();
                listView1.Items.Clear();

                // Ajout des colonnes
                listView1.Columns.Add("RefArticle");
                listView1.Columns.Add("Description");
                listView1.Columns.Add("Familles");
                listView1.Columns.Add("Sous-familles");
                listView1.Columns.Add("Marques");
                listView1.Columns.Add("Prix");
                listView1.Columns.Add("Quantité");

                // Redimensionnement automatique des colonnes
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                // Redimensionnement des colonnes pour qu'elles prennent toute la largeur de la vue
                foreach (ColumnHeader EnTeteColonne in listView1.Columns)
                {
                    EnTeteColonne.Width = listView1.Width / listView1.Columns.Count;
                }

                // Création de la connexion à la base de données
                using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
                {
                    // Ouverture de la connexion
                    Connexion.Open();

                    // Création de la requête SQL permettant d'obtenir les articles de la sous-famille sélectionnée
                    string Requete = "SELECT RefArticle, Description, Familles.Nom AS NomFamille, SousFamilles.Nom AS NomSousFamille, Marques.Nom AS NomMarque, PrixHT, Quantite FROM Articles, SousFamilles, Familles, Marques"
                                                    + " WHERE Articles.RefSousFamille = SousFamilles.RefSousFamille"
                                                    + " AND SousFamilles.RefFamille = Familles.RefFamille"
                                                    + " AND Articles.RefMarque = Marques.RefMarque"
                                                    + " AND SousFamilles.RefFamille = (SELECT RefFamille FROM Familles WHERE Nom = '" + Args.Node.Parent.Text + "')"
                                                    + " AND Articles.RefSousFamille = (SELECT RefSousFamille FROM SousFamilles WHERE Nom = '" + Args.Node.Text + "')";

                    // Création de la commande SQL
                    using (SQLiteCommand Commande = new SQLiteCommand(Requete, Connexion))
                    {
                        // Création du lecteur
                        SQLiteDataReader Lecteur = Commande.ExecuteReader();

                        // Lecture du resultat de la requête
                        while (Lecteur.Read())
                        {
                            // Ajout de l'article dans la liste
                            ListViewItem ItemListe = new ListViewItem(new string[] { Lecteur["RefArticle"].ToString(), Lecteur["Description"].ToString(), Lecteur["NomFamille"].ToString(), Lecteur["NomSousFamille"].ToString(), Lecteur["NomMarque"].ToString(), Lecteur["PrixHT"].ToString(), Lecteur["Quantite"].ToString() });
                            listView1.Items.Add(ItemListe);
                        }
                    }
                }
            }
            // Si le noeud sélectionné correspond à une marque
            else if (Args.Node.Parent.Text == "Marques")
            {
                // Réinitialisation de la liste
                listView1.Columns.Clear();
                listView1.Items.Clear();

                // Ajout des colonnes
                listView1.Columns.Add("RefArticle");
                listView1.Columns.Add("Description");
                listView1.Columns.Add("Familles");
                listView1.Columns.Add("Sous-familles");
                listView1.Columns.Add("Marques");
                listView1.Columns.Add("Prix");
                listView1.Columns.Add("Quantité");

                // Redimensionnement automatique des colonnes
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                // Redimensionnement des colonnes pour qu'elles prennent toute la largeur de la vue
                foreach (ColumnHeader EnTeteColonne in listView1.Columns)
                {
                    EnTeteColonne.Width = listView1.Width / listView1.Columns.Count;
                }

                // Création de la connexion à la base de données
                using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
                {
                    // Ouverture de la connexion
                    Connexion.Open();

                    // Création de la requête SQL permettant d'obtenir les articles de la marque sélectionnée
                    string Requete = "SELECT RefArticle, Description, Familles.Nom AS NomFamille, SousFamilles.Nom AS NomSousFamille, Marques.Nom AS NomMarque, PrixHT, Quantite FROM Articles, SousFamilles, Familles, Marques"
                                                    + " WHERE Articles.RefSousFamille = SousFamilles.RefSousFamille"
                                                    + " AND SousFamilles.RefFamille = Familles.RefFamille"
                                                    + " AND Articles.RefMarque = Marques.RefMarque"
                                                    + " AND Articles.RefMarque = (SELECT RefMarque FROM Marques WHERE Nom = '" + Args.Node.Text + "')";

                    // Création de la commande SQL
                    using (SQLiteCommand Commande = new SQLiteCommand(Requete, Connexion))
                    {
                        // Création du lecteur
                        SQLiteDataReader Lecteur = Commande.ExecuteReader();

                        // Lecture du resultat de la requête
                        while (Lecteur.Read())
                        {
                            // Ajout de l'article dans la liste
                            ListViewItem ItemListe = new ListViewItem(new string[] { Lecteur["RefArticle"].ToString(), Lecteur["Description"].ToString(), Lecteur["NomFamille"].ToString(), Lecteur["NomSousFamille"].ToString(), Lecteur["NomMarque"].ToString(), Lecteur["PrixHT"].ToString(), Lecteur["Quantite"].ToString() });
                            listView1.Items.Add(ItemListe);
                        }
                    }
                }
            }
            else
            {
                // Réinitialisation de la liste
                listView1.Columns.Clear();
                listView1.Items.Clear();
            }
        }

        /// <summary>
        /// Permet de gérer le clic sur une colonne de la liste
        /// </summary>
        /// <param name="Sender">Objet <b>Object</b> prend en charge les éventuels objets que l'on renseigne en paramètre.</param>
        /// <param name="Args">Objet <b>EventArgs</b> contient les informations sur l'évènement.</param>
        private void listView1_ColumnClick(object Sender, ColumnClickEventArgs Args)
        {
            
        }
    }
}
