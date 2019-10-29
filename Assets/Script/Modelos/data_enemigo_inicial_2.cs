using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Modelos
{
    public class data_enemigo_inicial_2
    {
        public int id { get; set; }
        public Vector3 pos { get; set; }
        public Vector3 radio { get; set; }
        public int coleccionable { get; set; }
        public int vidas { get; set; }

        public data_enemigo_inicial_2(int id, Vector3 pos, Vector3 radio, int coleccionable, int vidas)
        {
            this.id = id;
            this.pos = pos;
            this.radio = radio;
            this.coleccionable = coleccionable;
            this.vidas = vidas;
        }
    }
}
