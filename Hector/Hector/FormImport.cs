using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
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
            // On instancie la variable contenant le nombre d'erreurs de l'importation
            int NombreErreurs = 0;
            // On regarde si un fichier a été choisi au préalable.
            if (!string.IsNullOrEmpty(_CheminFichier))
            {
                // On trouve le nombre total de lignes du fichier.
                int NombreLignes = File.ReadLines(_CheminFichier).Count();

                // On se connecte à la BDD.
                using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
                {
                    Connexion.Open();

                    // On prend toutes les tables du fichier.
                    string query = "SELECT name FROM sqlite_master WHERE type='table';";
                    string RequeteDeleteTable = "";
                    using (SQLiteCommand command = new SQLiteCommand(query, Connexion))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Puis on efface les données contenues dans toutes les tables.
                                string tableName = reader.GetString(0);

                                RequeteDeleteTable = "DELETE FROM " + tableName;

                                using (SQLiteCommand CommandeDelete = new SQLiteCommand(RequeteDeleteTable, Connexion))
                                {
                                    CommandeDelete.ExecuteNonQuery();
                                }
                            }
                        }
                    }


                    // On crée le tableau accueillant tous les éléments du fichier
                    List<string[]> Tableau = new List<string[]>();

                    // On lance la lecture du fichier
                    using (StreamReader Sr = new StreamReader(_CheminFichier, Encoding.Default))
                    {
                        // On s'occupe de connaitre le nombre de tables de la BDD
                        int NombreColonnes = 0;

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

                        // On parcourt toutes les lignes du fichier
                        while (!Sr.EndOfStream)
                        {
                            string Ligne = Sr.ReadLine();
                            Console.WriteLine(Ligne);
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
                        }

                        Sr.Close();
                        Sr.Dispose();
                    }

                    List<string> Marques = new List<string>();
                    List<string> Familles = new List<string>();
                    List<(string, string)> Sous_Familles = new List<(string, string)>();

                    // On parcourt le tableau venant d'être rempli
                    for (int IndiceLigne = 1; IndiceLigne < Tableau.Count; IndiceLigne++)
                    {
                        // Pour trouver toutes les marques
                        if (Marques.Contains(Tableau[IndiceLigne][2]) == false)
                        {
                            Marques.Add(Tableau[IndiceLigne][2]);
                        }

                        // Pour trouver toutes les familles
                        if (Familles.Contains(Tableau[IndiceLigne][3]) == false)
                        {
                            Familles.Add(Tableau[IndiceLigne][3]);
                        }

                        // Pour trouver les sous-familles
                        // Nous faisons face à un couple de données, on implémente donc une nouvelle fonction.
                        if (TrouverSousFamille(Sous_Familles, Tableau[IndiceLigne][3], Tableau[IndiceLigne][4]) == false)
                        {
                            Sous_Familles.Add((Tableau[IndiceLigne][3], Tableau[IndiceLigne][4]));
                        }
                    }


                    string RequeteAjoutMarque = "";
                    // On ajoute les éléments de la table Marques
                    for (int IndiceMarque = 0; IndiceMarque < Marques.Count; IndiceMarque++)
                    {
                        RequeteAjoutMarque = "INSERT INTO Marques (Nom) VALUES ('" + Marques[IndiceMarque] + "')";

                        using (SQLiteCommand CommandeMarques = new SQLiteCommand(RequeteAjoutMarque, Connexion))
                        {
                            CommandeMarques.ExecuteNonQuery();
                        }
                    }

                    string RequeteAjoutFamille = "";
                    // On ajoute les éléments de la table Familles
                    for (int IndiceFamille = 0; IndiceFamille < Familles.Count; IndiceFamille++)
                    {
                        RequeteAjoutFamille = "INSERT INTO Familles (Nom) VALUES ('" + Familles[IndiceFamille] + "')";

                        using (SQLiteCommand CommandeFamilles = new SQLiteCommand(RequeteAjoutFamille, Connexion))
                        {
                            CommandeFamilles.ExecuteNonQuery();
                        }
                    }

                    string RequeteAjoutSousFamille = "";
                    // On ajoute les éléments de la table Sous Familles avec la référence de la Famille
                    for (int IndiceSousFamille = 0; IndiceSousFamille < Sous_Familles.Count; IndiceSousFamille++)
                    {
                        RequeteAjoutSousFamille = "INSERT INTO SousFamilles (RefFamille, Nom) VALUES ((SELECT RefFamille FROM Familles WHERE Nom = '" + Sous_Familles[IndiceSousFamille].Item1 + "'), '" + Sous_Familles[IndiceSousFamille].Item2 + "')";

                        using (SQLiteCommand CommandeSousFamilles = new SQLiteCommand(RequeteAjoutSousFamille, Connexion))
                        {
                            CommandeSousFamilles.ExecuteNonQuery();
                        }
                    }

                    string RequeteAjoutArticle = "";
                    // Maintenant, on veut rajouter tous les articles
                    for (int IndiceArticle = 0; IndiceArticle < Tableau.Count; IndiceArticle++)
                    {
                        RequeteAjoutArticle = "INSERT OR IGNORE INTO Articles(RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) " +
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

                    // On supprime le tableau alloué dynamiquement
                    Tableau.Clear();
                    Tableau = null;
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
            int NombreLignes = File.ReadLines(_CheminFichier).Count();
        }


        private (List<string[]>, int) ParserDonnees()
        {
            // On crée le tableau accueillant tous les éléments du fichier
            List<string[]> Tableau = new List<string[]>();

            // On lance la lecture du fichier
            using (StreamReader Sr = new StreamReader(_CheminFichier, Encoding.Default))
            {
                // On s'occupe de connaitre le nombre de tables de la BDD
                int NombreColonnes = 0;

                // On veut aussi connaître le nombre d'erreurs lors du parse.


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

                // On parcourt toutes les lignes du fichier
                while (!Sr.EndOfStream)
                {
                    string Ligne = Sr.ReadLine();
                    Console.WriteLine(Ligne);
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
                }

                Sr.Close();
                Sr.Dispose();


            }
        }
    }
}
