using Hector.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            // Pour obtenir la taille de l'écran principal
            Screen Ecran = Screen.PrimaryScreen;
            int LongueurEcran = Ecran.Bounds.Width;
            int HauteurEcran = Ecran.Bounds.Height;

            // On modifie la position de la fenêtre pour qu'elle soit la même qu'avant la fermeture de la fenêtre.
            // On vérifie que la fenêtre ne sorte pas de l'écran
            if (Settings.Default.Position.X < 0 || Settings.Default.Position.X + Settings.Default.Taille.Width > LongueurEcran)
            {
                this.Left = 0;
                Console.WriteLine("coucou");
            }
            else
            {
                this.Left = Settings.Default.Position.X;
            }

            if (Settings.Default.Position.Y < 0 || Settings.Default.Position.Y + Settings.Default.Taille.Height > HauteurEcran)
            {
                this.Top = 0;
                Console.WriteLine("salut");
            }
            else
            {
                this.Top = Settings.Default.Position.Y;
            }

            // On modifie la taille de la fenêtre pour qu'elle soit la même qu'avant la fermeture de la fenêtre.
            this.Size = Settings.Default.Taille;

            // Suppression des noeuds de l'arbre
            foreach (TreeNode Noeud in TreeView1.Nodes)
            {
                Noeud.Nodes.Clear();
            }

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
                        TreeView1.BeginUpdate();
                        TreeView1.Nodes[1].Nodes.Add(LecteurFamilles["Nom"].ToString());
                        TreeView1.EndUpdate();
                        
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
                                TreeView1.BeginUpdate();
                                TreeView1.Nodes[1].Nodes[IndiceNoeudFamille].Nodes.Add(LecteurSousFamilles["Nom"].ToString());
                                TreeView1.EndUpdate();
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
                        TreeView1.BeginUpdate();
                        TreeView1.Nodes[2].Nodes.Add(LecteurMarques["Nom"].ToString());
                        TreeView1.EndUpdate();
                    }
                }

                // Sélection du premier noeud de l'arbre
                this.TreeView1_AfterSelect(Sender, new TreeViewEventArgs(TreeView1.Nodes[0]));
            }

            // Réinitialisation de la barre de statut
            statusStrip1.Items.Clear();

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

            // Ajout d'un séparateur dans la barre de statut
            statusStrip1.Items.Add(new ToolStripSeparator());

            // Création du label du nombre de familles dans la barre de statut
            ToolStripStatusLabel StatusLabelNbFamilles = new ToolStripStatusLabel();

            // Création de la connexion à la base de données
            using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
            {
                // Ouverture de la connexion
                Connexion.Open();

                // Création de la requête SQL permettant d'obtenir le nombre de familles
                string Requete = "SELECT COUNT(*) AS NbFamilles FROM Familles";

                // Création de la commande SQL
                using (SQLiteCommand Commande = new SQLiteCommand(Requete, Connexion))
                {
                    // Création du lecteur
                    SQLiteDataReader Lecteur = Commande.ExecuteReader();

                    // Lecture du resultat de la requête
                    while (Lecteur.Read())
                    {
                        // Ajout du nombre de familles dans le label
                        StatusLabelNbFamilles.Text = Lecteur["NbFamilles"].ToString() + " Familles";
                    }
                }
            }

            // Ajout du label dans la barre de statut
            statusStrip1.Items.Add(StatusLabelNbFamilles);

            // Ajout d'un séparateur dans la barre de statut
            statusStrip1.Items.Add(new ToolStripSeparator());

            // Création du label du nombre de sous-familles dans la barre de statut
            ToolStripStatusLabel StatusLabelNbSousFamilles = new ToolStripStatusLabel();

            // Création de la connexion à la base de données
            using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
            {
                // Ouverture de la connexion
                Connexion.Open();

                // Création de la requête SQL permettant d'obtenir le nombre de sous-familles
                string Requete = "SELECT COUNT(*) AS NbSousFamilles FROM SousFamilles";

                // Création de la commande SQL
                using (SQLiteCommand Commande = new SQLiteCommand(Requete, Connexion))
                {
                    // Création du lecteur
                    SQLiteDataReader Lecteur = Commande.ExecuteReader();

                    // Lecture du resultat de la requête
                    while (Lecteur.Read())
                    {
                        // Ajout du nombre de sous-familles dans le label
                        StatusLabelNbSousFamilles.Text = Lecteur["NbSousFamilles"].ToString() + " Sous-Familles";
                    }
                }
            }

            // Ajout du label dans la barre de statut
            statusStrip1.Items.Add(StatusLabelNbSousFamilles);

            // Ajout d'un séparateur dans la barre de statut
            statusStrip1.Items.Add(new ToolStripSeparator());

            // Création du label du nombre de marques dans la barre de statut
            ToolStripStatusLabel StatusLabelNbMarques = new ToolStripStatusLabel();

            // Création de la connexion à la base de données
            using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
            {
                // Ouverture de la connexion
                Connexion.Open();

                // Création de la requête SQL permettant d'obtenir le nombre de marques
                string Requete = "SELECT COUNT(*) AS NbMarques FROM Marques";

                // Création de la commande SQL
                using (SQLiteCommand Commande = new SQLiteCommand(Requete, Connexion))
                {
                    // Création du lecteur
                    SQLiteDataReader Lecteur = Commande.ExecuteReader();

                    // Lecture du resultat de la requête
                    while (Lecteur.Read())
                    {
                        // Ajout du nombre de marques dans le label
                        StatusLabelNbMarques.Text = Lecteur["NbMarques"].ToString() + " Marques";
                    }
                }
            }

            // Ajout du label dans la barre de statut
            statusStrip1.Items.Add(StatusLabelNbMarques);
        }


        /// <summary>
        /// Permet d'actualiser les données de la page.
        /// </summary>
        /// <param name="Sender">Objet <b>Object</b> prend en charge les éventuels objets que l'on renseigne en paramètre.</param>
        /// <param name="Args">Objet <b>EventArgs</b> contient les informations sur l'évènement.</param>
        private void ActualiserToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            // Rechargement de la page
            this.FormMain_Load(Sender, Args);
        }


        /// <summary>
        /// Permet d'ouvrir une fenêtre modale d'importation de fichiers et de données pour la base de données.
        /// </summary>
        /// <param name="Sender">Objet <b>Object</b> prend en charge les éventuels objets que l'on renseigne en paramètre.</param>
        /// <param name="Args">Objet <b>EventArgs</b> contient les informations sur l'évènement.</param>
        private void ImporterToolStripMenuItem_Click_1(object Sender, EventArgs Args)
        {
            // Création de la fenêtre modale d'importation
            FormImport FenetreImportation = new FormImport();
            // Ouverture de la fenêtre modale
            FenetreImportation.ShowDialog();
            // Rechargement de la page
            this.FormMain_Load(Sender, Args);
        }


        /// <summary>
        /// Permet d'ouvrir une fenêtre modale d'exportation de des données de la base de données dans un fichier.
        /// </summary>
        /// <param name="Sender">Objet <b>Object</b> prend en charge les éventuels objets que l'on renseigne en paramètre.</param>
        /// <param name="Args">Objet <b>EventArgs</b> contient les informations sur l'évènement.</param>
        private void ExporterToolStripMenuItem_Click(object Sender, EventArgs Args)
        {
            FormExport FenetreExportation = new FormExport();
            FenetreExportation.ShowDialog();
        }

        /// <summary>
        /// Permet de gérer l'évènement de sélection d'un noeud dans l'arbre.
        /// </summary>
        /// <param name="Sender">Objet <b>Object</b> prend en charge les éventuels objets que l'on renseigne en paramètre.</param>
        /// <param name="Args">Objet <b>EventArgs</b> contient les informations sur l'évènement.</param>
        private void TreeView1_AfterSelect(object Sender, TreeViewEventArgs Args)
        {
            // Réinitialisation du tri de la liste
            ListView1.ListViewItemSorter = new ListViewItemComparer();

            // Réinitialisation des groupes
            ListView1.Groups.Clear();

            // Si le noeud sélectionné est "Tous les articles"
            if (Args.Node.Text == "Tous les articles")
            {
                // Réinitialisation de la liste
                ListView1.Columns.Clear();
                ListView1.Items.Clear();

                // Ajout des colonnes
                ListView1.Columns.Add("RefArticle");
                ListView1.Columns.Add("Description");
                ListView1.Columns.Add("Familles");
                ListView1.Columns.Add("Sous-familles");
                ListView1.Columns.Add("Marques");
                ListView1.Columns.Add("Prix");
                ListView1.Columns.Add("Quantité");

                // Redimensionnement auutomatique des colonnes
                ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                // Redimensionnement des colonnes pour qu'elles prennent toute la largeur de la vue
                foreach(ColumnHeader EnTeteColonne in ListView1.Columns)
                {
                    EnTeteColonne.Width = ListView1.Width / ListView1.Columns.Count;
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
                            ListView1.Items.Add(ItemListe);
                        }
                    }
                }
            }
            // Si le noeud sélectionné est "Familles"
            else if (Args.Node.Text == "Familles")
            {
                // Réinitialisation de la liste
                ListView1.Columns.Clear();
                ListView1.Items.Clear();

                // Ajout de la colonne
                ListView1.Columns.Add("Description");

                // Redimensionnement automatique des colonnes
                ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                // Redimensionnement des colonnes pour qu'elles prennent toute la largeur de la vue
                foreach (ColumnHeader EnTeteColonne in ListView1.Columns)
                {
                    EnTeteColonne.Width = ListView1.Width / ListView1.Columns.Count;
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
                            ListView1.Items.Add(ItemListe);
                        }
                    }
                }
            }
            // Si le noeud sélectionné est "Marques"
            else if (Args.Node.Text == "Marques")
            {
                // Réinitialisation de la liste
                ListView1.Columns.Clear();
                ListView1.Items.Clear();

                // Ajout de la colonne
                ListView1.Columns.Add("Description");

                // Redimensionnement automatique des colonnes
                ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                // Redimensionnement des colonnes pour qu'elles prennent toute la largeur de la vue
                foreach (ColumnHeader EnTeteColonne in ListView1.Columns)
                {
                    EnTeteColonne.Width = ListView1.Width / ListView1.Columns.Count;
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
                            ListView1.Items.Add(ItemListe);
                        }
                    }
                }
            }
            // Si le noeud sélectionné correspond à une famille
            else if (Args.Node.Parent.Text == "Familles")
            {
                // Réinitialisation de la liste
                ListView1.Columns.Clear();
                ListView1.Items.Clear();

                // Ajout de la colonne
                ListView1.Columns.Add("Description");

                // Redimensionnement automatique des colonnes
                ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                // Redimensionnement des colonnes pour qu'elles prennent toute la largeur de la vue
                foreach (ColumnHeader EnTeteColonne in ListView1.Columns)
                {
                    EnTeteColonne.Width = ListView1.Width / ListView1.Columns.Count;
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
                            ListView1.Items.Add(ItemListe);
                        }
                    }
                }
            }
            // Si le noeud sélectionné correspond à une sous-famille
            else if (Args.Node.Parent.Parent != null && Args.Node.Parent.Parent.Text == "Familles")
            {
                // Réinitialisation de la liste
                ListView1.Columns.Clear();
                ListView1.Items.Clear();

                // Ajout des colonnes
                ListView1.Columns.Add("RefArticle");
                ListView1.Columns.Add("Description");
                ListView1.Columns.Add("Familles");
                ListView1.Columns.Add("Sous-familles");
                ListView1.Columns.Add("Marques");
                ListView1.Columns.Add("Prix");
                ListView1.Columns.Add("Quantité");

                // Redimensionnement automatique des colonnes
                ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                // Redimensionnement des colonnes pour qu'elles prennent toute la largeur de la vue
                foreach (ColumnHeader EnTeteColonne in ListView1.Columns)
                {
                    EnTeteColonne.Width = ListView1.Width / ListView1.Columns.Count;
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
                            ListView1.Items.Add(ItemListe);
                        }
                    }
                }
            }
            // Si le noeud sélectionné correspond à une marque
            else if (Args.Node.Parent.Text == "Marques")
            {
                // Réinitialisation de la liste
                ListView1.Columns.Clear();
                ListView1.Items.Clear();

                // Ajout des colonnes
                ListView1.Columns.Add("RefArticle");
                ListView1.Columns.Add("Description");
                ListView1.Columns.Add("Familles");
                ListView1.Columns.Add("Sous-familles");
                ListView1.Columns.Add("Marques");
                ListView1.Columns.Add("Prix");
                ListView1.Columns.Add("Quantité");

                // Redimensionnement automatique des colonnes
                ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                // Redimensionnement des colonnes pour qu'elles prennent toute la largeur de la vue
                foreach (ColumnHeader EnTeteColonne in ListView1.Columns)
                {
                    EnTeteColonne.Width = ListView1.Width / ListView1.Columns.Count;
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
                            ListView1.Items.Add(ItemListe);
                        }
                    }
                }
            }
            else
            {
                // Réinitialisation de la liste
                ListView1.Columns.Clear();
                ListView1.Items.Clear();
            }
        }

        /// <summary>
        /// Permet de gérer le clic sur une colonne de la liste
        /// </summary>
        /// <param name="Sender">Objet <b>Object</b> prend en charge les éventuels objets que l'on renseigne en paramètre.</param>
        /// <param name="Args">Objet <b>EventArgs</b> contient les informations sur l'évènement.</param>
        private void ListView1_ColumnClick(object Sender, ColumnClickEventArgs Args)
        {
            // Tri des éléments de la liste en fonction de la colonne sélectionnée
            ListView1.ListViewItemSorter = new ListViewItemComparer(Args.Column);

            // Récupération du nom de la colonne sélectionnée
            string NomColonne = ListView1.Columns[Args.Column].Text;

            // Réinitialisation des groupes
            ListView1.Groups.Clear();

            // Si la colonne sélectionnée correspond à la colonne "Familles", "Sous-familles" ou "Marques"
            if (NomColonne == "Familles" || NomColonne == "Sous-familles" || NomColonne == "Marques")
            {
                // Parcours des éléments de la liste
                foreach (ListViewItem Item in ListView1.Items)
                {
                    // Booléen permettant de savoir si le groupe existe
                    bool GroupeExiste = false;

                    // Pour chaque groupe de la liste
                    foreach (ListViewGroup Groupe in ListView1.Groups)
                    {
                        // Si le groupe existe
                        if (Item.SubItems[Args.Column].Text == Groupe.Header)
                        {
                            GroupeExiste = true;
                        }
                    }
                    // Si le groupe n'existe pas
                    if (!GroupeExiste)
                    {
                        // Ajout du groupe
                        ListView1.Groups.Add(new ListViewGroup(Item.SubItems[Args.Column].Text, HorizontalAlignment.Left));
                    }
                    // Pour chaque groupe de la liste
                    foreach (ListViewGroup Groupe in ListView1.Groups)
                    {
                        // Si le texte dans la colonne pour l'élément correspond au nom du groupe
                        if (Item.SubItems[Args.Column].Text == Groupe.Header)
                        {
                            // Ajout de l'élément dans le groupe
                            Item.Group = Groupe;
                        }
                    }
                }
            }
            // Si la colonne sélectionnée correspond à la colonne "Description"
            else if (NomColonne == "Description")
            {
                // Parcours des éléments de la liste
                foreach (ListViewItem Item in ListView1.Items)
                {
                    // Booléen permettant de savoir si le groupe existe
                    bool GroupeExiste = false;

                    // Pour chaque groupe de la liste
                    foreach (ListViewGroup Groupe in ListView1.Groups)
                    {
                        // Si le groupe correspondant à la première lettre de la description existe
                        if (Item.SubItems[Args.Column].Text.StartsWith(Groupe.Header))
                        {
                            GroupeExiste = true;
                        }
                    }
                    // Si le groupe n'existe pas
                    if (!GroupeExiste)
                    {
                        // Ajout du groupe nommé selon la première lettre de la description
                        ListView1.Groups.Add(new ListViewGroup(Item.SubItems[Args.Column].Text.Substring(0,1), HorizontalAlignment.Left));
                    }
                    // Pour chaque groupe de la liste
                    foreach (ListViewGroup Groupe in ListView1.Groups)
                    {
                        // Si le texte dans la colonne pour l'élément commence par le nom du groupe
                        if (Item.SubItems[Args.Column].Text.StartsWith(Groupe.Header))
                        {
                            // Ajout de l'élément dans le groupe
                            Item.Group = Groupe;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Permet de gérer l'appui sur une touche du clavier
        /// </summary>
        /// <param name="Sender">Objet <b>Object</b> prend en charge les éventuels objets que l'on renseigne en paramètre.</param>
        /// <param name="Args">Objet <b>EventArgs</b> contient les informations sur l'évènement.</param>
        private void ListView1_KeyDown(object Sender, KeyEventArgs Args)
        {
            // Si la touche F5 est appuyée
            if (Args.KeyCode == Keys.F5)
            {
                // Rechargement de la liste
                this.FormMain_Load(Sender, Args);
            }
            
        }


        private void FormMain_FormClosed(object Sender, FormClosedEventArgs Events)
        {
            // On enregistre la position de la fenêtre
            Settings.Default.Position = new Point(this.Left, this.Top);
            Settings.Default.Save();

            // On enregistre la taille de la fenêtre
            Settings.Default.Taille = this.Size;
            Settings.Default.Save();

            Console.WriteLine(Left + ", " + Top);
        }
    }
}
