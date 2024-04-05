using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hector
{
    /// <summary>
    /// Classe représentant une sous-famille
    /// </summary>
    class SousFamille
    {
        // Attributs
        private int _RefSousFamille;
        private Famille _FamilleSousFamille;
        private string _Nom;

        /// <summary>
        /// Constructeur de la classe SousFamille
        /// </summary>
        /// <param name="RefSousFamille">Reférence de la sous-famille</param>
        /// <param name="FamilleSousFamille">Famille de la sous-famille</param>
        /// <param name="Nom">Nom de la sous-famille</param>
        public SousFamille(int RefSousFamille, Famille FamilleSousFamille, string Nom)
        {
            this.RefSousFamille = RefSousFamille;
            this.FamilleSousFamille = FamilleSousFamille;
            this.Nom = Nom;
        }

        // Getters et Setters
        public int RefSousFamille { get => _RefSousFamille; set => _RefSousFamille = value; }
        public string Nom { get => _Nom; set => _Nom = value; }
        internal Famille FamilleSousFamille { get => _FamilleSousFamille; set => _FamilleSousFamille = value; }
    }
}
