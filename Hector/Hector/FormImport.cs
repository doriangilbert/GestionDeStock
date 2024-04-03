using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Hector
{
    /// <summary>
    /// Classe de la fenêtre modale permettant d'importer des données de fichiers "csv" dans notre base de données.
    /// </summary>
    public partial class FormImport : Form
    {
        private string _CheminFichier;

        /// <summary>
        /// Pour lancer la fenêtre (constructeur par défaut)
        /// </summary>
        public FormImport()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Permet d'ouvrir une fenêtre pour choisir un fichier csv. 
        /// Une fois fait, on prélève le chemin ainsi que le nom du fichier sélectionné.
        /// </summary>
        /// <param name="Sender">Objet <b>Object</b> prend en charge les éventuels objets que l'on renseigne en paramètre.</param>
        /// <param name="Args">Objet <b>EventArgs</b> contient les informations sur l'évènement.</param>
        private void ChoisirFichier_Click(object Sender, EventArgs Args)
        {
            // On instancie un objet de type OpenFileDialog avec comme filtre des fichiers de type "csv".
            OpenFileDialog OpenFileDialog = new OpenFileDialog
            {
                Filter = "csv files (*.csv)|*.csv"
            };

            // Si on choisi un fichier
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                // On obtient le chemin de ce fichier
                string FilePath = OpenFileDialog.FileName;
                // On l'enregistre dans une variable
                _CheminFichier = FilePath;
                // Puis on ne garde que le nom de ce fichier pour le mettre dans la zone de texte dédiée.
                FichierChoisi.Text = Path.GetFileName(FilePath);

                // On modifie le label pour indiquer qu'un fichier a bien été choisi et qu'il est prêt à être lu
                this.label1.Text = "Prêt pour l'importation...";
            }
        }


        /// <summary>
        /// Cherche parmis une liste de couples famille/sous-famille si le couple entré en paramètre est déjà dedans ou non.
        /// </summary>
        /// <param name="Sous_Familles">Objet <b>List<(string, string)></b> est la liste de couples dans laquelle nous recherchons un couple.</param>
        /// <param name="Famille">Objet <b>string</b> est la première moitié du couple que l'on recherche.</param>
        /// <param name="SousFamille">Objet <b>string</b> est la deuxième moitié du couple que l'on recherche.</param>
        /// <returns></returns>
        public bool TrouverSousFamille(List<(string, string)> Sous_Familles, string Famille, string SousFamille)
        {
            // On parcourt la liste
            for (int IndiceSousFamille = 0; IndiceSousFamille < Sous_Familles.Count; IndiceSousFamille++)
            {
                // On regarde si à la case IndisceSousFamille de la liste, les 2 parties du couples coincident.
                if (Sous_Familles[IndiceSousFamille].Item1 == Famille && Sous_Familles[IndiceSousFamille].Item2 == SousFamille)
                {
                    return true;
                }
            }
            // Si le couple n'appartient pas à la liste.
            return false;
        }

        /// <summary>
        /// On se connecte à la base de données puis on la vide.
        /// Ensuite, on parse le fichier csv choisi au préalable pour ajouter les objets dans la base de données vidée.
        /// </summary>
        /// <param name="Sender">Objet <b>Object</b> prend en charge les éventuels objets que l'on renseigne en paramètre.</param>
        /// <param name="Args">Objet <b>EventArgs</b> contient les informations sur l'évènement.</param>
        private void EcraserDonnees_Click(object Sender, EventArgs Args)
        {
            this.BarreDeProgression.Value = 0;

            // On instancie la variable contenant le nombre d'erreurs de l'importation
            int NombreErreurs;

            // On regarde si un fichier a été choisi au préalable.
            if (!string.IsNullOrEmpty(_CheminFichier))
            {
                // On trouve le nombre total de lignes du fichier.
                int NombreLignes = File.ReadLines(_CheminFichier).Count();

                // On se connecte à la BDD.
                using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
                {
                    Connexion.Open();

                    // On modifie le label pour indiquer que l'on supprime les données de la BDD
                    this.label1.Text = "Suppression des données";

                    // On prend toutes les tables du fichier.
                    string Requete = "SELECT name FROM sqlite_master WHERE type='table';";
                    string RequeteDeleteTable = "";
                    using (SQLiteCommand Commande = new SQLiteCommand(Requete, Connexion))
                    {
                        using (SQLiteDataReader Lecteur = Commande.ExecuteReader())
                        {
                            while (Lecteur.Read())
                            {
                                // Puis on efface les données contenues dans toutes les tables.
                                string NomDeTable = Lecteur.GetString(0);

                                RequeteDeleteTable = "DELETE FROM " + NomDeTable;

                                using (SQLiteCommand CommandeDelete = new SQLiteCommand(RequeteDeleteTable, Connexion))
                                {
                                    CommandeDelete.ExecuteNonQuery();
                                }
                            }
                        }
                    }


                    // On modifie le label pour indiquer que l'on parse le fichier
                    this.label1.Text = "Lecture du fichier en cours...";

                    // On parse les données et on les insère dans un tableau.
                    // On enregistre aussi le nombre d'erreurs total lors du parsage du fichier
                    List<string[]> Tableau;
                    (Tableau, NombreErreurs) = ParserDonnees(NombreLignes);

                    // On fait avancer la barre de progression en fonction du nombre d'erreurs
                    this.BarreDeProgression.Value = 50 + (int)((NombreErreurs / (float)NombreLignes) * 50);

                    // On cherche à savoir si un élément possédant la même ref existe déjà
                    string RequeteRechercheRef = "";
                    string RequeteAjoutArticle = "";

                    // On parcourt le tableau des données
                    for (int IndiceArticle = 1; IndiceArticle < Tableau.Count; IndiceArticle++)
                    {
                        // On modifie le label pour indiquer que l'on ajoute les articles dans la BDD
                        this.label1.Text = "Ajout des articles en cours...";

                        AjouterCategories(Connexion, Tableau, IndiceArticle);

                        // On recherche si la ref existe déjà
                        RequeteRechercheRef = "SELECT * FROM Articles WHERE RefArticle = '" + Tableau[IndiceArticle][1] + "'";

                        using (SQLiteCommand Commande = new SQLiteCommand(RequeteRechercheRef, Connexion))
                        {
                            using (SQLiteDataReader Lecteur = Commande.ExecuteReader())
                            {
                                // Si la ref n'existe pas, on rajoute l'article dans la BDD
                                if (!Lecteur.HasRows)
                                {
                                    RequeteAjoutArticle = "INSERT INTO Articles(RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) " +
                                        "VALUES('" + Tableau[IndiceArticle][1] + "', '" + Tableau[IndiceArticle][0] + "', " +
                                        "(SELECT RefSousFamille FROM SousFamilles WHERE Nom = '" + Tableau[IndiceArticle][4] + "'), " +
                                        "(SELECT RefMarque FROM Marques WHERE Nom = '" + Tableau[IndiceArticle][2] + "'), @Valeur5, 0)";

                                    using (SQLiteCommand CommandeArticles = new SQLiteCommand(RequeteAjoutArticle, Connexion))
                                    {
                                        // Pour que la Query accepte de modifier le string en entier / nombre
                                        CommandeArticles.Parameters.AddWithValue("@Valeur5", Tableau[IndiceArticle][5]);

                                        CommandeArticles.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        // On fait avancer la barre de progression
                        this.BarreDeProgression.Value = 50 + (int)(((NombreErreurs + IndiceArticle + 1) / (float)NombreLignes) * 50);
                    }

                    // On supprime le tableau alloué dynamiquement
                    Tableau.Clear();
                    Tableau = null;

                    // On modifie le label pour indiquer que l'on a fini l'ajout
                    this.label1.Text = "Terminé";
                }
            }
        }


        /// <summary>
        /// On se connecte à la base de données puis on parse le fichier csv choisi au préalable,
        /// on ajoute ou modifie les objets dans la base de données vide ou déjà remplie.
        /// </summary>
        /// <param name="Sender">Objet <b>Object</b> prend en charge les éventuels objets que l'on renseigne en paramètre.</param>
        /// <param name="Args">Objet <b>EventArgs</b> contient les informations sur l'évènement.</param>
        private void AjoutDonnees_Click(object Sender, EventArgs Args)
        {
            this.BarreDeProgression.Value = 0;
            // On instancie la variable contenant le nombre d'erreurs de l'importation
            int NombreErreurs;

            // On regarde si un fichier a été choisi au préalable.
            if (!string.IsNullOrEmpty(_CheminFichier))
            {
                // On trouve le nombre total de lignes du fichier.
                int NombreLignes = File.ReadLines(_CheminFichier).Count();

                // On se connecte à la BDD.
                using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
                {
                    Connexion.Open();

                    // On modifie le label pour indiquer que l'on parse le fichier
                    this.label1.Text = "Lecture du fichier en cours...";

                    // On parse les données et on les insère dans un tableau.
                    // On enregistre aussi le nombre d'erreurs total lors du parsage du fichier
                    List<string[]> Tableau;
                    (Tableau, NombreErreurs) = ParserDonnees(NombreLignes);

                    // On fait avancer la barre de progression en fonction du nombre d'erreurs
                    this.BarreDeProgression.Value = 50 + (int)((NombreErreurs / (float)NombreLignes) * 50);

                    // On cherche à savoir si un élément possédant la même ref existe déjà
                    string RequeteRechercheRef = "";
                    string RequeteAjoutArticle = "";

                    // On parcourt le tableau des données
                    for (int IndiceArticle = 1; IndiceArticle < Tableau.Count; IndiceArticle++)
                    {
                        // On modifie le label pour indiquer que l'on ajoute les articles dans la BDD
                        this.label1.Text = "Ajout des articles en cours...";

                        AjouterCategories(Connexion, Tableau, IndiceArticle);

                        // On recherche si la ref existe déjà
                        RequeteRechercheRef = "SELECT * FROM Articles WHERE RefArticle = '" + Tableau[IndiceArticle][1] + "'";

                        using (SQLiteCommand Commande = new SQLiteCommand(RequeteRechercheRef, Connexion))
                        {
                            using (SQLiteDataReader Lecteur = Commande.ExecuteReader())
                            {
                                // Si la ref n'existe pas, on rajoute l'article dans la BDD
                                if (!Lecteur.HasRows)
                                {
                                    RequeteAjoutArticle = "UPDATE Articles SET Description = '" + Tableau[IndiceArticle][0] + "', " +
                                        "RefSousFamille = (SELECT RefSousFamille FROM SousFamilles WHERE Nom = '" + Tableau[IndiceArticle][4] + "'), " +
                                        "RefMarque = (SELECT RefMarque FROM Marques WHERE Nom = '" + Tableau[IndiceArticle][2] + "'), " +
                                        "PrixHT = @Valeur5, Quantite = 0 WHERE RefArticle = '" + Tableau[IndiceArticle][1] + "'";

                                    using (SQLiteCommand CommandeArticles = new SQLiteCommand(RequeteAjoutArticle, Connexion))
                                    {
                                        // Pour que la Query accepte de modifier le string en entier / nombre
                                        CommandeArticles.Parameters.AddWithValue("@Valeur5", Tableau[IndiceArticle][5]);

                                        CommandeArticles.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        // On fait avancer la barre de progression
                        this.BarreDeProgression.Value = 50 + (int)(((NombreErreurs + IndiceArticle + 1) / (float)NombreLignes) * 50);
                    }

                    // On supprime le tableau alloué dynamiquement
                    Tableau.Clear();
                    Tableau = null;

                    // On modifie le label pour indiquer que l'on a fini l'ajout
                    this.label1.Text = "Terminé";
                }
            }
        }


        /// <summary>
        /// S'occupe de parser le fichier CSV pour mettre les données dans un tableau "utilisable" par l'application.
        /// </summary>
        /// <returns>Un tableau des éléments du fichier à parser et le nombre d'erreurs lors du parsage</returns>
        private (List<string[]>, int) ParserDonnees(int NombreLignes)
        {
            // On initialise le nombre d'erreurs
            int NombreErreurs = 0;

            // On crée le tableau accueillant tous les éléments du fichier
            List<string[]> Tableau = new List<string[]>();

            // On lance la lecture du fichier
            using (StreamReader Sr = new StreamReader(_CheminFichier, Encoding.Default))
            {
                // On veut connaitre le nombre de tables de la BDD
                int NombreColonnes = 0;

                // On ne regarde que la première ligne pour l'instant
                if (!Sr.EndOfStream)
                {
                    string Ligne1 = Sr.ReadLine();

                    // On supprime les espaces en trop après les ";"
                    while (Ligne1.Contains("; "))
                    {
                        Ligne1 = Ligne1.Replace("; ", ";");
                    }
                    // On supprime les espaces en trop avant les ";"
                    while (Ligne1.Contains(" ;"))
                    {
                        Ligne1 = Ligne1.Replace(" ;", ";");
                    }

                    // On sépare les colonnes via un tableau en omettant les espaces/cases vides.
                    char[] Separateur = { ';' };
                    string[] Mots1 = Ligne1.Split(Separateur, StringSplitOptions.RemoveEmptyEntries);

                    // On garde le nombre de colonnes du tableau de côté
                    // (pour éviter les erreurs type "pas assez de données" ou "trop de données dans cette ligne")
                    NombreColonnes = Mots1.Length;

                    Tableau.Add(Mots1);
                }

                int NumeroLigne = 1;

                // On parcourt toutes les autres lignes du fichier
                while (!Sr.EndOfStream)
                {
                    string Ligne = Sr.ReadLine();
                    // On supprime les espaces en trop après les ";"
                    while (Ligne.Contains("; "))
                    {
                        Ligne = Ligne.Replace("; ", ";");
                    }
                    // On supprime les espaces en trop avant les ";"
                    while (Ligne.Contains(" ;"))
                    {
                        Ligne = Ligne.Replace(" ;", ";");
                    }

                    // On sépare les colonnes via un tableau en omettant les espaces / cases vides.
                    char[] Separateur = { ';' };
                    string[] Mots = Ligne.Split(Separateur, StringSplitOptions.RemoveEmptyEntries);

                    // On regarde si il y a bien le bon nombre d'informations / de colonnes.
                    if (Mots.Length == NombreColonnes)
                    {
                        Tableau.Add(Mots);
                    }

                    else
                    {
                        NombreErreurs++;
                    }

                    NumeroLigne++;

                    // On fait avancer la barre de progression
                    this.BarreDeProgression.Value = (int)((NumeroLigne / (float)NombreLignes) * 50);
                }

                Sr.Close();
                Sr.Dispose();

                return (Tableau, NombreErreurs);
            }
        }


        /// <summary>
        /// S'occupe d'ajouter les différentes marques, familles et sous familles
        /// </summary>
        /// <param name="Connexion">Objet <b>SQLiteConnection</b> permettant de garder la connexion à la base de données.</param>
        private void AjouterCategories(SQLiteConnection Connexion, List<string[]> Tableau, int IndiceArticle)
        {
            // On recherche si la marque existe déjà
            string RequeteRechercheMarque = "SELECT * FROM Marques WHERE nom = '" + Tableau[IndiceArticle][2] + "'";

            using (SQLiteCommand Commande = new SQLiteCommand(RequeteRechercheMarque, Connexion))
            {
                using (SQLiteDataReader Lecteur = Commande.ExecuteReader())
                {
                    // Si la marque n'existe pas, on la rajoute à la BDD
                    if (!Lecteur.HasRows)
                    {
                        string RequeteAjoutMarque = "INSERT INTO Marques (Nom) VALUES ('" + Tableau[IndiceArticle][2] + "')";

                        using (SQLiteCommand CommandeMarques = new SQLiteCommand(RequeteAjoutMarque, Connexion))
                        {
                            CommandeMarques.ExecuteNonQuery();
                        }
                    }
                }
            }

            // On recherche si la famille existe déjà
            string RequeteRechercheFamille = "SELECT * FROM Familles WHERE nom = '" + Tableau[IndiceArticle][3] + "'";

            using (SQLiteCommand Commande = new SQLiteCommand(RequeteRechercheFamille, Connexion))
            {
                using (SQLiteDataReader Lecteur = Commande.ExecuteReader())
                {
                    // Si la famille n'existe pas, on la rajoute à la BDD
                    if (!Lecteur.HasRows)
                    {
                        string RequeteAjoutFamille = "INSERT INTO Familles (Nom) VALUES ('" + Tableau[IndiceArticle][3] + "')";

                        using (SQLiteCommand CommandeFamilles = new SQLiteCommand(RequeteAjoutFamille, Connexion))
                        {
                            CommandeFamilles.ExecuteNonQuery();
                        }
                    }
                }
            }

            // On recherche si la sous famille existe déjà
            string RequeteRechercheSousFamille = "SELECT * FROM SousFamilles WHERE nom = '" + Tableau[IndiceArticle][4] + "' AND RefFamille = (SELECT RefFamille FROM Familles WHERE Nom = '" + Tableau[IndiceArticle][3] + "')";

            using (SQLiteCommand Commande = new SQLiteCommand(RequeteRechercheSousFamille, Connexion))
            {
                using (SQLiteDataReader Lecteur = Commande.ExecuteReader())
                {
                    // Si la famille n'existe pas, on la rajoute à la BDD
                    if (!Lecteur.HasRows)
                    {
                        string RequeteAjoutSousFamille = "INSERT INTO SousFamilles (RefFamille, Nom) VALUES ((SELECT RefFamille FROM Familles WHERE Nom = '" + Tableau[IndiceArticle][3] + "'), '" + Tableau[IndiceArticle][4] + "')";

                        using (SQLiteCommand CommandeSousFamilles = new SQLiteCommand(RequeteAjoutSousFamille, Connexion))
                        {
                            CommandeSousFamilles.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}
