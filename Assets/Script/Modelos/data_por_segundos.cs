using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Modelos
{
    class data_por_segundos
    {
        public int id;
        public Vector3 posicion;

        public Vector3 radio;

        public data_por_segundos(int id, Vector3 posicion, Vector3 radio)
        {
            this.id = id;
            this.posicion = posicion;
            this.radio = radio;
        }
    }
}
