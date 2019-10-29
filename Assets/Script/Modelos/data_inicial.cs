using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Modelos
{
    public class data_inicial
    {
        public int id { get; set; }
        public Vector3 posicion { get; set; }

        public data_inicial(int id,  Vector3 posicion)
        {
            this.id = id;
            this.posicion = posicion;
        }
    }
}
