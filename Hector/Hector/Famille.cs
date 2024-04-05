using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hector
{
    /// <summary>
    /// Classe représentant une famille
    /// </summary>
    class Famille
    {
        // Attributs
        private int _RefFamille;
        private string _Nom;

        /// <summary>
        /// Constructeur de la classe Famille
        /// </summary>
        /// <param name="RefFamille">Référence de la famille</param>
        /// <param name="Nom">Nom de la famille</param>
        public Famille(int RefFamille, string Nom)
        {
            this.RefFamille = RefFamille;
            this.Nom = Nom;
        }

        // Getters et Setters
        public int RefFamille { get => _RefFamille; set => _RefFamille = value; }
        public string Nom { get => _Nom; set => _Nom = value; }
    }
}
