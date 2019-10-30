using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Modelos
{
    [Serializable]
    public class data_enemigo_por_segundos
    {
        public int id { get; set; }
        public data_vector pos { get; set; }
        public int vida { get; set; }
        public data_vector radio { get; set; }
        public int coleccionable { get; set; }

        public data_enemigo_por_segundos(int id, data_vector pos, int vida, data_vector radio, int coleccionable)
        {
            this.id = id;
            this.pos = pos;
            this.vida = vida;
            this.radio = radio;
            this.coleccionable = coleccionable;
        }
    }
}
