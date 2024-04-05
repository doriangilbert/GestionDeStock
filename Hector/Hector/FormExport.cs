using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hector
{
    /// <summary>
    /// Classe de la fenêtre modale permettant d'exporter les données de la base de données dans un fichier "csv".
    /// </summary>
    public partial class FormExport : Form
    {
        private string _CheminFichier;

        /// <summary>
        /// Pour lancer la fenêtre (constructeur par défaut)
        /// </summary>
        public FormExport()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Permet d'ouvrir une fenêtre pour choisir un fichier csv. 
        /// Une fois fait, on prélève le chemin ainsi que le nom du fichier sélectionné.
        /// </summary>
        /// <param name="Sender">Objet <b>Object</b> prend en charge les éventuels objets que l'on renseigne en paramètre.</param>
        /// <param name="Args">Objet <b>EventArgs</b> contient les informations sur l'évènement.</param>
        private void ChoisirFichier_Click(object sender, EventArgs e)
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
                this.LabelStatus.Text = "Prêt pour l'exportation...";
            }
        }


        /// <summary>
        /// On se connecte à la base de données puis on recueille toutes les données de cette dernière.
        /// Ensuite, on les transvase dans un fichier csv.
        /// </summary>
        /// <param name="Sender">Objet <b>Object</b> prend en charge les éventuels objets que l'on renseigne en paramètre.</param>
        /// <param name="Args">Objet <b>EventArgs</b> contient les informations sur l'évènement.</param>
        private void ExportationDonnees_Click(object sender, EventArgs e)
        {
            this.BarreDeProgression.Value = 0;

            // On regarde si un fichier a été choisi au préalable.
            if (!string.IsNullOrEmpty(_CheminFichier))
            {
                // On trouve le nombre total de lignes de la BDD.
                int NombreLignes = 0;

                // On se connecte à la BDD.
                using (SQLiteConnection Connexion = new SQLiteConnection("Data Source=Hector.SQLite"))
                {
                    Connexion.Open();

                    // On modifie le label pour indiquer que l'on lit les données de la BDD
                    this.LabelStatus.Text = "Lecture des données";

                    // On récupère le nombre d'articles dans la BDD.
                    string RequeteCompte = "SELECT COUNT(*) AS NbArticles FROM Articles";

                    using (SQLiteCommand Commande = new SQLiteCommand(RequeteCompte, Connexion))
                    {
                        SQLiteDataReader Lecteur = Commande.ExecuteReader();

                        while (Lecteur.Read())
                        {
                            NombreLignes = Convert.ToInt32(Lecteur["NbArticles"]);
                        }
                    }

                    // On veut récupérer tous les éléments à placer dans le fichier csv: Description, Ref article, Marque, Famille, Sous-Famille, Prix article.
                    string RequeteDonnees = "SELECT Articles.Description AS Description_Article, Articles.RefArticle AS Ref_Article, Articles.PrixHT AS Prix_Article, " +
                        "Marques.Nom AS Nom_Marque, Familles.Nom AS Nom_Famille, SousFamilles.Nom AS Nom_SousFamille FROM Articles " +
                        "JOIN Marques ON Articles.RefMarque = Marques.RefMarque " +
                        "JOIN SousFamilles ON Articles.RefSousFamille = SousFamilles.RefSousFamille " +
                        "JOIN Familles ON SousFamilles.RefFamille = Familles.RefFamille";

                    using (SQLiteCommand Commande = new SQLiteCommand(RequeteDonnees, Connexion))
                    {
                        SQLiteDataReader Lecteur = Commande.ExecuteReader();

                        // On écrit via le StreamWriter. On met "false" en paramètre pour écraser le fichier.
                        using (StreamWriter Sw = new StreamWriter(_CheminFichier, false, Encoding.Default))
                        {
                            Sw.WriteLine("Description;Ref;Marque;Famille;Sous-Famille;Prix H.T.");
                            while (Lecteur.Read())
                            {
                                Sw.WriteLine(Lecteur["Description_Article"] + ";" + 
                                    Lecteur["Ref_Article"] + ";" + 
                                    Lecteur["Nom_Marque"] + ";" + 
                                    Lecteur["Nom_Famille"] + ";" + 
                                    Lecteur["Nom_SousFamille"] + ";" +
                                    Lecteur["Prix_Article"].ToString().Replace('.', ','));
                            }
                        }
                    }
                }
            }
        }
    }
}
