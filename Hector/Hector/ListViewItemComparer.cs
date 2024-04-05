using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hector
{
    /// <summary>
    /// Classe permettant de comparer des éléments d'une ListView
    /// </summary>
    class ListViewItemComparer : IComparer
    {
        // Attributs
        private int _Colonne;

        /// <summary>
        /// Constructeur par défaut de la classe ListViewItemComparer
        /// </summary>
        public ListViewItemComparer()
        {
            this.Colonne = 0;
        }

        /// <summary>
        /// Constructeur de la classe ListViewItemComparer
        /// </summary>
        /// <param name="Colonne"></param>
        public ListViewItemComparer(int Colonne)
        {
            this.Colonne = Colonne;
        }

        // Getters et Setters
        public int Colonne { get => _Colonne; set => _Colonne = value; }

        /// <summary>
        /// Méthode permettant de comparer deux éléments d'une ListView
        /// </summary>
        /// <param name="Objet1"></param>
        /// <param name="Objet2"></param>
        /// <returns></returns>
        public int Compare(object Objet1, object Objet2)
        {
            // Conversion des objets à comparer en ListViewItem
            ListViewItem ElementListe1 = (ListViewItem)Objet1;
            ListViewItem ElementListe2 = (ListViewItem)Objet2;
            // Retour du résultat de la comparaison
            return String.Compare(ElementListe1.SubItems[Colonne].Text, ElementListe2.SubItems[Colonne].Text);
        }
    }
}
