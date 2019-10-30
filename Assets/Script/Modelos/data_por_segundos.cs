using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Modelos
{
    [Serializable]
    public class data_por_segundos
    {
        public int id { get; set; }
        public data_vector posicion { get; set; }
        public data_vector radio { get; set; }

        public int arma;

        public data_por_segundos(int id, data_vector posicion, data_vector radio, int arma)
        {
            this.id = id;
            this.posicion = posicion;
            this.radio = radio;
            this.arma = arma;
        }
    }
}
