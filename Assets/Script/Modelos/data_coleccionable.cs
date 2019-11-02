using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Modelos
{
    [Serializable]
    public class data_coleccionable
    {
        public int tipo { get; set; }
        public int cantidad { get; set; }
        public string cadena { get; set; }

        public data_coleccionable(int tipo, int cantidad, string cadena)
        {
            this.tipo = tipo;
            this.cantidad = cantidad;
            this.cadena = cadena;
        }
    }
}
