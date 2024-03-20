using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hector
{
    class SousFamille
    {
        private int _RefSousFamille;
        private Famille _FamilleSousFamille;
        private string _Nom;

        public SousFamille(int RefSousFamille, Famille FamilleSousFamille, string Nom)
        {
            this.RefSousFamille = RefSousFamille;
            this.FamilleSousFamille = FamilleSousFamille;
            this.Nom = Nom;
        }

        public int RefSousFamille { get => _RefSousFamille; set => _RefSousFamille = value; }
        public string Nom { get => _Nom; set => _Nom = value; }
        internal Famille FamilleSousFamille { get => _FamilleSousFamille; set => _FamilleSousFamille = value; }
    }
}
