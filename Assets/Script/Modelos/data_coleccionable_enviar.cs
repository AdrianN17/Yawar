using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Modelos
{
    [Serializable]
    public class data_coleccionable_enviar
    {
        public int id { get; set; }
        public List<data_coleccionable> inventario { get; set; }

        public data_coleccionable_enviar(int id, List<data_coleccionable> inventario)
        {
            this.id = id;
            this.inventario = inventario;
        }
    }
}
