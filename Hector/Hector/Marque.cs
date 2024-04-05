using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hector
{
    /// <summary>
    /// Classe représentant une marque
    /// </summary>
    class Marque
    {
        // Attributs
        private int _RefMarque;
        private string _Nom;

        /// <summary>
        /// Constructeur de la classe Marque
        /// </summary>
        /// <param name="RefMarque">Reférence de la marque</param>
        /// <param name="Nom">Nom de la marque</param>
        public Marque(int RefMarque, string Nom)
        {
            this.RefMarque = RefMarque;
            this.Nom = Nom;
        }

        // Getters et Setters
        public int RefMarque { get => _RefMarque; set => _RefMarque = value; }
        public string Nom { get => _Nom; set => _Nom = value; }
    }
}
