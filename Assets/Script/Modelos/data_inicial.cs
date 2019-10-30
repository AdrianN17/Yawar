using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Modelos
{
    [Serializable]
    public class data_inicial
    {
        public int id { get; set; }
        public data_vector posicion { get; set; }

        public data_inicial(int id, data_vector posicion)
        {
            this.id = id;
            this.posicion = posicion;
        }
    }
}
