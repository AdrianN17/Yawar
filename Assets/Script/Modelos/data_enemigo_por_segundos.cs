using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Modelos
{
    public class data_enemigo_por_segundos
    {
        public int id { get; set; }
        public Vector3 pos { get; set; }
        public int vida { get; set; }
        public Vector3 radio { get; set; }
        public int coleccionable { get; set; }

        public data_enemigo_por_segundos(int id, Vector3 pos, int vida, Vector3 radio, int coleccionable)
        {
            this.id = id;
            this.pos = pos;
            this.vida = vida;
            this.radio = radio;
            this.coleccionable = coleccionable;
        }
    }
}
