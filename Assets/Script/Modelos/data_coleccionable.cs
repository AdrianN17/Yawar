using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Modelos
{
    public class data_coleccionable
    {
        public int tipo;
        public int cantidad;
        public string cadena;

        public data_coleccionable(int tipo, int cantidad, string cadena)
        {
            this.tipo = tipo;
            this.cantidad = cantidad;
            this.cadena = cadena;
        }
    }
}
