using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hector
{
    class Article
    {
        private string _RefArticle;
        private string _Description;
        private SousFamille _SousFamilleArticle;
        private Marque _MarqueArticle;
        private float _PrixHT;
        private int _Quantite;

        public Article(string RefArticle, string Description, SousFamille SousFamilleArticle, Marque MarqueArticle, float PrixHT, int Quantite)
        {
            this.RefArticle = RefArticle;
            this.Description = Description;
            this.SousFamilleArticle = SousFamilleArticle;
            this.MarqueArticle = MarqueArticle;
            this.PrixHT = PrixHT;
            this.Quantite = Quantite;
        }

        public string RefArticle { get => _RefArticle; set => _RefArticle = value; }
        public string Description { get => _Description; set => _Description = value; }
        public float PrixHT { get => _PrixHT; set => _PrixHT = value; }
        public int Quantite { get => _Quantite; set => _Quantite = value; }
        internal SousFamille SousFamilleArticle { get => _SousFamilleArticle; set => _SousFamilleArticle = value; }
        internal Marque MarqueArticle { get => _MarqueArticle; set => _MarqueArticle = value; }
    }
}
