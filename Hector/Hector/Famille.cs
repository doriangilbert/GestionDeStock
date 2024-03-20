using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hector
{
    class Famille
    {
        private int _RefFamille;
        private string _Nom;

        public Famille(int RefFamille, string Nom)
        {
            this.RefFamille = RefFamille;
            this.Nom = Nom;
        }

        public int RefFamille { get => _RefFamille; set => _RefFamille = value; }
        public string Nom { get => _Nom; set => _Nom = value; }
    }
}
