using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hector
{
    class Marque
    {
        private int _RefMarque;
        private string _Nom;

        public Marque(int RefMarque, string Nom)
        {
            this.RefMarque = RefMarque;
            this.Nom = Nom;
        }

        public int RefMarque { get => _RefMarque; set => _RefMarque = value; }
        public string Nom { get => _Nom; set => _Nom = value; }
    }
}
