using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hector
{
    /// <summary>
    /// Classe représentant un article
    /// </summary>
    class Article
    {
        // Attributs
        private string _RefArticle;
        private string _Description;
        private SousFamille _SousFamilleArticle;
        private Marque _MarqueArticle;
        private float _PrixHT;
        private int _Quantite;

        /// <summary>
        /// Constructeur de la classe Article
        /// </summary>
        /// <param name="RefArticle"></param>
        /// <param name="Description"></param>
        /// <param name="SousFamilleArticle"></param>
        /// <param name="MarqueArticle"></param>
        /// <param name="PrixHT"></param>
        /// <param name="Quantite"></param>
        public Article(string RefArticle, string Description, SousFamille SousFamilleArticle, Marque MarqueArticle, float PrixHT, int Quantite)
        {
            this.RefArticle = RefArticle;
            this.Description = Description;
            this.SousFamilleArticle = SousFamilleArticle;
            this.MarqueArticle = MarqueArticle;
            this.PrixHT = PrixHT;
            this.Quantite = Quantite;
        }

        // Getters et Setters
        public string RefArticle { get => _RefArticle; set => _RefArticle = value; }
        public string Description { get => _Description; set => _Description = value; }
        public float PrixHT { get => _PrixHT; set => _PrixHT = value; }
        public int Quantite { get => _Quantite; set => _Quantite = value; }
        internal SousFamille SousFamilleArticle { get => _SousFamilleArticle; set => _SousFamilleArticle = value; }
        internal Marque MarqueArticle { get => _MarqueArticle; set => _MarqueArticle = value; }
    }
}
