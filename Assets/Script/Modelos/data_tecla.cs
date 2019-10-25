using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Modelos
{
    public class data_tecla
    {
        public int id;
        public string tecla;
        public string orientacion;

        public data_tecla(int id, string tecla, string orientacion)
        {
            this.id = id;
            this.tecla = tecla;
            this.orientacion = orientacion;
        }
    }
}
