using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Modelos
{
    [Serializable]
    public class data_colecionable_con_id
    {
        public int id_colecionable { get; set; }
        public int tipo { get; set; }
        public int cantidad { get; set; }
        public data_vector vector { get; set; }

        public data_colecionable_con_id(int id_colecionable, int tipo, int cantidad, data_vector vector)
        {
            this.id_colecionable = id_colecionable;
            this.tipo = tipo;
            this.cantidad = cantidad;
            this.vector = vector;
        }
    }
}
