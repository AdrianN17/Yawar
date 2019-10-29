using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Modelos
{
    public class data_por_segundos
    {
        public int id { get; set; }
        public Vector3 posicion { get; set; }

        public Vector3 radio { get; set; }

        public int arma;

        public data_por_segundos(int id, Vector3 posicion, Vector3 radio, int arma)
        {
            this.id = id;
            this.posicion = posicion;
            this.radio = radio;
            this.arma = arma;
        }
    }
}
